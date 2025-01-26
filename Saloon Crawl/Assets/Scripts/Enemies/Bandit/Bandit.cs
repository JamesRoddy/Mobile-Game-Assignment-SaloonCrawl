using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : EnemyDescriptorInfo
{
    // Start is called before the first frame update
    private Aiming banditAim;
    private DeathChecker checker;
    private LineRenderer lineRenderer; 
    private GameObject trailRenderer;
    private Transform gunPos;
    private TrailRenderer trail;
    private SpriteRenderer spriteRenderer;
    private GameManagerScript gameManagerScript; 
    public override void EnemyStart()
    {
        banditAim = transform.GetChild(0).GetComponent<Aiming>();
        checker = GetComponent<DeathChecker>();
        Debug.Log("bandit aim is null "+(banditAim == null));
        lineRenderer = GetComponent<LineRenderer>();
        trailRenderer = transform.Find("TrailPos").gameObject;
        gunPos = transform.Find("ArmPivot").Find("Arm").GetChild(0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        banditAim.AimingStart();
        gameManagerScript = FindFirstObjectByType<GameManagerScript>();


    }





    public override void EnemyEnable()
    {


      /*  lineRenderer = GetComponent<LineRenderer>();
        trailRenderer = transform.Find("TrailPos").gameObject;
        gunPos = transform.Find("ArmPivot").Find("Arm").GetChild(0);
        spriteRenderer = GetComponent<SpriteRenderer>();*/
    
        spriteRenderer.flipX = false;
        lineRenderer.enabled = true;
        
        lineRenderer.SetPosition(0, gunPos.position);
        lineRenderer.SetPosition(1, gunPos.position); 
        trailRenderer.transform.position = gunPos.position;
        trailRenderer.GetComponent<TrailRenderer>().enabled = false;

    }

    // Update is called once per frame
    public override void EnemyUpdate()
    {
        banditAim.AimingUpdate();

        if(checker.isAlive == false)
        {
            banditAim.aimingReset();
            InstantiatePopUp();
            controller.CurrentScore += ScoreIncrement;
            controller.conactToScore(Convert.ToString(ScoreIncrement));
            gameManagerScript.enemiesKilledThisRun += 1;
            this.gameObject.SetActive(false);
        }
    }
}
