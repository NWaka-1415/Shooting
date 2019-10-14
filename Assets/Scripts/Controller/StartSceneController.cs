using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField] private Text pressText = null;

        private bool _isClicked;

        private void Start()
        {
#if UNITY_ANDROID
            pressText.text = "Touch any position";
#else
            pressText.text = "Press any key";
#endif

            _isClicked = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isClicked) return;
            if(Input.GetKey(KeyCode.Escape)) Application.Quit();
            else if (Input.anyKeyDown)
            {
                //ゲームシーンに遷移
                RoomController.Instance.GoToRoom(RoomController.Room.Menu);
                _isClicked = true;
            }
        }
    }
}