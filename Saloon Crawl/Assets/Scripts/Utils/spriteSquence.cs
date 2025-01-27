using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteSquence : MonoBehaviour
{
    // Start is
    // called before the first frame update

    [SerializeField] private List<GameObject> spriteSequence;
    private int count;
    [SerializeField] private float waitTime;
    bool cameraHasReachedPosition = false;
    [SerializeField] Camera cam;
    [SerializeField] float minDisance;
    float currentTimer = 0.0f;
    void Start()
    {
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(cam.transform.position, transform.position) < minDisance)
        {
            cameraHasReachedPosition = true;
        }



        if (cameraHasReachedPosition)
        {
            spriteSequenceUpdate();

        }





    }


    private void spriteSequenceUpdate()
    {
        if (count == spriteSequence.Count)

        {

            Debug.Log("deactivating sequnce ");
            this.gameObject.SetActive(false);
        }
        spriteSequence[count].SetActive(true);

        if (currentTimer < waitTime)
        {
            Debug.Log("sequence waiting to switch " + currentTimer);
            return;
        }

        spriteSequence[count].SetActive(false);
        count++;
        currentTimer = 0.0f;




    }
}