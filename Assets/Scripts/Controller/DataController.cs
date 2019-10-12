using System.Collections.Generic;
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

        private List<string[]> _stageData;

        public List<string[]> StageData => _stageData;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            
            Initialize();
        }

        public void Initialize()
        {
            _killCount = 0;
            _selectStage = 0;
            _score = 0;
            _stageData = CSVController.LoadCSV("stageData");
        }

        public void ResetInstance()
        {
            _killCount = 0;
            _score = 0;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
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