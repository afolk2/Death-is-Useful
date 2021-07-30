using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private float t = 0;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(t < .1)
        t += Time.deltaTime;
        rb.velocity = transform.up * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (t > .1)
            Destroy(gameObject);
    }
}
