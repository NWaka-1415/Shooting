using System;
using System.Collections.Generic;
using Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class MenuSceneController : MonoBehaviour
    {
        private static MenuSceneController _instance = null;

        public static MenuSceneController Instance => _instance;

        private List<StageButton> _stageButtons;

        private int _stageNumber;

        [SerializeField] private GameObject stageButtonPrefab = null; //そのうちresourceロードにするかも
        [SerializeField] private GameObject stageSelectZone = null;
        [SerializeField] private Button okButton = null;
        [SerializeField] float[] xAxises = new float[2];
        [SerializeField] private float yAxisStartPos = 0;
        [SerializeField] private float yAxisDuration = 10;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            _stageNumber = DataController.Instance.StageData.Count;
            Debug.Log($"{_stageNumber}個のステージを作成します。");
            SetStageButtons();
            okButton.onClick.AddListener(OnclickOk);
        }

        private void SetStageButtons()
        {
            _stageButtons = new List<StageButton>(_stageNumber);
            for (int i = 0; i < _stageNumber; i++)
            {
                StageButton stageButtonScript = Instantiate(stageButtonPrefab).GetComponent<StageButton>();
                stageButtonScript.transform.parent = stageSelectZone.transform;
                stageButtonScript.Initialize(i, new Vector2(xAxises[i % 2], yAxisStartPos + yAxisDuration * i),
                    i == DataController.Instance.SelectStage);
                _stageButtons.Add(stageButtonScript);
            }
        }

        public void SelectStageButton(int stage)
        {
            DataController.Instance.SetSelectStage(stage);
            foreach (StageButton stageButton in _stageButtons)
            {
                if (stageButton.StageNumber != stage) stageButton.Select(false);
            }
        }

        private void OnclickOk()
        {
            RoomController.Instance.GoToRoom(RoomController.Room.Gaming);
        }
    }
}