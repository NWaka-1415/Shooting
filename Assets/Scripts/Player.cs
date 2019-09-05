using System.Collections;
using System.Collections.Generic;
using Controller;
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

    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private int fadeCount = 2;

    private Color _myColor;
    private float _alpha;
    private bool _fadeReactionFlag;
    private bool _fadeUp;
    private int _fadingCount;

    private float _currentInterval;
    private bool _shotFlag;

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
        if (_fadeReactionFlag) FadeReaction();
        else spriteRenderer.color = _myColor;

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
        _shotFlag = false;
        _fadeReactionFlag = false;
        _fadeUp = false;
        _currentInterval = 0f;
        _fadingCount = 0;
        _myColor = spriteRenderer.color;
        _alpha = _myColor.a;
        GameSceneController.Instance.SetHitPoint(_hp);
    }

    protected void CalcFiringInterval()
    {
        if (!_shotFlag) return;
        if (_currentInterval >= firingInterval)
        {
            _shotFlag = false;
            _currentInterval = 0f;
        }
        else _currentInterval += Time.deltaTime;
    }

    protected void Shoot()
    {
        if(GameSceneController.Instance.IsGameOver) return;
        if (_shotFlag) return;
        //ゲームオブジェクトを作成
        Bullet bulletInstanceScript = Instantiate(_shotPrefab).GetComponent<Bullet>();
        //ポジションをShooterにする
        bulletInstanceScript.Initialize(transform.position);
        _shotFlag = true;
    }

    protected void Move(Direction direction)
    {
        Vector2 pos = transform.position;
        switch (direction)
        {
            case Direction.Top:
                transform.position =
                    new Vector3(pos.x,
                        Mathf.Clamp(pos.y + _speed, GameSceneController.Instance.BottomCameraWorldPos,
                            GameSceneController.Instance.TopCameraWorldPos));
                break;
            case Direction.Bottom:
                transform.position =
                    new Vector3(pos.x,
                        Mathf.Clamp(pos.y - _speed, GameSceneController.Instance.BottomCameraWorldPos,
                            GameSceneController.Instance.TopCameraWorldPos));
                break;
            case Direction.Left:
                transform.position =
                    new Vector3(
                        Mathf.Clamp(pos.x - _speed, GameSceneController.Instance.LeftCameraWorldPos,
                            GameSceneController.Instance.RightCameraWorldPos), pos.y);
                break;
            case Direction.Right:
                transform.position =
                    new Vector3(
                        Mathf.Clamp(pos.x + _speed, GameSceneController.Instance.LeftCameraWorldPos,
                            GameSceneController.Instance.RightCameraWorldPos), pos.y);
                break;
        }
    }

    private void FadeReaction()
    {
        if (_fadeUp)
        {
            _alpha += 0.25f;
            if (_alpha >= 1)
            {
                _fadeUp = false;
                _fadingCount++;
            }
        }
        else
        {
            _alpha -= 0.25f;
            if (_alpha <= 0) _fadeUp = true;
        }

        spriteRenderer.color = new Color(_myColor.r, _myColor.g - 50, _myColor.b - 50, _alpha);
        if (_fadingCount >= fadeCount)
        {
            _fadingCount = 0;
            _fadeUp = false;
            _alpha = _myColor.a;
            _fadeReactionFlag = false;
        }
    }

    /// <summary>
    /// 当たり判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_fadeReactionFlag) return;
        if (other.CompareTag("Enemy"))
        {
            //ぶつかったのがEnemyだったら
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            _fadeReactionFlag = true;
            _hp -= enemy.AttackPower;
            if (_hp <= 0)
            {
                _hp = 0;
                GameSceneController.Instance.GameOver();
            }

            GameSceneController.Instance.SetHitPoint(_hp);
        }
    }
}