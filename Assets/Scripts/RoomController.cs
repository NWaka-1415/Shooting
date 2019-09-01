using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void GoToRoom(Room room)
    {
        _currentRoom = room;
    }
}