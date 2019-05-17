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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //左矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(
                    Mathf.Clamp(pos.x - _speed, _gameSceneManager.LeftCameraWorldPos,
                        _gameSceneManager.RightCameraWorldPos), pos.y);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            //右矢印キーが押されたら
            Vector2 pos = transform.position;
            transform.position =
                new Vector3(
                    Mathf.Clamp(pos.x - _speed, _gameSceneManager.LeftCameraWorldPos,
                        _gameSceneManager.RightCameraWorldPos), pos.y);
        }
    }
}