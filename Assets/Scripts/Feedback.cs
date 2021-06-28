using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feedback : MonoBehaviour
{
    public Text hitFeedback;
    public Text movementFeedback;
    public Color red;
    public Color green;
    public GameObject tooSlowWarning;
    public GameObject Splat;
    public GameObject Cave;
    public GameObject BatCursor;
    public GameObject SmokeEffect;
    public AudioClip splatSound;
    public AudioClip trumpet;
    public AudioClip caveNoises;
    public AudioClip burnSound;
    public Animator batAnimatorController;
    AudioSource audioSource;
    public ExperimentController experimentController;
    public TrialController trialController;
    public TutorialController tutorialController;
    public bool trainingComplete;
    public bool feedbackOneTime;
    public bool warningOneTime;
    void Awake()
    {   
        Splat.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        hitFeedback.text = "";
        if(UserInfo.Instance.handedness == "left")
        {
            movementFeedback.GetComponent<RectTransform>().localPosition = new Vector3 (-movementFeedback.transform.localPosition.x, movementFeedback.transform.localPosition.y);
        }
        
    }

    public void Update()
    {
        if(experimentController.GameState == "feedback")
        {
            if(!feedbackOneTime)
            {   
                StartCoroutine(ClickCountdown());
                feedbackOneTime = true;
                if(experimentController.GameProgress == "training" || experimentController.GameProgress == "tutorial1" || (experimentController.GameProgress == "experiment" && trialController.stopTrials[experimentController.trial] == 0) || (experimentController.GameProgress == "tutorial2" && experimentController.hit && experimentController.trial == 19))
                {
                    if(!((experimentController.GameProgress == "training" && trainingComplete) || (experimentController.GameProgress == "tutorial2")))
                    {             
                    HitFeedback();
                    }
                    else
                    {
                        if (!(UserInfo.Instance.zeroAimQuestions && experimentController.GameProgress == "tutorial2"))
                        {
                            audioSource.PlayOneShot(trumpet, 1f);
                        }
                    }
                }
                else if(experimentController.GameProgress == "tutorial2")
                {
                    if(tutorialController.tutorial2StopTrials.Contains(experimentController.trial) || tutorialController.tutorial2ChangeTrials.Contains(experimentController.trial))
                    {
                        StopFeedback();
                    }
                    else
                    {
                        if(experimentController.GameProgress == "tutorial2" && experimentController.trial != 19)
                        {
                            HitFeedback();
                        }   
                    }
                }
                else if(experimentController.GameProgress == "experiment" && trialController.stopTrials[experimentController.trial] == 1)
                {
                    StopFeedback();
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                if(experimentController.GameProgress != "tutorial1" || experimentController.GameProgress != "tutorial2")
                {
                    if(experimentController.GameProgress == "experiment" && (experimentController.movementTime < 300 || trialController.stopTrials[experimentController.trial] == 1) || experimentController.GameProgress == "training")
                    {
                        NextTrial();
                    }
                    else if(experimentController.GameProgress == "experiment" && !warningOneTime)
                    {
                        StartCoroutine(WarningTimeout());
                        warningOneTime = true;
                    }
                }
            }
        }
        else
        {
            feedbackOneTime = false;
            Splat.SetActive(false);
            Cave.SetActive(false);
            hitFeedback.text = "";
            movementFeedback.text = "";
        }
    }

    public void HitFeedback()
    {
        if(experimentController.hit)
        {
            if(experimentController.GameProgress == "experiment" && experimentController.movementTime > 300) 
            {
                tooSlowWarning.SetActive(true);
            }
            else
            {
                audioSource.PlayOneShot(splatSound, 0.5f);
                Splat.SetActive(true);
                hitFeedback.GetComponent<Text>().color = green;
                MovementFeedback();
            }
            
        }
        else
        {
            if(experimentController.GameProgress == "experiment" && experimentController.movementTime > 300) 
            {
                tooSlowWarning.SetActive(true);
            }
            else
            {
                hitFeedback.text = "MISS!";
                hitFeedback.GetComponent<Text>().color = red;
                MovementFeedback();
            }
        }
    }
    public void MovementFeedback()
    {
        //only show movement time feedback in training and tutorial for training
        if(experimentController.GameProgress == "training" || (experimentController.GameProgress == "tutorial1" && experimentController.trial > 0))
        {
            if(experimentController.movementTime > 200 + experimentController.acceptableError)
            {
                movementFeedback.text = experimentController.movementTime.ToString() + "\nToo Slow";
                movementFeedback.GetComponent<Text>().color = red;
            }
            else if(experimentController.movementTime < 200 - experimentController.acceptableError)
            {
                movementFeedback.text = experimentController.movementTime.ToString() + "\nToo Fast";
                movementFeedback.GetComponent<Text>().color = red;
            }
            else
            {
                movementFeedback.text = experimentController.movementTime.ToString() + "\nGood!";
                movementFeedback.GetComponent<Text>().color = green;
            }
        }
    }

    public void StopFeedback()
    {
        if(!experimentController.moved)
        {
            Cave.SetActive(true);
            audioSource.PlayOneShot(caveNoises, 1f);
            hitFeedback.GetComponent<Text>().color = green;
        }
        else
        {
            hitFeedback.text = "OUCH!";
            hitFeedback.GetComponent<Text>().color = red;
            audioSource.PlayOneShot(burnSound, 0.5f);
            batAnimatorController.SetBool("BatBurn", true);
            Vector3 SmokePos = BatCursor.GetComponent<Transform>().position;
            SmokePos.y = SmokePos.y + 1.8f;
            SmokeEffect.transform.position = SmokePos; 
            SmokeEffect.SetActive(true);
        }
    }
    public void NextTrial()
    {
        StopAllCoroutines();
        tooSlowWarning.SetActive(false);
        SmokeEffect.SetActive(false);
        experimentController.clickReminder.SetActive(false);
        if(experimentController.GameProgress != "tutorial1" && experimentController.GameProgress != "tutorial2")
        {
            experimentController.EndTrial();
        }
    }
    IEnumerator ClickCountdown()
    {
        yield return new WaitForSeconds(5.0f);
        if(!warningOneTime)
        {
            experimentController.ClickTimeout();
        }
    }
    IEnumerator WarningTimeout()
    {
        yield return new WaitForSeconds(5.0f);
        warningOneTime = false;
        NextTrial();
    }
}
