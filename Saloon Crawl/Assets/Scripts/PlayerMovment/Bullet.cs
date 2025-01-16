using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bullet;
    float fBulletSpeed = 20f;
    Vector3 bulletVelocity;

    // Start is called before the first frame update
    void Start()
    {
        bullet = GetComponent<Rigidbody2D>();
        bulletVelocity = this.transform.right * fBulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bullet.velocity = bulletVelocity;
        Destroy(this.gameObject, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
