using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class ZombieMovement : EnemyDescriptorInfo
{

    private float speed = -3;
    private Rigidbody2D zombieRigidBody;
    private Camera playerCam;
    private DeathChecker zombieDeath;


    public override void EnemyStart()
    {
        zombieRigidBody = GetComponent<Rigidbody2D>();
        playerCam = Camera.main;
        zombieDeath = GetComponent<DeathChecker>();
       
    }

    public override void EnemyUpdate()
    { 
        Movement();
        if (isInNotCameraView())
        {  
            gameObject.SetActive(false);
        }
        isDead();
    }

    private void Movement()
    {
        zombieRigidBody.velocity = new Vector2(speed, zombieRigidBody.velocity.y);
    }
    private bool isInNotCameraView()
    {

        Vector3 camViewPortPos = playerCam.WorldToViewportPoint(new Vector3(transform.position.x, transform.position.y, transform.position.z));
        return camViewPortPos.x < 0.0f;
    }

    private void isDead()
    {   
        
        if(zombieDeath.isAlive == false)
        {
            InstantiatePopUp();
            this.gameObject.SetActive(false);
        }
    }
}
