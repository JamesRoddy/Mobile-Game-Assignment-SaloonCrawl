using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBottle : MonoBehaviour
{
    Rigidbody2D bottle;
    float fBottleSpeed = 10f;
    Vector3 bottleVelocity;

    // Start is called before the first frame update
    void Start()
    {
        bottle = GetComponent<Rigidbody2D>();
        bottleVelocity = -this.transform.right * fBottleSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bottle.velocity = bottleVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
