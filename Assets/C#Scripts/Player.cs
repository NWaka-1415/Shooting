using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 正の値で頼む
    /// </summary>
    private float _speed;

    private int _hp;

    [SerializeField] private GameObject _shotPrefab = null;

    private GameSceneManager _gameSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        _speed = 0.1f;
        _hp = 100;
        _gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        if (_gameSceneManager == null) Debug.LogError("Error!");
        _gameSceneManager.SetHitPoint(_hp);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //左矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(
                    Mathf.Clamp(pos.x - _speed, _gameSceneManager.LeftCameraWorldPos,
                        _gameSceneManager.RightCameraWorldPos), pos.y);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //右矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(
                    Mathf.Clamp(pos.x + _speed, _gameSceneManager.LeftCameraWorldPos,
                        _gameSceneManager.RightCameraWorldPos), pos.y);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //上矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(pos.x,
                    Mathf.Clamp(pos.y + _speed, _gameSceneManager.BottomCameraWorldPos,
                        _gameSceneManager.TopCameraWorldPos));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //下矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(pos.x,
                    Mathf.Clamp(pos.y - _speed, _gameSceneManager.BottomCameraWorldPos,
                        _gameSceneManager.TopCameraWorldPos));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ゲームオブジェクトを作成
            GameObject shotInstance = Instantiate(_shotPrefab);
            //ポジションをShooterにする
            shotInstance.transform.position = gameObject.transform.position;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //ぶつかったのがEnemyだったら
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            _hp -= enemy.AttackPower;
            if (_hp <= 0) _hp = 0;
            _gameSceneManager.SetHitPoint(_hp);
        }
    }
}