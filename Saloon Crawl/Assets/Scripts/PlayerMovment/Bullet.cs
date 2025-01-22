using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D bullet;
    float fBulletSpeed = 30f;
    public Vector2 Dir;
    TouchControls control;
    playerController player;
    private float fCollisionRadius = 0.1f;

    public LayerMask Ground;
    public LayerMask Enemy;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<TouchControls>();
        player = FindObjectOfType<playerController>();
        bullet = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Destroy(this.gameObject, 0.5f);

        Dir = control.getTouchPos() - (new Vector2(player.bulletSpawnPoint.transform.position.x, player.bulletSpawnPoint.transform.position.y));
        Debug.Log("Dir" + Dir);
        Dir.Normalize();
        Debug.Log("Normalised Dir" + Dir);
        bullet.velocity = Dir * fBulletSpeed;
        tr.emitting = true;

        if(Physics2D.OverlapCircle(transform.position, fCollisionRadius, Ground))
        {
            Destroy(this.gameObject);
        }

        if (Physics2D.OverlapCircle(transform.position, fCollisionRadius, Enemy))
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
