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
    public override void EnemyStart()
    {
        banditAim = transform.GetChild(0).GetComponent<Aiming>();
        checker = GetComponent<DeathChecker>();
        Debug.Log("bandit aim is null "+(banditAim == null));
        banditAim.AimingStart();
    }

    public override void EnemyEnable()
    {
        
        Debug.Log("second call to getting bandit compoenent " + (banditAim == null));
       
       
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
