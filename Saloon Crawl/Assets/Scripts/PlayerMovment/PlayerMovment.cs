using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour
{


    private BoxCollider2D playerBoxCollider;
    private Rigidbody2D playerRigidBody;
    private TerrainType currentTerrain;
    private int currentTerrainCycles;
    private float playerSpeed = 3.0f;
    private float minDistanceToEndOfCurrentTerrain = 400.0f;
    private bool shouldJump = false;
    [SerializeField] LayerMask groundLayer;
    bool grounded = false;
    float jumpVelocity = 5.0f;
    public Animator CowboyAnim;

    //Shooting variables
    public bool shouldShoot = false;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public bool invoked = false;
    public float fBulletAngle;
    GameObject bullet;
    Bullet bull;
    public GameObject arm;
    

    public bool shouldSlide = false;
    bool isSliding = false;
    float fSlidePowerY = -10f;

    //Jump Soundeffects
    public AudioClip bulletShot;
    public AudioClip jumpSound;
    public AudioClip slideSound;
    public AudioClip kickSound;
    public AudioClip runSound;

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
        addMomentum();
        jump();
        shoot();
        slide();
        CowboyAnim.SetBool("OnGround", grounded);

    }
    private void addMomentum()
    {

        playerRigidBody.velocity = new Vector2(playerSpeed, playerRigidBody.velocity.y);
        /*AudioSource.PlayClipAtPoint(runSound, transform.position);*/


    }

    private void jump()
    {
        if (shouldJump && grounded)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpVelocity);

            /*            AudioSource.PlayClipAtPoint(jumpSound, transform.position);*/

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
            arm.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, fBulletAngle));
            bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletPrefab.transform.rotation);

/*            AudioSource.PlayClipAtPoint(bulletShot, bulletSpawnPoint.position);*/

        }

        else if (bullet.IsDestroyed())
        {
            shouldShoot = false;
            invoked = false;
        }
    }


    public bool CurrentTerrainCyclesIsMultiple(int numberToTest) {

        if (numberToTest == 0)
        {
            return true;
        }

        return currentTerrainCycles % numberToTest == 0;
}
    private void slide()
    {
        if(isSliding)
        {
            return;
        }

        else if (shouldSlide)
        {
            StartCoroutine(AdjustCollider());
        }
        
    }

    IEnumerator AdjustCollider()
    {
        float fStoreX = playerBoxCollider.size.x;
        float fStoreY = playerBoxCollider.size.y;
        shouldSlide = false;
        isSliding = true;
        CowboyAnim.SetBool("IsSliding", true);

        if (!Grounded)
        {
            Debug.Log("Airborn");
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, fSlidePowerY);
            playerBoxCollider.size = new Vector2(fStoreY, fStoreX - 0.2f);
        }

        else
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, playerRigidBody.velocity.y);
            playerBoxCollider.size = new Vector2(fStoreY, fStoreX - 0.2f);
        }

        yield return new WaitForSeconds(0.5f);
        playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0f);
        playerBoxCollider.size = new Vector2(fStoreX, fStoreY);
        isSliding = false;

        CowboyAnim.SetBool("IsSliding", false);

        
        Debug.Log("shouldSlide3: " + shouldSlide);

    }

    public bool isCloseToEndOfCurrentterrain()
    {


        return Vector3.SqrMagnitude((currentTerrain.transform.position + currentTerrain.GetComponent<TerrainType>().getHalfScale) - transform.position) <= minDistanceToEndOfCurrentTerrain;

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

    public bool Grounded
    {
        get { return grounded; }
    }

    public TerrainType CurrentTerrain
    {
        set { currentTerrain = value; }
        get { return currentTerrain; }
    }

    public GameObject getCurrentTerrainGameObject
    {
        get { return currentTerrain.gameObject; }
    }

    public int getTerrainCycles
    {
        get { return currentTerrainCycles; }
        set { currentTerrainCycles = value; }
    }
    public TerrainClassifications CurrentTerrainClassification
    {

        get { return currentTerrain.GetComponent<TerrainType>().GetClassification; }
    } 

    bool isGrounded()
    {


        RaycastHit2D hit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);


        return hit.collider != null;
    }


}

