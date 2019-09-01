using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    private bool _isClicked;

    private void Start()
    {
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