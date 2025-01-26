using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaniateScorePopUp : MonoBehaviour
{
    [SerializeField] private GameObject prefab; 


    public void inistantiateScorePop(Vector3 position, Quaternion rot)
    {
        Instantiate(prefab, position, rot);


    }







}
