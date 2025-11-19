using System;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlatformJump.Inputs
{
    public class TouchInput : MonoBehaviour, IUserInput
    {
        public event Action Jump;
        public float DirectionX { get; private set; }

        [SerializeField]
        private Button _left;

        [SerializeField]
        private Button _right;

        [SerializeField]
        private Button _jump;

        private void Awake()
        {
            _left.OnPointerDownAsObservable().Subscribe(MoveLeft);
            _left.OnPointerUpAsObservable().Subscribe(ResetMove);
            _right.OnPointerDownAsObservable().Subscribe(MoveRight);
            _right.OnPointerUpAsObservable().Subscribe(ResetMove);
            _jump.onClick.AddListener(MakeJump);
        }

        private void MakeJump()
        {
            Jump?.Invoke();
        }

        private void MoveRight(PointerEventData _)
        {
            DirectionX = 1.0f;
        }

        private void MoveLeft(PointerEventData _)
        {
            DirectionX = -1.0f;
        }

        private void ResetMove(PointerEventData _)
        {
            DirectionX = 0.0f;
        }
    }
}