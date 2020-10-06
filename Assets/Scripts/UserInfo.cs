using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    //Static reference
    public static UserInfo Instance {get; private set;}
    //Data to persist
    public string heightPx;
    public string widthPx;
    public string pxRatio;
    public string browserVersion;
    public string tokenId;
    public string handedness;
    public DateTime consentTime;
    public string GameMode;
    public float mouseSensitivity;
    public bool consent;
    public bool fullscreen;
    public bool locked;
    public int n_pauses;
    public List<string> trials_paused;
    private void Awake()
    {   
        //check if info is null
        if (Instance == null)
        {
            //This instance becomes the single instance available
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
            //Otherwise check if the control instance is not this one
        else
        {
            //In case there is a different instance destroy this one.
            Destroy(gameObject);
        }
    }
}
