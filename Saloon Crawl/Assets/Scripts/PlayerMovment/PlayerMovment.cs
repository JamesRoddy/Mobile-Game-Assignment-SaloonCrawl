using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class playerController : MonoBehaviour
{


    private BoxCollider2D playerBoxCollider;
    private Rigidbody2D playerRigidBody;
    private TerrainType currentTerrain;
    private TouchControls touchControls;
    private Camera currentCam;
    private bool shouldIncrementScore = true;
    private int currentTerrainCycles;
    private float playerSpeed = 4.0f;
    private float minDistanceToEndOfCurrentTerrain = 144.0f;
    private bool shouldJump = false;
    [SerializeField] LayerMask groundLayer;
    bool grounded = false;
    float jumpVelocity = 7.0f;
    public Animator CowboyAnim;
    private float scoreIncrement = 0.0f;
    private float scoreIncrementMax = 0.1f;
    //Shooting variables
    public bool shouldShoot = false;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public bool invoked = false;
    public float fBulletAngle;
    GameObject bullet;
    Bullet bull;
    public GameObject arm;
    private int score;
    private bool isViewingNextTerrain = false;
    private int currentCoinCount = 0;
    private CamMovement followCam;

    private DeathChecker deathChecker;
    private Vector3 nextTerrainPos;
    public bool shouldSlide = false;
    bool isSliding = false;
    float fSlidePowerY = -10f;
    private PlayerCameraShift PlayerCameraShift;
    private string scoreString = "";
    //Jump Soundeffects
    public AudioSource runSound;
    public AudioSource bulletShot;
    public AudioSource jumpSound;
    public AudioSource slideSound;
    public AudioSource kickSound;

    // Start is called before the first frame update
    void Start()
    {
        playerBoxCollider = GetComponent<BoxCollider2D>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        currentCam = FindFirstObjectByType<Camera>();
        PlayerCameraShift = currentCam.GetComponent<PlayerCameraShift>();
        CowboyAnim = GetComponent<Animator>();
        bull = FindObjectOfType<Bullet>();
        followCam = FindFirstObjectByType<CamMovement>();
        touchControls = FindFirstObjectByType<TouchControls>();
        runSound.Play();


       deathChecker = GetComponent<DeathChecker>();

    }

    // Update is called once per frame
    void Update()
    {

/*            shouldViewNextTerrain();
*/            grounded = isGrounded();
            addMomentum();
            jump();
            shoot();
            slide();
            CowboyAnim.SetBool("OnGround", grounded);
            CowboyAnim.SetBool("IsAlive", deathChecker.IsAlive);
            IsDead();
            
      

    }
    private void shouldViewNextTerrain()
    {
        if(currentTerrain.NextTerrainType != null && (touchControls.accelerationHasHitPositve() || touchControls.SwipeLeft) && !IsViewingNextTerrain &&! PlayerCameraShift.IsShiftingBack)
        {
            Debug.Log("CAM SHIFTING conditions hit to view next terrain current");
            IsViewingNextTerrain = true;
            followCam.FollowPlayer = false; 
            nextTerrainPos = new Vector3(currentTerrain.NextTerrainType.transform.position.x, transform.position.y, transform.position.z);
            Debug.Log("CAM SHIFTING next terrain pos " + nextTerrainPos);
        }

        if (!IsViewingNextTerrain )
        {

            playerRigidBody.simulated = true;

            return;
        }
        
        playerRigidBody.simulated = false;
        shouldIncrementScore = false;


    }
  
    private void addMomentum()
    {

/*        playerRigidBody.velocity = new Vector2(playerSpeed, playerRigidBody.velocity.y);
*/    }

    
    public bool isOnRightSideByCertainFractionOfScale(float divider)
    {

        return transform.position.x >= (currentTerrain.transform.position.x + currentTerrain.transform.localScale.x / divider);


    }

    private void jump()
    {
        if (shouldJump && grounded)
        {

            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpVelocity);

            shouldJump = false;
            jumpSound.Play();  
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

            bulletShot.Play();

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
            slideSound.Play();
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
    public bool isInCurrentTerrian()
    {

/*        Debug.Log("is on current terrain " + currentTerrain.GetClassification + " is in bounds " + (transform.position.x <= currentTerrain.SpawnRight.x && transform.position.x >= currentTerrain.SpawnLeft.x));
*/        return (transform.position.x <= currentTerrain.SpawnRight.x && transform.position.x >= currentTerrain.SpawnLeft.x);
    }
    public bool isCloseToEndOfCurrentterrain()
    {
/*        Debug.Log("current terrain was null " + (currentTerrain == null));
*/
        return Vector3.SqrMagnitude((currentTerrain.transform.position + currentTerrain.getHalfScale) - transform.position) <= minDistanceToEndOfCurrentTerrain;

    }

    private void ScoreConstantIncrement()
    {

        if (shouldIncrementScore)
        {
            scoreIncrement += Time.deltaTime;

            if (scoreIncrement >= scoreIncrementMax)
            {
                CurrentScore++;
                scoreString = Convert.ToString(CurrentScore);
                scoreIncrement = 0.0f;
            }

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
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            collision.collider.enabled = false;
            Rigidbody2D enemyBody = collision.collider.GetComponent<Rigidbody2D>();
            enemyBody.simulated = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }


    public int CurrentCoinCount { 
        get { return currentCoinCount; }
        set { currentCoinCount = value; }
    }
    public Vector3 NextTerrainCamPos
    {
        get { return new Vector3(nextTerrainPos.x,nextTerrainPos.y,currentCam.transform.position.z); }
    }
    public bool IsViewingNextTerrain
    {
        set {  isViewingNextTerrain = value; }
        get { return isViewingNextTerrain; }
    }
    public string currentScoreString
    {
        get { return scoreString; }
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

    public  int CurrentScore
    {
        get { return score; }
        set { score = value; }
    } 
    public void addToPoints(int value)
    {
        score += value;
    }
    bool isGrounded()
    {


        RaycastHit2D hit = Physics2D.BoxCast(playerBoxCollider.bounds.center, playerBoxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);


        return hit.collider != null;
    }

    private void IsDead()
    {
        if ( deathChecker.IsAlive == false)
        {
            CowboyAnim.SetBool("IsAlive", false);
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

            if (grounded == true)
            {
               if(CowboyAnim.GetBool("Kick") == false)
                {
                    Time.timeScale = 0;
                }
            }
            return;
        }

        ScoreConstantIncrement();
    }


    

}

