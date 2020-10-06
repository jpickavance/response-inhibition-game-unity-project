using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern string fullscreenListener();
    [DllImport("__Internal")]
    private static extern string lockListener();
    public GameObject pauseScreen;
    public GameObject switchToFullScreen;
    public GameObject switchToLock;
    public GameObject escapeText;
    public GameObject taskApparatus;
    public GameObject Instructions;
    public ExperimentController experimentController;
    public TutorialController tutorialController;
    public bool oneTime;
    public bool experiment;
    void Start()
    {
        oneTime = false;
        if(UserInfo.Instance.fullscreen)
        {
            taskApparatus.SetActive(true);
            pauseScreen.SetActive(false);
            switchToLock.SetActive(true);
            switchToFullScreen.SetActive(false);
        }
        else
        {
            taskApparatus.SetActive(false);
            pauseScreen.SetActive(true);
            switchToFullScreen.SetActive(true);
        }
        if(experimentController == null && UserInfo.Instance.GameMode != "debug") //only set listener functions if this is the calibration pause object (i.e. experiment controller not yet created)
        {
            UserInfo.Instance.locked = false;
            fullscreenListener();
            lockListener();
        }
        else
        {
            if(UserInfo.Instance.handedness == "left")
            {
                MirrorXPos(escapeText);
            }
        }
    }

    void Update()
    {
        if(!oneTime)
        {
            if(!UserInfo.Instance.fullscreen || !UserInfo.Instance.locked)
            {
                pauseScreen.SetActive(true);
                escapeText.SetActive(false);
                Instructions.SetActive(false);
                Time.timeScale = 0;

                if(!UserInfo.Instance.fullscreen)
                {
                    switchToLock.SetActive(false);
                }
                else
                {
                    switchToLock.SetActive(true);
                }
            }
            else
            {
                pauseScreen.SetActive(false);
                switchToLock.SetActive(false);
                escapeText.SetActive(true);
                Instructions.SetActive(true);
                Time.timeScale = 1;
            }
        }
        else if(experiment == true)
        {
            pauseScreen.SetActive(false);
            switchToLock.SetActive(false);
            escapeText.SetActive(false);
            Instructions.SetActive(true);
            switchToFullScreen.SetActive(false);
        }
        else
        {
            pauseScreen.SetActive(false);
            switchToLock.SetActive(false);
            escapeText.SetActive(false);
            Instructions.SetActive(false);
            switchToFullScreen.SetActive(false);
        }
    }

    private void ToggleFullscreen(string stateReq)
    {
        if(!oneTime)
        {
            if(stateReq == "fullscreen")
            {
                UserInfo.Instance.fullscreen = true;
                switchToFullScreen.SetActive(false);
                taskApparatus.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(stateReq == "exitFullscreen")
            {
                UserInfo.Instance.fullscreen = false;
                switchToFullScreen.SetActive(true);
                taskApparatus.SetActive(false);
                experimentController.pauses += 1;
                UserInfo.Instance.trials_paused.Add(experimentController.trial + ":" + experimentController.GameState);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if(stateReq == "fullscreen")
            {
                UserInfo.Instance.fullscreen = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(stateReq == "exitFullscreen")
            {
                UserInfo.Instance.fullscreen = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void ToggleLock(string stateReq)
    {
            if(stateReq == "lock")
            {
                UserInfo.Instance.locked = true;
            }
            else if(stateReq == "unlock")
            {
                UserInfo.Instance.locked = false;
            }
    }

    private void MirrorXPos(GameObject instructionsObject)
    {
        instructionsObject.GetComponent<RectTransform>().localPosition = new Vector3 (-instructionsObject.transform.localPosition.x, instructionsObject.transform.localPosition.y);
    }
}
