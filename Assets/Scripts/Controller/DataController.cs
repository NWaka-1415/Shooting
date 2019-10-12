using UnityEngine;

namespace Controller
{
    public class DataController : MonoBehaviour
    {
        private static DataController _instance = null;

        public static DataController Instance => _instance;

        private float _killCount;

        public float KillCount => _killCount;

        private int _selectStage;

        public int SelectStage => _selectStage;

        private int _score;

        public int Score => _score;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        public void Initialize()
        {
            _killCount = 0;
            _selectStage = 0;
            _score = 0;
        }

        public void ResetInstance()
        {
            _killCount = 0;
            _score = 0;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        public void SetSelectStage(int stage)
        {
            _selectStage = stage;
        }

        public void AddKillCount()
        {
            _killCount++;
        }
    }
}