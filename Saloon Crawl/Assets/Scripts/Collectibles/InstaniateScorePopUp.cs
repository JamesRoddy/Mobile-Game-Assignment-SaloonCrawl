using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaniateScorePopUp : MonoBehaviour
{
    [SerializeField] private GameObject prefab; 


    public void inistantiateScorePop(Vector3 position, Quaternion rot)
    {
        Debug.Log("SCORE POP UP instantiating score pop up ");
        Instantiate(prefab, position, rot);


    }







}
