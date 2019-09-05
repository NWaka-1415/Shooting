using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller
{
    public class RoomController : MonoBehaviour
    {
        private static RoomController _instance = null;

        public static RoomController Instance => _instance;

        public enum Room
        {
            Start,
            Menu,
            Gaming,
            Result
        }

        private Dictionary<Room, string> _rooms;

        private Room _currentRoom;

        public Room CurrentRoom => _currentRoom;

        private void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SetRooms();
            foreach (KeyValuePair<Room,string> room in _rooms)
            {
                if (room.Value.Equals(SceneManager.GetActiveScene().name))
                {
                    _currentRoom = room.Key;
                    Debug.Log(_currentRoom);
                    break;
                }
            }
        }

        private void SetRooms()
        {
            _rooms = new Dictionary<Room, string>();
            _rooms.Add(Room.Start, "StartScene");
            _rooms.Add(Room.Menu, "");
            _rooms.Add(Room.Gaming, "GameScene");
            _rooms.Add(Room.Result, "Result");
        }

        public void GoToRoom(Room room)
        {
            _currentRoom = room;
            SceneManager.LoadSceneAsync(_rooms[_currentRoom]);
        }
    }
}