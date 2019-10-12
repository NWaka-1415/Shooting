using UnityEngine;

namespace Objects
{
    public class BackGroundObjects : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;

        public void Initialize(Vector2 pos, Sprite sprite, Vector2 speed)
        {
            transform.position = pos;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer.sprite = sprite;
            _rigidbody2D.velocity = speed;
        }

        public bool CheckPos(float pos)
        {
            return transform.position.y <= pos;
        }
    }
}