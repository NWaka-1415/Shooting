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

    /// <summary>
    /// 発射間隔
    /// </summary>
    [SerializeField] private float firingInterval = 0.5f;

    private float _currentInterval;
    private bool _shot;

    protected enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        CalcFiringInterval();
        #region PC

        //左矢印キーが押されたら
        if (Input.GetKey(KeyCode.LeftArrow)) Move(Direction.Left);

        //右矢印キーが押されたら
        if (Input.GetKey(KeyCode.RightArrow)) Move(Direction.Right);

        //上矢印キーが押されたら
        if (Input.GetKey(KeyCode.UpArrow)) Move(Direction.Top);

        //下矢印キーが押されたら
        if (Input.GetKey(KeyCode.DownArrow)) Move(Direction.Bottom);

        if (Input.GetKey(KeyCode.Space)) Shoot();

        #endregion
    }

    public void Initialize()
    {
        _speed = 0.1f;
        _hp = 100;
        _shot = false;
        _currentInterval = 0f;
        GameSceneManager.Instance.SetHitPoint(_hp);
    }

    protected void CalcFiringInterval()
    {
        if (!_shot) return;
        if (_currentInterval >= firingInterval)
        {
            _shot = false;
            _currentInterval = 0f;
        }
        else _currentInterval += Time.deltaTime;
    }

    protected void Shoot()
    {
        if (_shot) return;
        //ゲームオブジェクトを作成
        GameObject shotInstance = Instantiate(_shotPrefab);
        //ポジションをShooterにする
        shotInstance.transform.position = gameObject.transform.position;
        _shot = true;
    }

    protected void Move(Direction direction)
    {
        Vector2 pos = transform.position;
        switch (direction)
        {
            case Direction.Top:
                transform.position =
                    new Vector3(pos.x,
                        Mathf.Clamp(pos.y + _speed, GameSceneManager.Instance.BottomCameraWorldPos,
                            GameSceneManager.Instance.TopCameraWorldPos));
                break;
            case Direction.Bottom:
                transform.position =
                    new Vector3(pos.x,
                        Mathf.Clamp(pos.y - _speed, GameSceneManager.Instance.BottomCameraWorldPos,
                            GameSceneManager.Instance.TopCameraWorldPos));
                break;
            case Direction.Left:
                transform.position =
                    new Vector3(
                        Mathf.Clamp(pos.x - _speed, GameSceneManager.Instance.LeftCameraWorldPos,
                            GameSceneManager.Instance.RightCameraWorldPos), pos.y);
                break;
            case Direction.Right:
                transform.position =
                    new Vector3(
                        Mathf.Clamp(pos.x + _speed, GameSceneManager.Instance.LeftCameraWorldPos,
                            GameSceneManager.Instance.RightCameraWorldPos), pos.y);
                break;
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