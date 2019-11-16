using System.Collections;
using System.Collections.Generic;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Stage = Stage.Stage;

namespace Controller
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField] private Text _hpText = null;
        [SerializeField] private Text killCountText = null;

        [SerializeField] private GameObject _enemyPrefab = null;

        [SerializeField] private GameObject _backGroundPrefab = null;

        [SerializeField] private GameObject[] _backGroundImages = null;

        [SerializeField] Sprite[] _rockAndPlanetSprites = new Sprite[0];

        [SerializeField] private int _enemyNumber = 10;
        [SerializeField] private GameObject controllerForSmartPhone = null;
        [SerializeField] private GameObject pausePanel = null;
        [SerializeField] private GameObject _gameOverPanel = null;
        [SerializeField] private GameObject _gameClearPanel = null;

        [SerializeField] private Button pauseButton = null;
        [SerializeField] private Button okButton = null;
        [SerializeField] private Button shootButton = null;

        [SerializeField] private Player player = null;

        public Button ShootButton => shootButton;

        private Camera _camera;

        private static GameSceneController _instance = null;

        public static GameSceneController Instance => _instance;

        private bool _isGameOver;
        public bool IsGameOver => _isGameOver;
        private bool _isGameClear;

        private bool _isPause;

        public bool IsPause => _isPause;

        private float _time;

        private float _leftCameraWorldPos;
        private float _rightCameraWorldPos;
        private float _topCameraWorldPos;
        private float _bottomCameraWorldPos;

        private Vector2[] _backGroundDefaultPos;

        private List<Enemy> _enemies;
        private List<BackGroundObjects> _backGroundObjects;

        private int _pushedEnemyNumber;

        private global::Stage.Stage _currentStage;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(this.gameObject);
            _currentStage = DataController.Instance.Stages[DataController.Instance.SelectStage];
        }

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_ANDROID
            controllerForSmartPhone.SetActive(true);
#else
            controllerForSmartPhone.SetActive(false);
#endif
            _isPause = false;
            _isGameOver = false;
            _isGameClear = false;
            _time = 0.5f;
            _pushedEnemyNumber = 0;
            DataController.Instance.ResetInstance();
            pausePanel.SetActive(false);
            _gameOverPanel.SetActive(false);
            _gameClearPanel.SetActive(false);

            _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
            _leftCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).x;
            _rightCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).x;
            _topCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).y;
            _bottomCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).y;

            SetKillCount();

            pauseButton.onClick.AddListener(OnclickPause);
            okButton.onClick.AddListener(OnclickOkOnPause);

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
                _pushedEnemyNumber++;
            }
        }

        private void Update()
        {
            if (_isPause) return;

            DataController.Instance.AddTime();
            CheckGameClear();

            //背景
            for (int i = 0; i < _backGroundImages.Length; i++)
            {
                if (_backGroundImages[i].transform.position.y <= _bottomCameraWorldPos - 0.5f)
                {
                    int index = i - 1 >= 0 ? i - 1 : _backGroundImages.Length - 1;
//                    Debug.Log(index);
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
                if (!enemy.gameObject.activeSelf)
                {
                    if (_currentStage.ThisStageType == global::Stage.Stage.StageType.KillAll)
                    {
                        if (_currentStage.EnemyCount > _pushedEnemyNumber)
                        {
                            enemy.Initialize(ResetPos());
                            _pushedEnemyNumber++;
//                            Debug.Log(_pushedEnemyNumber);
                        }
                    }
                    else enemy.Initialize(ResetPos());
                }
            }

            //リザルト画面へ遷移
            if (_time <= 0) RoomController.Instance.GoToRoom(RoomController.Room.Result);
            if (_isGameClear) _time -= Time.deltaTime;
            else if (_isGameOver) _time -= Time.deltaTime;
        }

        /// <summary>
        /// ランダムでエネミーの発生するポイントを返却します
        /// </summary>
        /// <returns></returns>
        private Vector3 ResetPos()
        {
            return new Vector3(Random.Range(_leftCameraWorldPos, _rightCameraWorldPos),
                Random.Range(_topCameraWorldPos, _topCameraWorldPos + 5f));
        }

        /// <summary>
        /// 指定したHpを表示します
        /// </summary>
        /// <param name="hp"></param>
        public void SetHitPoint(int hp)
        {
            _hpText.text = $" Hp:{hp}";
        }

        /// <summary>
        /// 倒した数を加算します
        /// </summary>
        public void AddKillCount()
        {
            DataController.Instance.AddKillCount();
            SetKillCount();
        }

        /// <summary>
        /// 倒した数を表示します
        /// </summary>
        private void SetKillCount()
        {
            switch (_currentStage.ThisStageType)
            {
                case global::Stage.Stage.StageType.KillAll:
                case global::Stage.Stage.StageType.KillCount:
                    killCountText.text = $"{DataController.Instance.KillCount:0000} / {_currentStage.EnemyCount:0000}";
                    break;
                case global::Stage.Stage.StageType.TimeKill:
                    killCountText.text = $"{DataController.Instance.KillCount:0000}";
                    break;
            }
        }

        /// <summary>
        /// ゲームクリアしたかどうかをチェック
        /// </summary>
        private void CheckGameClear()
        {
            switch (_currentStage.ThisStageType)
            {
                case global::Stage.Stage.StageType.KillAll:
                case global::Stage.Stage.StageType.KillCount:
                    if (_currentStage.EnemyCount <= DataController.Instance.KillCount) GameClear();
                    break;
                case global::Stage.Stage.StageType.TimeKill:
                    break;
            }
        }

        /// <summary>
        /// ポーズボタンが押された際
        /// </summary>
        private void OnclickPause()
        {
            pauseButton.interactable = false;
            _isPause = !_isPause;
            pausePanel.SetActive(_isPause);
            Time.timeScale = _isPause ? 0f : 1f;
            pauseButton.interactable = true;
        }

        /// <summary>
        /// ポーズ画面のOKボタンが押された際
        /// </summary>
        private void OnclickOkOnPause()
        {
            RoomController.Instance.GoToRoom(RoomController.Room.Menu);
            Time.timeScale = 1f;
        }

        public void GameOver()
        {
            _isGameOver = true;
            _gameOverPanel.SetActive(true);
        }

        public void GameClear()
        {
            _isGameClear = true;
            DataController.Instance.CalcScore(player.Hp);
            _gameClearPanel.SetActive(true);
        }

        public float LeftCameraWorldPos => _leftCameraWorldPos;

        public float RightCameraWorldPos => _rightCameraWorldPos;

        public float TopCameraWorldPos => _topCameraWorldPos;

        public float BottomCameraWorldPos => _bottomCameraWorldPos;
    }
}