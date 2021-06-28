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
    public string GameMode;
    public string UserTable;
    public string TrialTable;
    public string LeaderboardTable;
    public string TokenTable;
    public bool aimQuestions;
    public bool zeroAimQuestions;
    public int n_aimQuestions;
    public int fanfareCount;
    public string age;
    public string gender;
    public string heightPx;
    public string widthPx;
    public string pxRatio;
    public string browserVersion;
    public string username;
    public string tokenId;
    public string handedness;
    public string pointer;
    public DateTime consentTime;
    public float mouseSensitivity;
    public bool consent;
    public bool fullscreen;
    public bool locked;
    public string gameProgress;
    public int trialProgress;
    public int SSD;
    public int n_pauses;
    public int score;
    public List<string> trials_paused;
    public string ZeroOrder;
    public float ZeroCertainTargetPos;
    public float ZeroCertainTargetHitX;
    public float ZeroCertainTargetHitY;
    public float ZeroCertainBatHitX;
    public float ZeroCertainBatHitY;
    public float ZeroUncertainTargetPos;
    public float ZeroUncertainTargetHitX;
    public float ZeroUncertainTargetHitY;
    public float ZeroUncertainBatHitX;
    public float ZeroUncertainBatHitY;

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
