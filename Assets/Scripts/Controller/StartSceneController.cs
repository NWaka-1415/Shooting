using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class StartSceneController : MonoBehaviour
    {
        private bool _isClicked;

        private void Start()
        {
            RoomController.Instance.GoToRoom(RoomController.Room.Start);
            _isClicked = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(_isClicked) return;
            if (Input.anyKeyDown)
            {
                //ゲームシーンに遷移
                SceneManager.LoadSceneAsync("GameScene");
                _isClicked = true;
            }
        }
    }
}