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

    // Start is called before the first frame update
    void Start()
    {
        _speed = 0.1f;
        _hp = 100;
        GameSceneManager.Instance.SetHitPoint(_hp);
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
                    Mathf.Clamp(pos.x - _speed, GameSceneManager.Instance.LeftCameraWorldPos,
                        GameSceneManager.Instance.RightCameraWorldPos), pos.y);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //右矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(
                    Mathf.Clamp(pos.x + _speed, GameSceneManager.Instance.LeftCameraWorldPos,
                        GameSceneManager.Instance.RightCameraWorldPos), pos.y);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //上矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(pos.x,
                    Mathf.Clamp(pos.y + _speed, GameSceneManager.Instance.BottomCameraWorldPos,
                        GameSceneManager.Instance.TopCameraWorldPos));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            //下矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(pos.x,
                    Mathf.Clamp(pos.y - _speed, GameSceneManager.Instance.BottomCameraWorldPos,
                        GameSceneManager.Instance.TopCameraWorldPos));
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
            if (_hp <= 0)
            {
                _hp = 0;
                GameSceneManager.Instance.GameOver();
            }
            GameSceneManager.Instance.SetHitPoint(_hp);
        }
    }
}