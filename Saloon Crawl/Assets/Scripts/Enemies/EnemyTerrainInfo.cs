using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTerrainInfo : MonoBehaviour
{

    [SerializeField] protected float spawnInterval;









    public float SpawnInterval
    {
        get { return spawnInterval; }
    }






}
