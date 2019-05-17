using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private float _time;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = new Vector2(0f, 10f);
        _time = 1f;
    }

    void Update()
    {
        if (_time <= 0) Destroy(gameObject);
        _time -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //敵を消し
            Destroy(other.gameObject);
            //自分も消す
            Destroy(gameObject);
        }
    }
}