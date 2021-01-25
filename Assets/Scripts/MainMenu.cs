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
    public InputField AgeField;
    public int ageYears;
    public Text errMessage;
    public GameObject consentForm;
    public DateTime consentTime;
    public DateTime startTime;
    public bool genderTicked;
    public ToggleGroup genderToggle;
    public bool handTicked;
    public ToggleGroup handToggle;
    public bool pointerTicked;
    public ToggleGroup pointerToggle;
    public CameraController cameraController;
    public GameObject Buttons;
    public GameObject Form;

    [System.Serializable]
    public class tokenClass
    {
        public string tokenId;
        public bool available;
        public string gameProgress;
        public int trial;
        public int SSD;
        public int score;
    }

    public void Awake()
    {
        TokenField.Select();
    }

    public void Start()
    {
        UserInfo.Instance.consent = false;
        genderTicked = false;
        handTicked = false;
        pointerTicked = false;
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
        if((UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan") && (UserInfo.Instance.consent == false || !handTicked || !pointerTicked || !genderTicked || ageYears < 18))
        {
            if(ageYears < 18)
            {
                errMessage.text = "You must be 18 years or older to participate";
            }
            if(!genderTicked)
            {
                errMessage.text = "You must indicate your gender before participating in the experiment";
            }
            if(!handTicked)
            {
                errMessage.text = "You must select your preferred hand before participating in the experiment";
            }
            else if(!pointerTicked)
            {
                errMessage.text = "You must select your pointer device before participating in the experiment";
            }
            else if(UserInfo.Instance.consent == false)
            {
                errMessage.text = "You must provide your consent before participating in the experiment";
            }
        }
        else
        {
            if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan")
            {
                ReadData("JP_FBS_Pilot_TokenTable", UserInfo.Instance.tokenId.ToString());
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
        cameraController.ScaleFullScreenCamera();
        consentForm.SetActive(true);
        Buttons.SetActive(false);
        Form.SetActive(false);
    }
    public void GiveConsent()
    {
        UserInfo.Instance.consent = true;
        UserInfo.Instance.consentTime = DateTime.Now;
        errMessage.text = "";
        consentForm.SetActive(false);
        Buttons.SetActive(true);
        Form.SetActive(true);
    }

    public void RefuseConsent()
    {
        UserInfo.Instance.consent = false;
        consentForm.SetActive(false);
        Buttons.SetActive(true);
        Form.SetActive(true);
    }
    
    public void GetToken()
    {
        UserInfo.Instance.tokenId = TokenField.text.ToString();
    }

    public void GetAge()
    {
        ageYears = Convert.ToInt32(AgeField.text);
        UserInfo.Instance.age = AgeField.text.ToString();
    }

    public void GenderFemale()
    {
        UserInfo.Instance.gender = "female";
        genderTicked = true;
        genderToggle.allowSwitchOff = false;
    }
    public void GenderMale()
    {
        UserInfo.Instance.gender = "male";
        genderTicked = true;
        genderToggle.allowSwitchOff = false;
    }

    public void HandLeft()
    {
        UserInfo.Instance.handedness = "left";
        handTicked = true;
        handToggle.allowSwitchOff = false;
    }

    public void HandRight()
    {
        UserInfo.Instance.handedness = "right";
        handTicked = true;
        handToggle.allowSwitchOff = false;
    }

    public void PointerMouse()
    {
        UserInfo.Instance.pointer = "mouse";
        pointerTicked = true;
        pointerToggle.allowSwitchOff = false;
    }

    public void PointerTrackpad()
    {
        UserInfo.Instance.pointer = "trackpad";
        pointerTicked = true;
        pointerToggle.allowSwitchOff = false;
    }

        // This is called in the ReadData() .js function, supplying the requested data as its argument. Checks token table for existence and completion
    public void StringCallback (string reqData)
    {
        tokenClass tokenObject = JsonUtility.FromJson<tokenClass>(reqData);
        //if available or incomplete progess start the experiment 
        if(tokenObject.available == true || tokenObject.gameProgress != "complete")
        {
            UpdateToken("JP_FBS_Pilot_TokenTable", 
                        UserInfo.Instance.tokenId.ToString());
            //cursor was locked here
            UserInfo.Instance.gameProgress = tokenObject.gameProgress;
            UserInfo.Instance.trialProgress = tokenObject.trial;
            UserInfo.Instance.SSD = tokenObject.SSD;
            UserInfo.Instance.score = tokenObject.score;
            SceneManager.LoadScene("SettingsMenu");
        }
        else if(tokenObject.available == false)
        {
            errMessage.text = "The token you have entered has already been used";
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
