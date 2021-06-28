using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour
{
    public ExperimentController experimentController;
    public BackgroundController backgroundController;
    public TrialController trialController;
    public TutorialController tutorialController;
    public double timer;
    public float mouseSensitivity;
    public float mouseSensitivityTransformer = 0.1f;
    public float minY;
    public float maxY;
    public Vector2 StartPosition = new Vector2(4.5f, 0f);
    public List<string> timeList;
    public List<string> posyList;
    public List<string> yInputList;
    public List<string> xInputList;
    public List<string> targetPosxList;
    public GameObject Target;
    public Vector2 StartZoneTopLeft;
    public Vector2 StartZoneBottomRight;
    void Start()
        {
            if(experimentController.handedness == "left")
            {
                StartPosition.x = -1*StartPosition.x;
            }
            if(UserInfo.Instance.GameMode == "debug")
            {
                mouseSensitivityTransformer = mouseSensitivityTransformer*4;
            }
            transform.position = StartPosition;
            mouseSensitivity = UserInfo.Instance.mouseSensitivity;
        }
    void Update()
        {
        float yDeltaPos = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * mouseSensitivityTransformer * Time.deltaTime;
        float xDeltaPos = Input.GetAxisRaw("Mouse X") * mouseSensitivity * mouseSensitivityTransformer * Time.deltaTime;
        transform.Translate(0f, yDeltaPos, 0f);
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;

            if(experimentController.GameState == "moving" || experimentController.GameState == "feedback")
            {
                timer = Math.Round((DateTime.Now - experimentController.moveBegin).TotalMilliseconds, 0);
                string timeOutput = timer.ToString();
                string posyOutput = Math.Round(clampedPosition.y, 3).ToString();
                string yInput = Math.Round(yDeltaPos, 3).ToString();
                string xInput = Math.Round(xDeltaPos, 3).ToString();
                string targetPos = Target.transform.position.x.ToString();
                timeList.Add(timeOutput);
                posyList.Add(posyOutput);
                yInputList.Add(yInput);
                xInputList.Add(xInput);
                targetPosxList.Add(targetPos);
        
                if(timer >= experimentController.SSD && !experimentController.onetime && ((experimentController.GameProgress == "experiment" && trialController.stopTrials[experimentController.trial] == 1) || (experimentController.GameProgress == "tutorial2" && tutorialController.tutorial2ChangeTrials.Contains(experimentController.trial))))
                {
                    experimentController.stopBegin = DateTime.Now;
                    backgroundController.StopTime();
                    experimentController.onetime = true;
                }
            }
        }

    //this handles progression to next trial when participant keeps Bat in safe zone for duration
    //works because experimentcontroller resets SetActive for the Bat object at end of each trial
    void OnEnable()
        {
            if(Physics2D.OverlapArea(StartZoneTopLeft, StartZoneBottomRight))
            {
                experimentController.GameState = "holding";
            }
        }
}
