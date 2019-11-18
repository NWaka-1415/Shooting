using System;
using Controller;
using UnityEngine;

namespace Objects
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int _attackPower = 10;
        private Rigidbody2D _rigidbody2D;

        private float _speed;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _speed = 0f;
        }

        public Enemy Initialize(Vector2 pos)
        {
            transform.position = pos;
            gameObject.SetActive(true);
            
            _rigidbody2D.velocity = new Vector2(0f, _speed);
            return this;
        }

        public void SetSpeed(float speed = -1f)
        {
            if (speed >= 0) speed *= -1f;
            _speed = speed;
            _rigidbody2D.velocity = new Vector2(0f, _speed);
        }
        
        public bool CheckPos(float pos)
        {
            return transform.position.y <= pos;
        }

        public int AttackPower => _attackPower;
    }
}