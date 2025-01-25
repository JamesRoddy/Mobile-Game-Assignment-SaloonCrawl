using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : Interactable
{

    Rigidbody2D kickable;
    float fTravelSpeedRight = 10f;
    float fTravelSpeedUp = 5f;
    TouchControls control;
    playerController player;
    Vector3 kickableVelocity;
    public Transform kickPosition;
    public bool isKicking = false;
    bool bKicked = false;
    public GameObject brokenStoolTop;
    public GameObject brokenStoolBottom;
    int iNum = 0;
    private Camera playerCam;
    private BoxCollider2D collision;
    //private DeathChecker deathChecker;

    public LayerMask Ground;
    public LayerMask Enemy;

    public override void interactableStart()
    {
        control = FindObjectOfType<TouchControls>();
        player = FindObjectOfType<playerController>();
        kickable = GetComponent<Rigidbody2D>();
        playerCam = Camera.main;
        collision = GetComponent<BoxCollider2D>();
        kickableVelocity = (this.transform.up * fTravelSpeedUp) + (this.transform.right * fTravelSpeedRight);
    }

    public override void interactableUpdate()
    {
        if (player.transform.position.x >= kickPosition.transform.position.x && player.transform.position.x < kickable.transform.position.x && control.bSwipeRight)
        {
            transform.gameObject.layer = 13;
            StartCoroutine(playAnim());
            kickable.velocity = kickableVelocity;
            control.bSwipeRight = false;
            bKicked = true;
            //deathChecker = player.GetComponent<DeathChecker>();
        }

        else if (isNotInCameraView())
        {
            this.gameObject.SetActive(false);
        }

        if (kickable.transform.position.y > 0.0f)
        {
            kickable.transform.Rotate(0f, 0f, Time.deltaTime * 1000f, Space.World);
        }

    }

    void replaceSprites()
    {
        if (iNum <= 1)
        { 
            var stoolBottom = Instantiate(brokenStoolBottom, kickable.transform.position, kickable.transform.rotation);
            var stoolTop = Instantiate(brokenStoolTop, kickable.transform.position, kickable.transform.rotation);
            Destroy(stoolTop, 0.5f);
            Destroy(stoolBottom, 0.5f);
        }

        else
        {
            iNum = 0;
        }
       
    }

    IEnumerator playAnim()
    {
/*        DeathChecker playerDeath = player.GetComponent<DeathChecker>();
        if (playerDeath.IsAlive == true)
        {
            player.CowboyAnim.SetBool("Kick", true);
            t += Time.deltaTime;
            if (t < 0.5)
            {
                player.CowboyAnim.SetBool("Kick", false);
                yield return null;
            }
        }
        else
        {
            player.CowboyAnim.SetBool("Kick", false );
                            yield return null;
        }*/
        player.CowboyAnim.SetBool("Kick", true);
        yield return new WaitForSeconds(0.5f);
        player.CowboyAnim.SetBool("Kick", false);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(bKicked)
        {
            replaceSprites();  
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground"))
            {
                player.CowboyAnim.SetBool("Kick", false);
                iNum += 1;
                
                this.gameObject.SetActive(false);
                ResetVariables();
            }

        }

        else
        {
            bKicked = false;
        }

        
    }

    private bool isNotInCameraView()
    {
        Vector3 camViewPortPos = playerCam.WorldToViewportPoint(new Vector3(transform.position.x + collision.bounds.size.x / 2.0f, transform.position.y, transform.position.z));
        return camViewPortPos.x < 0.0f;
    }


    void ResetVariables()
    {
        if(!isActiveAndEnabled)
        {
            kickable.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            collision.gameObject.SetActive(true);
            fTravelSpeedRight = 10f;
            fTravelSpeedUp = 5f;
            isKicking = false;
            bKicked = false;
            iNum = 0;
            this.gameObject.SetActive(false);
            transform.gameObject.layer = 9;
        }
    }


    public bool Kicked
    {
        set { bKicked = value; } 
    }
}
