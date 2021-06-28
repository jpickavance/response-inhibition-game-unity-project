using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentController : MonoBehaviour
{
    //import .js function for inserting data into db
    [DllImport("__Internal")]
    private static extern void InsertData  (string tableName, 
                                            string tokenId, 
                                            string trialNum,
                                            string trialStartTime,
                                            string certaintyCond,
                                            string stopCond,
                                            string SSD,
                                            string moved,
                                            string hit,
                                            string mouseZero,
                                            string setupTime,
                                            string enterTime,
                                            string holdTime,
                                            string stopTime,
                                            string initiationTime,
                                            string movementTime,
                                            string feedbackTime,
                                            string timeData,
                                            string targetxData, 
                                            string posyData,
                                            string yInputData,
                                            string xInputData);


    [DllImport("__Internal")]
    private static extern void InsertUser (string tableName,
                                           string token,
                                           string age,
                                           string gender,
                                           string widthPx,
                                           string heightPx,
                                           string pxRatio,
                                           string browserVersion,
                                           string handedness,
                                           string pointer,
                                           string sensitivity,
                                           string consentTime,
                                           string tutorial1Trials,
                                           string tutorial2Trials,
                                           string startTime);

    [DllImport("__Internal")]
    private static extern void InsertLeaderboardUser   (string tableName,
                                                        string token,
                                                        int score,
                                                        string rSSRT,
                                                        string hitPerc,
                                                        string pSSRT,
                                                        string comboHigh,
                                                        string falseStarts);

    /* NO NEED TO UPDATE USER TABLE IN MTURK BUILD 
    [DllImport("__Internal")]
    private static extern void UpdateUser  (string tableName, 
                                            string tokenId, 
                                            string n_pauses,
                                            string trials_paused);
    [DllImport("__Internal")]
    private static extern void CompleteUser (string tableName, 
                                            string tokenId);

    */

    
    //variables for program
    public string URL;
    public string WID;

    public string tokenId;
    public int trial;
    public int trainingTrial;
    public Color red;
    public int n_training;
    public double n_trials;
    public int n_bins;
    public int acceptableError;
    public float setSpeed;
    public string handedness;
    public double minSetDelay;
    public double maxSetDelay;
    public float mouseZeroY;
    public int SSD;
    public DateTime trialBegin;
    public DateTime enterBegin;
    public DateTime holdBegin;
    public DateTime moveBegin;
    public DateTime initiationBegin;
    public DateTime feedbackBegin;
    public DateTime trialEnd;
    public DateTime stopBegin;
    public double setupTime;
    public double enterTime;
    public double holdTime;
    public double initiationTime;
    public double movementTime;
    public double feedbackTime;
    public double stopTime;
    public bool moved;
    public bool hit;
    public bool onetime;
    public bool reminderOnce;
    public string timeData;
    public string targetxData;
    public string posyData;
    public string yInputData;
    public string xInputData;
    public Text TrialCount;
    public Text gameOver;
    public Text Feedback;
    public Text Results;
    public GameObject escapeText;
    public GameObject Target;
    public GameObject StartPoint;
    public GameObject Intercept;
    public StartPointController startPointController;
    public TargetController targetController;
    public BackgroundController backgroundController;
    public TrialController trialController;
    public TutorialController tutorialController;
    public Feedback feedbackController;
    public PauseController pauseController;
    public GameObject onCloseListener;
    public MouseMove mouseMove;
    public GameObject Bat;
    public GameObject clickReminder;
    public GameObject ResultScreen;
    public string GameState;
    public string GameProgress;
    //stringified dependent variables for database
    public string tTrial;
    public string certainty;
    public string stopTrial; 
    public string str_SSD;
    public string str_moved;
    public string str_hit;
    public int pauses;
    public double n_hits;
    public int n_pauses;
    public int n_falsestarts;
    public int combo;
    public int scoreIncrement;
    public int score;
    public List<int> certFlightTimes;
    public List<int> uncertFlightTimes;
    public List<string> trials_paused;
    public List<int> comboList;
    public string hitPerc;
    public string flightTime;
    public string highCombo;
    public string falseStarts;
    public string proactiveScore;
    public string reactiveScore;
    public Sprite finalGrade;
    public GameObject gradeGraphic;
    public AudioClip Stamp;
    public AudioClip trumpet;
    public AudioSource audioSource;
    public Sprite gradeA;
    public Sprite gradeB;
    public Sprite gradeC;
    public Sprite gradeF;
    public bool resultsDisplayed;
    //functions and methods
    void Awake()
    {
        pauseController.ToggleFullscreen("fullscreen");
        handedness = UserInfo.Instance.handedness;
        audioSource = GetComponent<AudioSource>();
        if(handedness == "left")
        {
            TrialCount.GetComponent<RectTransform>().localPosition = new Vector2(-TrialCount.transform.localPosition.x, TrialCount.transform.localPosition.y);
            pauseController.MirrorXPos(escapeText);
        }
        if(UserInfo.Instance.GameMode != "debug" && UserInfo.Instance.tokenId != "notryan")
        {
            tokenId = UserInfo.Instance.tokenId.ToString();
            if(UserInfo.Instance.gameProgress != "tutorial1" || UserInfo.Instance.trialProgress != 0)
            {
                Debug.LogError("Loading save state....");
                GameProgress = UserInfo.Instance.gameProgress;
                trial = UserInfo.Instance.trialProgress;
                SSD = UserInfo.Instance.SSD;
                score = UserInfo.Instance.score;
                //add dummy data to lists so leaderboard doesn't crash on late rejoin
                comboList.Add(1);
                certFlightTimes.Add(200);
                uncertFlightTimes.Add(200);
                //////////////////////////////////////////////////////////
                Target.GetComponent<TargetController>().speed = setSpeed;
                scoreIncrement = 100;
                gameOver.text = "";
                moved = false;
                hit = false;
                resultsDisplayed = false;
                pauseController.experiment = true;
                StartTrial();
            }
            else
            {
                Debug.LogError("Loading new state....");
                GameProgress = "tutorial1";
                n_hits = 0;
                trial = 0;
                trainingTrial = 0;
                pauses = 0;
                score = 0;
                scoreIncrement = 100;
                gameOver.text = "";
                moved = false;
                hit = false;
                resultsDisplayed = false;
                pauseController.experiment = true;
                if(!UserInfo.Instance.zeroAimQuestions || UserInfo.Instance.n_aimQuestions == 0)
                {
                    tutorialController.StartTutorialTrial();
                }
                else if(UserInfo.Instance.n_aimQuestions == 1)
                {
                    trial = 20;
                    GameProgress = "tutorial2";
                }
            }
        }
        else
        {
            GameProgress = "tutorial1";
            n_hits = 0;
            trial = 0;
            trainingTrial = 0;
            pauses = 0;
            score = 0;
            scoreIncrement = 100;
            gameOver.text = "";
            moved = false;
            hit = false;
            resultsDisplayed = false;
            pauseController.experiment = true;
            if(!UserInfo.Instance.zeroAimQuestions || UserInfo.Instance.n_aimQuestions == 0)
            {
                tutorialController.StartTutorialTrial();
            }
            else if(UserInfo.Instance.n_aimQuestions == 1)
            {
                trial = 20;
                GameProgress = "tutorial2";
            }
        }
    }
    public void Update()
    {
        if(GameState == "gameover" && resultsDisplayed)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("clicked");
                GameOver();
            }
        }
    }
    public void StartTrial()
    {
        trialBegin = DateTime.Now;
        mouseMove.timeList = new List<string>();
        mouseMove.targetPosxList = new List<string>();
        mouseMove.posyList = new List<string>();
        mouseMove.yInputList = new List<string>();
        mouseMove.xInputList = new List<string>();
        timeData = "";
        targetxData = "";
        posyData = "";
        yInputData = "";
        xInputData = "";
        moved = false;
        hit = false;
        randomiseSetDelay(minSetDelay, maxSetDelay);
        ResetApparatus();
        onetime = false;
        GameState = "setup";
        if(GameProgress == "training")
        {
            TrialCount.text = "In a row: " + trial.ToString() + "/" + n_training;
        }
        else if(GameProgress == "experiment")
        {
            CheckCertainty();
            TrialCount.text = "Level: \t" + (trial + 1).ToString() + "/" + n_trials.ToString();
        }
        else
        {
            TrialCount.text = "";
        }

    }
    public void TriggerTarget()
    {
        moveBegin = DateTime.Now;
        startPointController.clicked = false;
        GameState = "moving";
    }

    public void GiveFeedback()
    {
        feedbackBegin = DateTime.Now;
        //StartCoroutine(ClickCountdown());
        reminderOnce = true;
        setupTime = Math.Round((enterBegin - trialBegin).TotalMilliseconds, 0);
        enterTime = Math.Round((holdBegin - enterBegin).TotalMilliseconds, 0);
        holdTime = Math.Round((moveBegin - holdBegin).TotalMilliseconds, 0);
        initiationTime = Math.Round((initiationBegin - moveBegin).TotalMilliseconds, 0);
        movementTime = Math.Round((feedbackBegin - initiationBegin).TotalMilliseconds, 0);
        if(GameProgress == "experiment" && trialController.stopTrials[trial] == 1)
        {
            stopTime = Math.Round((stopBegin - moveBegin).TotalMilliseconds, 0);
        }
        else
        {
            stopTime = 0;
        }
        //when training is finished
        if(GameProgress == "training" && (trial == (n_training - 1) && hit && movementTime >= (200 - acceptableError) && movementTime <= (200 + acceptableError) || trainingTrial >= 38))
        {
            feedbackController.trainingComplete = true;
            tutorialController.TrainingComplete.SetActive(true);
            tutorialController.Instructions10.SetActive(false);
            TrialCount.text = "";
            feedbackController.Splat.SetActive(false);
        }
        GameState = "feedback";
    }

    public void EndTrial()
    {
        trialEnd = DateTime.Now;
        StopAllCoroutines();
        if(GameProgress == "training")
        {
            tTrial = "t" + trainingTrial.ToString();
            certainty = "x";
            stopTrial = "x"; 
        }
        else
        {
            tTrial = trial.ToString();
            certainty = trialController.certainty[trial].ToString();
            stopTrial = trialController.stopTrials[trial].ToString();
        }

        if(stopTrial == "0")
        {
            str_SSD = "x";
        }
        else
        {
            str_SSD = SSD.ToString();
        }

        if(moved)
        {
            str_moved = "1";
        }
        else
        {
            str_moved = "0";
        }

        if(hit)
        {
            str_hit = "1";
        }
        else
        {
            str_hit = "0";
        }

        feedbackTime = Math.Round((trialEnd - feedbackBegin).TotalMilliseconds, 0);
        timeData = String.Join(", ", mouseMove.timeList.ToArray());
        targetxData = String.Join(",", mouseMove.targetPosxList.ToArray());
        posyData = String.Join(", ", mouseMove.posyList.ToArray());
        yInputData = String.Join(", ", mouseMove.yInputList.ToArray());
        xInputData = String.Join(", ", mouseMove.xInputList.ToArray());

        if(UserInfo.Instance.GameMode != "debug" && GameProgress != "tutorial1" && GameProgress != "tutorial2" && UserInfo.Instance.tokenId != "notryan")
        {
            InsertData (UserInfo.Instance.TrialTable,
            UserInfo.Instance.tokenId.ToString(),
            tTrial.ToString(),
            trialBegin.ToString(),
            certainty,
            stopTrial,
            str_SSD,
            str_moved,
            str_hit,
            mouseZeroY.ToString(),
            setupTime.ToString(),
            enterTime.ToString(),
            holdTime.ToString(),
            stopTime.ToString(),
            initiationTime.ToString(),
            movementTime.ToString(),
            feedbackTime.ToString(),
            timeData,
            targetxData,
            posyData,
            yInputData,
            xInputData);
        }
        else
        {
            Debug.Log("time: " + timeData);
            Debug.Log("targetPos: " + targetxData);
            Debug.Log("mouseZero: " + mouseZeroY.ToString());
            Debug.Log("Sensitivity: " + UserInfo.Instance.mouseSensitivity.ToString());
            Debug.Log("posyData: " + posyData);
            Debug.Log("yInput: " + yInputData);
            Debug.Log("xInput: " + xInputData);        
        }

        if(GameProgress == "training")
        {
            trainingTrial +=1;
            if(movementTime >= (200 - acceptableError) && movementTime <= (200 + acceptableError) && hit)
            {
                trial += 1;
            }
            else
            {
                trial = 0;
            }
            //when training is finished
            if(trial == n_training || trainingTrial >= 39)
            {
                tutorialController.TrainingComplete.SetActive(false);
                tutorialController.Instructions10.SetActive(false);
                trial = 0;
                GameProgress = "tutorial2"; //was experiment
            }
            StartTrial();
        }
        else if(GameProgress == "experiment")
        {
            if((trialController.stopTrials[trial] == 0 && hit) || (trialController.stopTrials[trial] == 1 && !moved))
            {
                if(trialController.stopTrials[trial] == 0 && hit)
                {
                    n_hits += 1;
                }
                combo += 1;
                score += scoreIncrement;
                scoreIncrement += 100;
            }
            if((trialController.stopTrials[trial] == 0 && !hit) || (trialController.stopTrials[trial] == 1 && moved))
            {
                comboList.Add(combo);
                combo = 0;
                scoreIncrement = 0;
                if(trialController.stopTrials[trial] == 1 && moved)
                {
                    n_falsestarts += 1;
                }
            }
            if(trialController.certainty[trial] == 1 && moved)
            {
                certFlightTimes.Add(Convert.ToInt16(movementTime));
            }
            if(trialController.certainty[trial] == 0 && (trialController.stopTrials[trial] != 1) && moved)
            {
                uncertFlightTimes.Add(Convert.ToInt16(movementTime));
            }
            StaircaseSSD();
            trial += 1;
            if(trial >= n_trials) 
            {
                ShowResults();
            }
            else
            {
                StartTrial();   
            }
        }
    }
    public void ClickTimeout()
    {
        clickReminder.SetActive(true);
        feedbackController.Splat.SetActive(false);
        feedbackController.Cave.SetActive(false);
        feedbackController.hitFeedback.text = "";
    }
    IEnumerator ClickCountdown()
    {
        yield return new WaitForSeconds(5.0f);
        ClickTimeout();
    }
    public void GameOver()
    {
        if(UserInfo.Instance.GameMode != "debug")
        {
            string trialPauseData = String.Join(", ", UserInfo.Instance.trials_paused.ToArray());
            /*UpdateUser(UserInfo.Instance.UserTable,
            UserInfo.Instance.tokenId,
            pauses.ToString(),
            trialPauseData);
            */
        }
        if(UserInfo.Instance.aimQuestions)
        {
            SceneManager.LoadScene("AimQuestions");
        }
        else
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    public void CheckCertainty()
    {
        if(trialController.certainty[trial] == 0)
        {
            backgroundController.DayTime();
        }
        else
        {
            backgroundController.NightTime();
        }
    }

    public void StaircaseSSD()
    {
        if(trialController.stopTrials[trial] == 1 && moved)
        {
            SSD -= 50;
        }
        else if(trialController.stopTrials[trial] == 1 && !moved)
        {
            SSD += 50;
        }
    }
    public void ResetApparatus()
    
    {
        StartPoint.SetActive(true);
        Target.SetActive(false);
        Target.GetComponent<TargetController>().transform.position = targetController.StartPos;
        Intercept.SetActive(true);
        //reset Bat to check if participant is already in start zone with physics2d.overlaparea (StartPointController)
        Bat.SetActive(false);
        Bat.SetActive(true);
    }

    public void randomiseSetDelay(double minNumber, double maxNumber)
    {
        double rng = new System.Random().NextDouble() * (maxNumber - minNumber) + minNumber;
        rng = Math.Round(rng, 3);
        startPointController.setDelay = Convert.ToSingle(rng);
    }

    public void CalculateResults()
    {
        hitPerc = Math.Round(((n_hits / (n_trials*(5f/6f)))*100), 0).ToString();
        flightTime = Math.Round(certFlightTimes.Average(), 0).ToString();
        if(comboList.Count == 0)
        {
            highCombo = "cheat";
        }
        else
        {
            highCombo = comboList.Max().ToString();
        }
        falseStarts = n_falsestarts.ToString();
        proactiveScore = Math.Round((certFlightTimes.Average() - uncertFlightTimes.Average()), 0).ToString();
        reactiveScore = (980 - SSD).ToString();
        if(n_hits >= (n_trials*(5f/6f) -5))
        {
            if(certFlightTimes.Average() < 250 && certFlightTimes.Average() > 150)
            {
                finalGrade = gradeA;
            }
            else if(certFlightTimes.Average() < 300 && certFlightTimes.Average() > 100)
            {
                finalGrade = gradeA;
            }
            else
            {
                finalGrade = gradeB;
            }
        }
        else
        {
            finalGrade = gradeC;
        }
    }
    public void ShowResults()
    {
        GameState = "gameover";
        //handle elements of UI that must stay open (hacky because pause controller same for calibration)
        TrialCount.text = "";
        feedbackController.hitFeedback.text = "";
        feedbackController.Splat.SetActive(false);
        feedbackController.Cave.SetActive(false);
        clickReminder.GetComponent<Text>().text = "";
        //make sure pause no longer works in experiment scene
        pauseController.oneTime = true;
        //calculate and show results with fanfare
        CalculateResults();
        Results.text = "\t\t\t\t\t" + hitPerc + "\t\t\t\t\t\t\t\t" + flightTime + "ms\n\n\t\t\t\t\t" + highCombo + "\t\t\t\t\t\t\t\t" + falseStarts + "\n\n\t\t\t\t\t\t\t" + proactiveScore + "ms\n\n\t\t\t\t\t\t\t" + reactiveScore + "ms";
        audioSource.PlayOneShot(trumpet, 1f);
        ResultScreen.SetActive(true);
        StartCoroutine(GradeStamp());
    }
    
    private void PostUserData()
    {
        /*CompleteUser(UserInfo.Instance.TokenTable,
                     UserInfo.Instance.tokenId.ToString());
                     */
    
        InsertUser(UserInfo.Instance.UserTable,  
                    UserInfo.Instance.tokenId.ToString(),
                    UserInfo.Instance.age.ToString(),
                    UserInfo.Instance.gender.ToString(),
                    UserInfo.Instance.widthPx.ToString(),
                    UserInfo.Instance.heightPx.ToString(),
                    UserInfo.Instance.pxRatio.ToString(),
                    UserInfo.Instance.browserVersion.ToString(),
                    UserInfo.Instance.handedness.ToString(),
                    UserInfo.Instance.pointer.ToString(),
                    UserInfo.Instance.mouseSensitivity.ToString(),
                    UserInfo.Instance.consentTime.ToString(),
                    tutorialController.tutorial1Trials.ToString(),
                    tutorialController.tutorial2Trials.ToString(),
                    tutorialController.startTime.ToString());
        
        InsertLeaderboardUser  (UserInfo.Instance.LeaderboardTable,
                                UserInfo.Instance.username.ToString(),
                                score,
                                reactiveScore,
                                hitPerc,
                                proactiveScore,
                                highCombo,
                                falseStarts);
    }

    IEnumerator GradeStamp()
    {
        yield return new WaitForSeconds(2.5f);
        onCloseListener.SetActive(false);
        if(UserInfo.Instance.GameMode != "debug")
        {
            PostUserData();
        }
        Cursor.lockState = CursorLockMode.None;
        gradeGraphic.GetComponent<SpriteRenderer>().sprite = finalGrade;
        audioSource.PlayOneShot(Stamp, 0.5f);
        gradeGraphic.SetActive(true);
        resultsDisplayed = true;
    }
    public void StringCallback (string info)
    {
        string Output = info;
    }
}

