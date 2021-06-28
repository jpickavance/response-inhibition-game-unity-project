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
    public GameObject taskApparatus;
    public GameObject Instructions;
    public ExperimentController experimentController;
    public CameraController cameraController;
    public bool oneTime;
    public bool experiment;
    public bool aimQs;
    void Start()
    {
        oneTime = false;
        if(UserInfo.Instance.fullscreen)
        {
            taskApparatus.SetActive(true);
            cameraController.ScaleFullScreenCamera();
            pauseScreen.SetActive(false);
            switchToLock.SetActive(true);
            switchToFullScreen.SetActive(false);
        }
        else
        {
            taskApparatus.SetActive(false);
            cameraController.ExitFullScreenCamera();
            pauseScreen.SetActive(true);
            switchToFullScreen.SetActive(true);
        }
        if(experimentController == null && UserInfo.Instance.GameMode != "debug" && !aimQs) //only set listener functions if this is the calibration pause object (i.e. experiment controller not yet created)
        {
            UserInfo.Instance.locked = false;
            fullscreenListener();
            lockListener();
        }
    }

    void Update()
    {
        if(!oneTime)
        {
            if(!UserInfo.Instance.fullscreen || (!UserInfo.Instance.locked && !aimQs))
            {
                pauseScreen.SetActive(true);
                Instructions.SetActive(false);
                Time.timeScale = 0;

                if(!UserInfo.Instance.fullscreen)
                {
                    switchToLock.SetActive(false);
                }
                else
                {
                    if(experimentController.GameProgress != "tutorial2" && experimentController.trial !=20)
                    {
                        switchToLock.SetActive(true);
                    }
                    else
                    {
                        switchToLock.SetActive(false);
                    }
                }
            }
            else
            {
                pauseScreen.SetActive(false);
                switchToLock.SetActive(false);
                Instructions.SetActive(true);
                Time.timeScale = 1;
            }
        }
        else if(experiment || aimQs)
        {
            pauseScreen.SetActive(false);
            switchToLock.SetActive(false);
            Instructions.SetActive(true);
            switchToFullScreen.SetActive(false);
        }
        else
        {
            pauseScreen.SetActive(false);
            switchToLock.SetActive(false);
            Instructions.SetActive(false);
            switchToFullScreen.SetActive(false);
        }
    }

    public void ToggleFullscreen(string stateReq)
    {
        if(!oneTime)
        {
            if(stateReq == "fullscreen")
            {
                UserInfo.Instance.fullscreen = true;
                switchToFullScreen.SetActive(false);
                taskApparatus.SetActive(true);
                cameraController.ScaleFullScreenCamera();
                if(!aimQs)
                {
                Cursor.lockState = CursorLockMode.Locked;
                }
                
                
            }
            else if(stateReq == "exitFullscreen")
            {
                UserInfo.Instance.fullscreen = false;
                switchToFullScreen.SetActive(true);
                taskApparatus.SetActive(false);
                experimentController.pauses += 1;
                cameraController.ExitFullScreenCamera();
                UserInfo.Instance.trials_paused.Add(experimentController.trial + ":" + experimentController.GameState);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if(stateReq == "fullscreen")
            {
                UserInfo.Instance.fullscreen = true;
                if(!aimQs)
                {
                Cursor.lockState = CursorLockMode.Locked;
                }
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

    public void MirrorXPos(GameObject instructionsObject)
    {
        instructionsObject.GetComponent<RectTransform>().localPosition = new Vector3 (-instructionsObject.transform.localPosition.x, instructionsObject.transform.localPosition.y);
    }
}
