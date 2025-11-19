using UnityEngine;

namespace PlatformJump.Player
{
    public class PlayerView : MonoBehaviour
    {
        public Rigidbody2D Body => _body;
        public Vector2 Position => transform.position;
        public Collision2D Collision { get; private set; }

        [SerializeField]
        private Rigidbody2D _body;

        private void OnCollisionEnter2D(Collision2D other)
        {
            Collision = other;
        }

        private void OnCollisionExit2D(Collision2D _)
        {
            Collision = null;
        }
    }
}