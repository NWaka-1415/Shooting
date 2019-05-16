using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed;
    private GameSceneManager _gameSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        _speed = 0.1f;
        _gameSceneManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
        if (_gameSceneManager == null) Debug.LogError("Error!");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x <= _gameSceneManager.LeftCameraWorldPos)
        {
            //左端
            transform.position = new Vector3(_gameSceneManager.LeftCameraWorldPos, transform.position.y);
        }

        if (transform.position.x >= _gameSceneManager.RightCameraWorldPos)
        {
            //右端
            transform.position = new Vector3(_gameSceneManager.RightCameraWorldPos, transform.position.y);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //左矢印キーが押されたら
            transform.Translate(-_speed, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //右矢印キーが押されたら
            transform.Translate(_speed, 0, 0);
        }
    }
}