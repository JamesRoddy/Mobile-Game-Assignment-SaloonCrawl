using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit : EnemyDescriptorInfo
{
    // Start is called before the first frame update
    private Aiming banditAim;
    public override void EnemyStart()
    {
        banditAim = transform.GetChild(0).GetComponent<Aiming>();
    }

    // Update is called once per frame
    public override void EnemyUpdate()
    {
        banditAim.AimingUpdate();
    }
}
