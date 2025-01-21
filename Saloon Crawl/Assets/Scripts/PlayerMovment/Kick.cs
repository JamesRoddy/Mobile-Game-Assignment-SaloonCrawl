using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
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
    private float fCollisionRadius = 0.1f;
    public GameObject brokenStoolTop;
    public GameObject brokenStoolBottom;

    public LayerMask Ground;
    public LayerMask Enemy;

    // Start is called before the first frame update
    void Start()
    {
        control = FindObjectOfType<TouchControls>();
        player = FindObjectOfType<playerController>();
        kickable = GetComponent<Rigidbody2D>();
        kickableVelocity = (this.transform.up * fTravelSpeedUp) + (this.transform.right * fTravelSpeedRight);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x >= kickPosition.transform.position.x && player.transform.position.x < kickable.transform.position.x && control.bSwipeRight)
        {
            Debug.Log("Kicked");
            kickable.velocity = kickableVelocity;
            control.bSwipeRight = false;
            bKicked = true;
        }

        if (kickable.transform.position.y > 0.0f)
        {
            Debug.Log("Rotated");
            kickable.transform.Rotate(0f, 0f, Time.deltaTime * 1000f, Space.World);
        }

        if (bKicked)
        {
            if (Physics2D.OverlapCircle(transform.position, fCollisionRadius, Ground))
            {
                Destroy(this.gameObject);
            }

            if(Physics2D.OverlapCircle(transform.position, fCollisionRadius, Enemy))
            {
                Destroy(this.gameObject);
            }
        }

        else
        {
            //Debug.Log("Reset Booleans");
            bKicked = false; 
        }

    }

    void replaceSprites()
    {
        var stoolBottom = Instantiate(brokenStoolBottom, kickable.transform.position, kickable.transform.rotation);
        var stoolTop = Instantiate(brokenStoolTop, kickable.transform.position, kickable.transform.rotation);
        Destroy(this.gameObject);
        Destroy(stoolTop, 2f);
        Destroy(stoolBottom, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(bKicked)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                replaceSprites();
            }

            if (collision.gameObject.CompareTag("Enemy"))
            {
                replaceSprites();
            }
        }
    }
}
