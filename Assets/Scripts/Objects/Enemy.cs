using UnityEngine;

namespace Objects
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int _attackPower = 10;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Vector2 pos)
        {
            transform.position = pos;
            gameObject.SetActive(true);

            _rigidbody2D.velocity = new Vector2(0f, -1f);
        }

        public bool CheckPos(float pos)
        {
            return transform.position.y <= pos;
        }

        public int AttackPower => _attackPower;
    }
}