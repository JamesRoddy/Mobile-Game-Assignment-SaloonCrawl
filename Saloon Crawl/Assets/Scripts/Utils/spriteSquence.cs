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
    PlayerCameraShift cameraShift;
   
    void Start()
    {
        count = 0;
        cameraShift = FindFirstObjectByType<PlayerCameraShift>();
       
        for(int i = 1;i<spriteSequence.Count;i++)
        {
            spriteSequence[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(" updating sprite sequence " + cameraHasReachedPosition);
        if(cameraShift.hasCentred )
        {
            Debug.Log("camera has reached position");
            cameraHasReachedPosition = true;
        }



        if (cameraHasReachedPosition)
        {
            transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,transform.position.z);    
            spriteSequenceUpdate();

        }





    }


    private void spriteSequenceUpdate()
    {
        if (count == spriteSequence.Count)

        {
            Debug.Log("sprite count hit ");
            Debug.Log("deactivating sequnce ");
            gameObject.SetActive(false);
            
        }
        else
        {
            spriteSequence[count].SetActive(true);

            if (currentTimer < waitTime)
            {
                Debug.Log("sequence waiting to switch " + currentTimer +" max switch time "+waitTime);
                currentTimer += Time.deltaTime;
                return;
            }

            spriteSequence[count].SetActive(false);
            count++;
            currentTimer = 0.0f;
        }
       




    }
}