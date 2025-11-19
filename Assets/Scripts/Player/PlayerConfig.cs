using UnityEngine;

namespace PlatformJump.Player
{
    [CreateAssetMenu(fileName = "MoveConfig", menuName = "PlatformJump/MoveConfig in PlatformJump")]
    public class PlayerConfig : ScriptableObject
    {
        public float GravityStrength => _gravityStrength;

        public float HorizontalAccel => _horizontalAccel;

        public float MaxHorizontalSpeed => _maxHorizontalSpeed;

        public float JumpVelocity => _jumpVelocity;

        public float BrakeSpeed => _brakeSpeed;

        public float MinStopSpeed => _minStopSpeed;

        [Header("Gravity")]
        [SerializeField]
        private float _gravityStrength = 9.81f;

        [Header("Movement")]
        [SerializeField]
        private float _horizontalAccel = 8f;

        [SerializeField]
        private float _maxHorizontalSpeed = 4f;

        [Header("Jump / Ground")]
        [SerializeField]
        private float _jumpVelocity = 3f;

        [Header("Braking (ground only)")]
        [SerializeField]
        private float _brakeSpeed = 3f;

        [SerializeField]
        private float _minStopSpeed = 0.05f;
    }
}