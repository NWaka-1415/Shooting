using System;
using UnityEngine;

namespace Controller
{
    public class CursorController : MonoBehaviour
    {
        private static CursorController _instance = null;

        public static CursorController Instance => _instance;

        private Vector2 _prevPos;

        private bool _visibleMode;

        private float _time;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            _prevPos = Input.mousePosition;
            _visibleMode = RoomController.Instance.CurrentRoom != RoomController.Room.Gaming;
        }

        private void Update()
        {
            if (_visibleMode) return;
            Vector2 mousePos = Input.mousePosition;
            //動いていれば
            if (Mathf.Abs(mousePos.x - _prevPos.x) > 0f ||
                Mathf.Abs(mousePos.y - _prevPos.y) > 0f)
            {
                _time = 1f;
                Cursor.visible = true;
            }
            else _time -= Time.deltaTime;

            if (_time <= 0f) Cursor.visible = false;
            _prevPos = mousePos;
        }

        public void SetCursorVisible(RoomController.Room room)
        {
            switch (room)
            {
                case RoomController.Room.Gaming:
                    _prevPos = Input.mousePosition;
                    _visibleMode = false;
                    break;
                default:
                    Cursor.visible = true;
                    _prevPos = Input.mousePosition;
                    _visibleMode = true;
                    break;
            }
        }
    }
}