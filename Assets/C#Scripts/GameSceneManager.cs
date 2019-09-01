using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private Text _hpText = null;
    [SerializeField] private GameObject _enemyPrefab = null;

    [SerializeField] private GameObject _backGroundPrefab = null;

    [SerializeField] private GameObject[] _backGroundImages = null;

    [SerializeField] Sprite[] _rockAndPlanetSprites = new Sprite[0];

    [SerializeField] private int _enemyNumber = 10;
    [SerializeField] private GameObject _gameOverPanel = null;
    [SerializeField] private GameObject _gameClearPanel = null;

    private Camera _camera;

    private static GameSceneManager _instance = null;

    public static GameSceneManager Instance => _instance;

    private bool _isGameOver;
    private bool _isGameClear;

    private float _time;

    private float _leftCameraWorldPos;
    private float _rightCameraWorldPos;
    private float _topCameraWorldPos;
    private float _bottomCameraWorldPos;

    private Vector2[] _backGroundDefaultPos;

    private List<Enemy> _enemies;
    private List<BackGroundObjects> _backGroundObjects;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        _isGameOver = false;
        _isGameClear = false;
        _time = 0.5f;
        _gameOverPanel.SetActive(false);
        _gameClearPanel.SetActive(false);

        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _leftCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).x;
        _rightCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).x;
        _topCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).y;
        _bottomCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).y;

        _backGroundDefaultPos = new Vector2[_backGroundImages.Length];
        for (int i = 0; i < _backGroundImages.Length; i++)
        {
            _backGroundDefaultPos[i] = _backGroundImages[i].transform.position;
        }

        _enemies = new List<Enemy>();
        _backGroundObjects = new List<BackGroundObjects>();

        for (int i = 0; i < _enemyNumber; i++)
        {
            Enemy enemy = Instantiate(_enemyPrefab).GetComponent<Enemy>();
            enemy.Initialize(ResetPos());
            _enemies.Add(enemy);
        }

        for (int i = 0; i < 10; i++)
        {
            GameObject bgGameObject = Instantiate(_backGroundPrefab);
            BackGroundObjects backGroundObjects = bgGameObject.GetComponent<BackGroundObjects>();
            backGroundObjects.Initialize(ResetPos(),
                _rockAndPlanetSprites[Random.Range(0, _rockAndPlanetSprites.Length)],
                new Vector2(0f, -Random.Range(2f, 5f)));
            _backGroundObjects.Add(backGroundObjects);
        }

        foreach (Enemy enemy in _enemies)
        {
            enemy.Initialize(ResetPos());
        }
    }

    private void Update()
    {
        //背景
        for (int i = 0; i < _backGroundImages.Length; i++)
        {
            if (_backGroundImages[i].transform.position.y <= _bottomCameraWorldPos - 0.5f)
            {
                int index = i - 1 >= 0 ? i - 1 : _backGroundImages.Length - 1;
                Debug.Log(index);
                _backGroundImages[i].transform.position =
                    _backGroundDefaultPos[index] +
                    new Vector2(0f, _backGroundImages[i].transform.localScale.y);
            }

            _backGroundImages[i].transform.Translate(0f, -0.2f, 0f);
            _backGroundDefaultPos[i] = _backGroundImages[i].transform.position;
        }

        //背景の岩石とか
        foreach (BackGroundObjects backGroundObject in _backGroundObjects)
        {
            if (backGroundObject.CheckPos(_bottomCameraWorldPos))
            {
                backGroundObject.Initialize(ResetPos(),
                    _rockAndPlanetSprites[Random.Range(0, _rockAndPlanetSprites.Length)],
                    new Vector2(0f, -Random.Range(2f, 5f)));
            }
        }

        //Enemy
        foreach (Enemy enemy in _enemies)
        {
            if (enemy.CheckPos(_bottomCameraWorldPos)) GameOver();
            if (!enemy.gameObject.activeSelf) enemy.Initialize(ResetPos());
        }

        //スタート画面に戻す
        if (_time <= 0) SceneManager.LoadSceneAsync("StartScene");
        if (_isGameClear) _time -= Time.deltaTime;
        else if (_isGameOver) _time -= Time.deltaTime;
    }

    private Vector3 ResetPos()
    {
        return new Vector3(Random.Range(_leftCameraWorldPos, _rightCameraWorldPos),
            Random.Range(_topCameraWorldPos, _topCameraWorldPos + 5f));
    }

    public void SetHitPoint(int hp)
    {
        _hpText.text = $" Hp:{hp}";
    }

    public void GameOver()
    {
        _isGameOver = true;
        _gameOverPanel.SetActive(true);
    }

    public void GameClear()
    {
        _isGameClear = true;
        _gameClearPanel.SetActive(true);
    }

    public float LeftCameraWorldPos => _leftCameraWorldPos;

    public float RightCameraWorldPos => _rightCameraWorldPos;

    public float TopCameraWorldPos => _topCameraWorldPos;

    public float BottomCameraWorldPos => _bottomCameraWorldPos;
}