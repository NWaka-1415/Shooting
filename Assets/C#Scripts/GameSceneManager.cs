using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private Text _hpText;

    private Camera _camera;

    private float _leftCameraWorldPos;
    private float _rightCameraWorldPos;
    private float _topCameraWorldPos;
    private float _bottomCameraWorldPos;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _leftCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).x;
        _rightCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).x;
        _topCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(1, 1)).y;
        _bottomCameraWorldPos = _camera.ViewportToWorldPoint(new Vector3(0, 0)).y;
    }

    public void SetHitPoint(int hp)
    {
        _hpText.text = $" Hp:{hp}";
    }

    public float LeftCameraWorldPos => _leftCameraWorldPos;

    public float RightCameraWorldPos => _rightCameraWorldPos;

    public float TopCameraWorldPos => _topCameraWorldPos;

    public float BottomCameraWorldPos => _bottomCameraWorldPos;
}