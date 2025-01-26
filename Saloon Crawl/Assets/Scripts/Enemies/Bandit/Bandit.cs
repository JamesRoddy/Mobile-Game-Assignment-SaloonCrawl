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
    public override void EnemyStart()
    {
        banditAim = transform.GetChild(0).GetComponent<Aiming>();
        checker = GetComponent<DeathChecker>();
        Debug.Log("bandit aim is null "+(banditAim == null));
        banditAim.AimingStart();
        lineRenderer = GetComponent<LineRenderer>();
        trailRenderer = transform.Find("TrailPos").gameObject;
        gunPos = transform.Find("Arm").GetChild(0); 
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void EnemyEnable()
    {
        
        Debug.Log("second call to getting bandit compoenent " + (banditAim == null));
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
            this.gameObject.SetActive(false);
        }
    }
}
