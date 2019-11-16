using System.Collections.Generic;
using UnityEngine;
using Stage = Stage.Stage;

namespace Controller
{
    public class DataController : MonoBehaviour
    {
        private static DataController _instance = null;

        public static DataController Instance => _instance;

        private int _killCount;

        public int KillCount => _killCount;

        private int _selectStage;

        public int SelectStage => _selectStage;

        private float _time;

        public float Time => _time;

        private int _score;

        public int Score => _score;

        private List<string[]> _stageData;

        public List<string[]> StageData => _stageData;

        private List<global::Stage.Stage> _stages;

        public List<global::Stage.Stage> Stages => _stages;

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
            _time = 0f;
            _stageData = CSVController.LoadCSV("stageData", "csv/");
            _stages=new List<global::Stage.Stage>();
            for (int i = 0; i < _stageData.Count; i++)
            {
                _stages.Add(new global::Stage.Stage(i));
            }
            Debug.Log(_stageData);
        }

        public void ResetInstance()
        {
            _killCount = 0;
            _score = 0;
            _time = 0f;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void SetSelectStage(int stage)
        {
            _selectStage = stage;
        }

        public void CalcScore(int hp)
        {
            _score = (int) (_killCount * hp - _time);
        }

        public void AddKillCount()
        {
            _killCount++;
        }

        public void AddTime()
        {
            _time += UnityEngine.Time.deltaTime;
        }
    }
}