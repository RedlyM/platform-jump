using System;
using System.Collections.Generic;
using PlatformJump.Helpers;
using PlatformJump.Inputs;
using R3;
using UnityEngine;

namespace PlatformJump.Player
{
    public class PlayerPresenter : IDisposable
    {
        private readonly PlatformHelper _platformHelper;
        private readonly PlayerConfig _playerConfig;
        private readonly PlayerView _player;
        private readonly IUserInput[] _inputs;

        private float _directionX;
        private bool _needJump;

        private List<IDisposable> _disposables;

        public PlayerPresenter(PlatformHelper platformHelper, PlayerConfig playerConfig, PlayerView player,
            params IUserInput[] inputs)
        {
            _platformHelper = platformHelper;
            _playerConfig = playerConfig;
            _player = player;
            _inputs = inputs;
        }

        public void Initialize()
        {
            var updateInput = Observable.EveryUpdate(UnityFrameProvider.Update).Subscribe(UpdateInput);
            var simulatePhysics = Observable.EveryUpdate(UnityFrameProvider.FixedUpdate).Subscribe(SimulatePhysics);

            _disposables = new List<IDisposable> { updateInput, simulatePhysics };

            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i].Jump += Jump;
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i].Jump -= Jump;
            }

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }

        private void Jump()
        {
            _needJump = true;
        }

        private void UpdateInput(Unit obj)
        {
            var max = 0f;

            for (int i = 0; i < _inputs.Length; i++)
            {
                if (Mathf.Abs(_inputs[i].DirectionX) > max)
                {
                    max = _inputs[i].DirectionX;
                    break;
                }
            }

            _directionX = max;
        }

        private void SimulatePhysics(Unit _)
        {
            //симуляция притяжения
            var perimeterPointWorld = _platformHelper.ComputeNearestPerimeterPoint(_player.Position);
            var gDir = perimeterPointWorld - _player.Position;
            var gdMag = gDir.magnitude;

            if (gdMag < Mathf.Epsilon)
            {
                return;
            }

            gDir /= gdMag;
            _player.Body.AddForce(gDir * _playerConfig.GravityStrength * _player.Body.mass, ForceMode2D.Force);

            //движение с помощью физики
            var tangent = new Vector2(-gDir.y, gDir.x).normalized;
            if (Mathf.Abs(_directionX) > Mathf.Epsilon)
            {
                _player.Body.AddForce(tangent * _directionX * _playerConfig.HorizontalAccel, ForceMode2D.Force);
            }

            if (_needJump)
            {
                if (IsGrounded())
                {
                    var jumpDir = -gDir;
                    var currentAlong = Vector2.Dot(_player.Body.velocity, jumpDir);
                    var delta = _playerConfig.JumpVelocity - currentAlong;
                    _player.Body.velocity += jumpDir * delta;
                }

                _needJump = false;
            }

            //Ограничение скорости
            var vx = Vector2.Dot(_player.Body.velocity, tangent);
            var clamped = Mathf.Clamp(vx, -_playerConfig.MaxHorizontalSpeed, _playerConfig.MaxHorizontalSpeed);
            _player.Body.velocity += tangent * (clamped - vx);

            //Самоторможение если стоим на поверхности
            if (IsGrounded() && Mathf.Abs(_directionX) < Mathf.Epsilon)
            {
                var vxNow = Vector2.Dot(_player.Body.velocity, tangent);
                if (Mathf.Abs(vxNow) <= _playerConfig.MinStopSpeed)
                {
                    _player.Body.velocity -= tangent * vxNow;
                }
                else
                {
                    var newVx = Mathf.MoveTowards(vxNow, 0f, _playerConfig.BrakeSpeed * Time.fixedDeltaTime);
                    _player.Body.velocity += tangent * (newVx - vxNow);
                }
            }
        }

        private bool IsGrounded()
        {
            return _player.Collision != null;
        }
    }
}