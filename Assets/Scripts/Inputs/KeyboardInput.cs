using System;
using UnityEngine;

namespace PlatformJump.Inputs
{
    public class KeyboardInput : MonoBehaviour, IUserInput
    {
        public event Action Jump;
        public float DirectionX { get; private set; }

        [SerializeField]
        private KeyCode _left;

        [SerializeField]
        private KeyCode _right;

        [SerializeField]
        private KeyCode _jump;

        void Update()
        {
            var directionX = 0f;

            if (Input.GetKey(_left))
            {
                directionX = -1f;
            }

            if (Input.GetKey(_right))
            {
                directionX = 1f;
            }

            DirectionX = directionX != 0f ? directionX : 0f;

            if (Input.GetKeyDown(_jump))
            {
                Jump?.Invoke();
            }
        }
    }
}