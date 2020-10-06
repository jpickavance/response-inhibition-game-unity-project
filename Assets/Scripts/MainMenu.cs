using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
        //js headers  
    [DllImport("__Internal")]
    private static extern string ReadData (string tableName,
                                           string token);
    
    [DllImport("__Internal")]
    private static extern string UpdateToken (string tableName,
                                              string token);
                                              
    [DllImport("__Internal")]
    private static extern string getScreenWidth();
    [DllImport("__Internal")]
    private static extern string getScreenHeight();
    [DllImport("__Internal")]
    private static extern string getPixelRatio();
    [DllImport("__Internal")]
    private static extern string getBrowserVersion();
    [DllImport("__Internal")]
    private static extern string fullscreenMenuListener();

    public InputField TokenField;
    public InputField HandednessField;

    public Text errMessage;
    public GameObject consentForm;
    public DateTime consentTime;
    public DateTime startTime;

    [System.Serializable]
    public class tokenClass
    {
        public string tokenId;
        public bool available;
    }

    public void Awake()
    {
        UserInfo.Instance.consent = false;
        TokenField.Select();
    }

    public void Start()
    {
        if(UserInfo.Instance.GameMode != "debug")
        {
            UserInfo.Instance.widthPx = getScreenWidth();
            UserInfo.Instance.heightPx = getScreenHeight();
            UserInfo.Instance.pxRatio = getPixelRatio();
            UserInfo.Instance.browserVersion = getBrowserVersion();
            fullscreenMenuListener();
        }
    }

    public void PlayGame() 
    { 
        if(UserInfo.Instance.consent == false && UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan")
        {
            errMessage.text = "You must provide your consent before participating in the experiment";
        }
        else
        {
            if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan")
            {
                ReadData("tokenTable", UserInfo.Instance.tokenId.ToString());
            }
            else
            {
                SceneManager.LoadScene("SettingsMenu");
            }
        }
 
    }

    public void ViewConsent()
    {
        Screen.fullScreen = true;
        consentForm.SetActive(true);
    }
    public void GiveConsent()
    {
        UserInfo.Instance.consent = true;
        UserInfo.Instance.consentTime = DateTime.Now;
        errMessage.text = "";
        consentForm.SetActive(false);
    }

    public void RefuseConsent()
    {
        UserInfo.Instance.consent = false;
        consentForm.SetActive(false);
    }
    
    public void GetToken()
    {
        UserInfo.Instance.tokenId = TokenField.text.ToString();
    }

    public void GetHandedness()
    {
        UserInfo.Instance.handedness = HandednessField.text.ToString();
    }

        // This is called in the ReadData() .js function, supplying the requested data as its argument
    public void StringCallback (string reqData)
    {
        tokenClass tokenObject = JsonUtility.FromJson<tokenClass>(reqData);
        if(tokenObject.available == true)
        {

            UpdateToken("tokenTable", 
                        UserInfo.Instance.tokenId.ToString());
            //cursor was locked here
            SceneManager.LoadScene("SettingsMenu");
        }
        else if(tokenObject.available == false)
        {
            errMessage.text = "The token you have entered has already been used.";
        }
    }

    private void ToggleFullscreen(string stateReq)
    {
        if(stateReq == "fullscreen")
        {
            UserInfo.Instance.fullscreen = true;
        }
        else if(stateReq == "exitFullscreen")
        {
            UserInfo.Instance.fullscreen = false;
        }
    }

    public void ErrorCallback(string error)
        {
            errMessage.text = error;
        }
 
}
