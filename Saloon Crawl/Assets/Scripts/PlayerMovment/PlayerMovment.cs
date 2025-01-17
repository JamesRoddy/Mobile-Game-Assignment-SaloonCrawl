using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{


    private BoxCollider2D playerBoxCollider;
    private Rigidbody2D playerRigidBody;


    private float playerSpeed = 3.0f;

    private bool shouldJump = false;
    [SerializeField] LayerMask groundLayer;
    bool grounded = false;
    float jumpVelocity = 5.0f;
    private Animator CowboyAnim;

    //Shooting variables
    public bool shouldShoot = false;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public bool invoked = false;
    public float fBulletAngle;
    GameObject bullet;
    Bullet bull;

    // Start is called before the first frame update
    void Start()
    {
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        CowboyAnim = GetComponent<Animator>();
        bull = FindObjectOfType<Bullet>();
    }

    // Update is called once per frame
    void Update()
    {

        grounded = isGrounded();
        //addMomentum();
        jump();
        shoot();
        CowboyAnim.SetBool("OnGround", grounded);


    }
    private void addMomentum()
    {

        playerRigidBody.velocity = new Vector2(playerSpeed, playerRigidBody.velocity.y);


    }

    private void jump()
    {
        if (shouldJump && grounded)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpVelocity);
            shouldJump = false;
        }



    }

    private void shoot()
    {
        //Debug.Log("Bullet Destroyed: " + bullet.IsDestroyed());
        if (shouldShoot && !invoked)
        {
            invoked = true;
            shouldShoot = false;
            bulletPrefab.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, fBulletAngle));
            bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletPrefab.transform.rotation);
        }

        else if(bullet.IsDestroyed())
        {
            invoked = false;
        }
    }


    public Vector2 playerPosVec2
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
    }

    public bool ShouldJump
    {
        set { shouldJump = value; }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    public bool CanJump
    {
        get { return grounded; }
    }
    bool isGrounded()
    {

        RaycastHit2D hit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        
        return hit.collider != null;
    }


}

