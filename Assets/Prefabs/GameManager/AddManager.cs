using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AddManager : MonoBehaviour
{

    string playStoreID = "3348239";
    string appleStoreID = "3348238";
    public bool testMode = false;
    public StorageHandler sh;

    void Start () {
        sh = GetComponent<StorageHandler>();
        Advertisement.Initialize(playStoreID, testMode);
        // StartCoroutine (ShowBannerWhenReady ()); Banner adds not working in europe yet apparently
        if(sh.GetPlayCount() > 1 && sh.GetPlayCount() % Parameters.livesForAdd == 0)
        {
            StartCoroutine (ShowFullScreenVideoWhenReady()); 
        }
        
    }

    IEnumerator  ShowBannerWhenReady()
    {
        while (!Advertisement.IsReady ("ContinousBanner")) 
        {
            Debug.Log("ADS: Still not initialised");
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Show("ContinousBanner");
        Debug.Log("ADS: YAY! Initialised");

    }

    IEnumerator  ShowFullScreenVideoWhenReady()
    {
        while (!Advertisement.IsReady ("video")) 
        {
            Debug.Log("ADS: Still not initialised");
            yield return new WaitForSeconds (0.5f);
        }
        Advertisement.Show("video");
        Debug.Log("ADS: YAY! Initialised");
    }

    public void ShowFullScreenVideo()
    {
        if(Advertisement.IsReady("video"))
        {
            Advertisement.Show("video");
        }
    }
}
