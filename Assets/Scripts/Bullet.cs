using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody2D = null;
    private float _time;

    void Update()
    {
        if (_time <= 0) Destroy(gameObject);
        _time -= Time.deltaTime;
    }

    public void Initialize(Vector2 pos)
    {
        transform.position = pos;
        myRigidbody2D.velocity = new Vector2(0f, 10f);
        _time = 1f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.y >= GameSceneController.Instance.TopCameraWorldPos) return;
        if (other.CompareTag("Enemy"))
        {
            GameSceneController.Instance.AddKillCount();
            //敵を消し
            other.gameObject.SetActive(false);
            //自分も消す
            gameObject.SetActive(false);
        }
    }
}