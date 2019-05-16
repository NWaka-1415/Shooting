using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private Camera _camera;
    private float _leftCameraWorldPos;
    private float _rightCameraWorldPos;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _leftCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).x;
        _rightCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).x;
    }

    public float LeftCameraWorldPos => _leftCameraWorldPos;

    public float RightCameraWorldPos => _rightCameraWorldPos;
}