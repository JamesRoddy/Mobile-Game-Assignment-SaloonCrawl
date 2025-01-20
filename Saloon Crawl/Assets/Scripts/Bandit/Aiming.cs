using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPosition;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
    }
    private void LookAtPlayer()
    {
        Vector3 rotateDirection = transform.position - player.transform.position; // find out the distance between player and enemy 
        rotateDirection.Normalize(); // normalise that distance
        float angle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg; // use atan2 to figure out how much to rotate and then change it into degrees 
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // rotate the Z axis only
    }
}
