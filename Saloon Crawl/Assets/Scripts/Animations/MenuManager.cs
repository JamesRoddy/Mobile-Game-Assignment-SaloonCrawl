using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        Debug.Log("Changing Scene");
        SceneManager.LoadScene(sceneName);
    }

    public void WaitChangeScene(string sceneName)
    {
        StartCoroutine(waitForAnim(sceneName));
    }

    IEnumerator waitForAnim(string sceneName)
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
}
