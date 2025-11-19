using System;

namespace PlatformJump.Inputs
{
    public interface IUserInput
    {
        public event Action Jump;
        public float DirectionX { get; }
    }
}