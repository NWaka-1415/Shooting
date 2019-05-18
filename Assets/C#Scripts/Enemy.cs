using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _attackPower = 10;
    private Rigidbody2D _rigidbody2D;
    private GameSceneManager _gameSceneManager;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
    }

    void Update()
    {
        if (transform.position.y <= _gameSceneManager.BottomCameraWorldPos)
        {
            _gameSceneManager.GameOver();
        }
    }

    public void Initialize(Vector2 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);

        _rigidbody2D.velocity = new Vector2(0f, -1f);
    }

    public int AttackPower => _attackPower;
}