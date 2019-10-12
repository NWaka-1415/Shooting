using System;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class ResultSceneController : MonoBehaviour
    {
        private static ResultSceneController _instance = null;

        public static ResultSceneController Instance => _instance;

        [SerializeField] private Button okButton = null;
        [SerializeField] private Text scoreText = null;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            okButton.onClick.AddListener(OnclickOk);
            SetScoreText();
        }

        private void SetScoreText()
        {
            scoreText.text = $"Score\n{DataController.Instance.Score}";
        }

        private void OnclickOk()
        {
            RoomController.Instance.GoToRoom(RoomController.Room.Menu);
        }
    }
}