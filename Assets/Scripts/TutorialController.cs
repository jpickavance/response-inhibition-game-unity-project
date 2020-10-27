using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class TutorialController : MonoBehaviour
{
    public ExperimentController experimentController;
    public BackgroundController backgroundController;
    public Feedback feedbackController;
    public Vector3 leftTransform = new Vector3(-1f, 1f, 1f);
    public GameObject Instructions1;
    public GameObject Instructions2;
    public GameObject Instructions3;
    public GameObject Instructions4;
    public GameObject Instructions5;
    public GameObject Instructions7;
    public GameObject Instructions8;
    public Text instructions8Text;
    public GameObject Instructions9;
    public Text instructions9Text;
    public GameObject Instructions10;
    public Text instructions10Text;
    public GameObject Instructions11;
    public GameObject Instructions13;
    public GameObject Instructions14;
    public GameObject Instructions15;
    public GameObject Instructions16;
    public GameObject Instructions17;
    public Text instructions17Text;
    public GameObject InstructionsMiss1;
    public GameObject InstructionsMoved;
    public GameObject TrainingComplete;
    public GameObject Bat;
    public GameObject TrialCount;
    public GameObject GameOver;
    public GameObject Target;
    public GameObject FeedbackHit;
    public GameObject FeedbackMovement;
    public GameObject counter1;
    public GameObject counter2;
    public int counter1Int;
    public int counter2Int;
    public Text counter1Text;
    public Text counter2Text;
    public int tutorial1Trials;
    public int tutorial2Trials;
    public List<int> tutorial2StopTrials;
    public List<int> tutorial2UncertainTrials;
    public List<int> tutorial2ChangeTrials;
    public bool onetime;
    public DateTime startTime;

    void Awake()
    {
        //reverse x coordinates of tutorial instructions for left handers
        if(UserInfo.Instance.handedness == "left")
        {
            MirrorXPos(Instructions1);
            MirrorXPos(Instructions2);
            MirrorXPos(Instructions3);
            MirrorXPos(Instructions4);
            MirrorXPos(Instructions5);
            MirrorXPos(Instructions7);
            MirrorXPos(Instructions8);
            MirrorXPos(Instructions9);
            MirrorXPos(Instructions10);
            MirrorXPos(Instructions11);
            MirrorXPos(Instructions13);
            MirrorXPos(Instructions14);
            MirrorXPos(Instructions15);
            MirrorXPos(Instructions16);
            MirrorXPos(InstructionsMiss1);
            MirrorXPos(InstructionsMoved);
            MirrorXPos(counter1);
            MirrorXPos(counter2);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        tutorial1Trials = 0;
        tutorial2Trials = 0;
        counter1Int = 0;
        onetime = false;
        tutorial2StopTrials = new List<int>{0, 2, 4, 5};
        tutorial2UncertainTrials = new List<int>{7, 8, 9, 10, 13, 14, 15, 16, 18};
        tutorial2ChangeTrials = new List<int>{8, 10, 13, 16, 18};
        instructions8Text.text = "A good flight time is between " + (200-experimentController.acceptableError).ToString() + " and " + (200+experimentController.acceptableError);
        instructions9Text.text = "Hit the fruit\nMove with a good flight time (" + (200-experimentController.acceptableError).ToString() + "-" + (200+experimentController.acceptableError) + ")\nUse the flight timer after each flight to help you adjust";
        instructions10Text.text = "Hit the fruit\n Move with a good flight time\nGoal: " + experimentController.n_training.ToString() + " times in a row";
        instructions17Text.text = "You have completed training\nThe rules are the same for the full game:\n\n\t<b>Night</b> - hit the fruit with a good flight time\n\t<b>Sunrise</b> - hit the fruit with a good flight time\n\t<b>Day</b> - stay inside the cave\n\nThere will be " + experimentController.n_trials.ToString() + " trials in total\nYou will be graded on your performance at the end\n\nGood luck!";
    }

    // Update is called once per frame
    void Update()
    {
        if(experimentController.GameProgress == "tutorial1")
        {
            Tutorial1();
        }
        if(experimentController.GameProgress == "tutorial2")
        {
            TrainingComplete.SetActive(false);
            Tutorial2();
        }
        
    }

    public void StartTutorialTrial()
    {
        experimentController.ResetApparatus();
        Bat.GetComponent<MouseMove>().timer = 0;
        experimentController.onetime = false;
        experimentController.hit = false;
        experimentController.moved = false;
        onetime = false;
        experimentController.GameState = "setup";
    }

    public void Tutorial1()
    {
        if(experimentController.trial == 0)
            {
                if(experimentController.GameState == "setup")
                {
                    Instructions1.SetActive(true);
                    Instructions2.SetActive(false);
                    Instructions3.SetActive(false);
                }
                if(experimentController.GameState == "entering")
                {
                    Instructions1.SetActive(false);
                    Instructions2.SetActive(true);
                    InstructionsMiss1.SetActive(false);
                }
                if(experimentController.GameState == "holding")
                {
                    Instructions2.SetActive(false);
                    Instructions3.SetActive(true);
                }
                if(experimentController.GameState == "moving")
                {
                    Instructions3.SetActive(false);
                    Instructions4.SetActive(true);
                    Target.GetComponent<TargetController>().speed = 2f;
                }
                if(experimentController.GameState == "feedback")
                {
                    if(experimentController.hit)
                    {
                        Instructions4.SetActive(false);
                        Instructions5.SetActive(true);
                        if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            tutorial1Trials += 1;
                            experimentController.trial += 1;
                            Instructions5.SetActive(false);
                            Target.GetComponent<TargetController>().speed = experimentController.setSpeed;
                            experimentController.randomiseSetDelay(0.5, 2);
                            StartTutorialTrial();
                        }
                    }
                    else
                    {
                        Instructions4.SetActive(false);
                        InstructionsMiss1.SetActive(true);
                        if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            tutorial1Trials += 1;
                            experimentController.ResetApparatus();
                            InstructionsMiss1.SetActive(false);
                            experimentController.GameState = "setup";
                        }
                    }
                }
            }        
        if(experimentController.trial == 1)
        {
                if(experimentController.GameState == "setup")
                {
                    Instructions1.SetActive(true);
                    Instructions2.SetActive(false);
                    Instructions3.SetActive(false);
                }
                if(experimentController.GameState == "entering")
                {
                    Instructions1.SetActive(false);
                }
                if(experimentController.GameState == "feedback")
                {
                    Instructions7.SetActive(true);
                    Instructions8.SetActive(true);

                        if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            if(experimentController.hit)
                            {
                                experimentController.trial += 1;
                            }
                            tutorial1Trials += 1;
                            Instructions7.SetActive(false);
                            Instructions8.SetActive(false);
                            StartTutorialTrial();
                        }                    
                }
        }
        if(experimentController.trial == 2)
        {
            Instructions9.SetActive(true);
            if(experimentController.GameState == "feedback")
                {
                    if(experimentController.hit && experimentController.movementTime >= 100 && experimentController.movementTime <= 300)
                    {
                        Instructions9.SetActive(false);
                        if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            tutorial1Trials += 1;
                            StartTraining();
                        }                    
                    }
                    else
                    {
                        if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            tutorial1Trials += 1;
                            StartTutorialTrial();
                        }
                    }
                }
        }
    }
    public void Tutorial2()
    {
        if(experimentController.trial == 0)
        {
            backgroundController.StopTime();
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                Instructions11.SetActive(true);
                InstructionsMoved.SetActive(false);
            }
            if(experimentController.GameState == "feedback")
            {
                Instructions11.SetActive(false);
                if(experimentController.moved)
                {
                    InstructionsMoved.SetActive(true);
                    if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            tutorial2Trials += 1;
                            experimentController.ResetApparatus();
                            InstructionsMoved.SetActive(false);
                            experimentController.hit = false;
                            experimentController.moved = false;
                            experimentController.GameState = "setup";
                        }
                }
                else
                {
                    if(Input.GetMouseButtonDown(0))
                        {
                            feedbackController.NextTrial();
                            counter1Int += 1;
                            tutorial2Trials += 1;
                            experimentController.trial += 1;
                            experimentController.ResetApparatus();
                            InstructionsMoved.SetActive(false);
                            experimentController.hit = false;
                            experimentController.moved = false;
                            experimentController.GameState = "setup";
                        }
                }
            }
        }
        if(experimentController.trial == 1)
        {
            Instructions13.SetActive(true);
            counter1.SetActive(true);
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 2)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 3)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 4)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 5)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 6)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 7)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            { 
                setBackground();
                Instructions13.SetActive(false);
                counter1.SetActive(false);
                Instructions14.SetActive(true);
            }
            CertainTrialCheck();
            experimentController.SSD = 450;

        }
        if(experimentController.trial == 8)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            { 
                setBackground();
                InstructionsMoved.SetActive(false);
                Instructions14.SetActive(false);
                Instructions15.SetActive(true);
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 9)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
                counter2.SetActive(true);
                Instructions15.SetActive(false);
                Instructions16.SetActive(true);
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 10)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering") 
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 11)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 12)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 13)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 14)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 15)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 16)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 17)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 18)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            StopTrialCheck();
        }
        if(experimentController.trial == 19)
        {
            if(experimentController.GameState == "setup" || experimentController.GameState == "entering")
            {
                setBackground();
            }
            CertainTrialCheck();
        }
        if(experimentController.trial == 20)
        {
            Instructions17.SetActive(true);
            if(Input.GetMouseButtonDown(0))
            {
                feedbackController.NextTrial();
                Instructions17.SetActive(false);
                StartExperiment();
            }
        }
    }
    public void StartTraining()
    {
        experimentController.trial = 0;
        experimentController.randomiseSetDelay(0.5, 1.5);
        experimentController.moved = false;
        experimentController.hit = false;
        experimentController.GameProgress = "training";
        Instructions10.SetActive(true);
        experimentController.StartTrial();
    }
    public void StartExperiment()
    {   
        startTime = DateTime.Now;
        experimentController.trial = 0;
        experimentController.SSD = 780;
        experimentController.GameProgress = "experiment";
        experimentController.StartTrial();
    }

    public void CertainTrialCheck()
    {
            if(experimentController.GameState == "feedback")
            {
                if(experimentController.hit)
                {
                    if(!onetime)
                    {
                        if(experimentController.trial < 7)
                        {
                            counter1Int += 1;
                        }
                        if(experimentController.trial >= 9)
                        {
                            counter2Int += 1;
                        }
                        counter1Text.text = (counter1Int - 1).ToString() + "/6";
                        counter2Text.text = (counter2Int).ToString() + "/11";
                        onetime = true;
                        if(experimentController.trial == 19)
                        {
                            Instructions16.SetActive(false);
                            counter2.SetActive(false);
                            Instructions17.SetActive(true);
                        }
                    }
                    if(Input.GetMouseButtonDown(0))
                    {
                        feedbackController.NextTrial();
                        experimentController.trial += 1;
                        tutorial2Trials += 1;
                        StartTutorialTrial();
                    }
                }
                else
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        feedbackController.NextTrial();
                        tutorial2Trials += 1;
                        StartTutorialTrial();
                    }
                }
            }
    }
    public void StopTrialCheck()
    {
            if(experimentController.GameState == "feedback")
            {
                Instructions15.SetActive(false);
                if(!experimentController.moved)
                {
                    if(!onetime)
                    {
                        if(experimentController.trial < 7)
                        {
                            counter1Int += 1;
                        }
                        if(experimentController.trial >= 9)
                        {
                            counter2Int += 1;
                        }
                        counter1Text.text = (counter1Int - 1).ToString() + "/6";
                        counter2Text.text = (counter2Int).ToString() + "/11";
                        onetime = true;
                    }
                    if(Input.GetMouseButtonDown(0))
                    {
                        feedbackController.NextTrial();
                        experimentController.trial += 1;
                        tutorial2Trials += 1;
                        StartTutorialTrial();
                    }
                }
                else
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        feedbackController.NextTrial();
                        tutorial2Trials += 1;
                        StartTutorialTrial();
                    }
                }
            }
    }
    public void setBackground()
    {
            if(tutorial2StopTrials.Contains(experimentController.trial))
            {
                backgroundController.StopTime();
            }
            else if(tutorial2UncertainTrials.Contains(experimentController.trial))
            {
                backgroundController.DayTime();
            }
            else
            {
                backgroundController.NightTime();
            }
    }
    public void MirrorXPos(GameObject instructionsObject)
    {
        instructionsObject.GetComponent<RectTransform>().localPosition = new Vector3 (-instructionsObject.transform.localPosition.x, instructionsObject.transform.localPosition.y);
    }
}
