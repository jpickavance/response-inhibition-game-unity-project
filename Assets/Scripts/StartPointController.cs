using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPointController : MonoBehaviour
{
    public Color red;
    public Color green;
    public Sprite caveLeft;
    public GameObject Sky;
    public Sprite StopSky;
    public bool clicked;
    public ExperimentController experimentController;
    public TutorialController tutorialController;
    public TrialController trialController;
    public GameObject Target;
    public GameObject Bat;
    public GameObject smokeEffect;
    public Vector2 cursorStartPosition = new Vector2(4.5f, -3.6f);
    public Vector2 caveStartPosition = new Vector2(4.5f, -3.5f);
    public float setDelay = 5f;
    public float ZeroMouse;
    SpriteRenderer m_SR;
    [SerializeField] public Animator batAnimatorController;

    void Awake()
    {
        if(experimentController.handedness == "left")
        {
            caveStartPosition.x = -caveStartPosition.x;
            cursorStartPosition.x = -cursorStartPosition.x;
            gameObject.GetComponent<SpriteRenderer>().sprite = caveLeft;
        }
        transform.position = caveStartPosition;

        m_SR = GetComponent<SpriteRenderer>();
        clicked = false;
    }



    void Update()
    {
        if(experimentController.GameState == "setup")
        {
            m_SR.color = red;
        }
        if(experimentController.GameState == "entering")
        {
            m_SR.color = green;
            if(Input.GetMouseButtonDown(0) == true && !clicked)
            {
                clicked = true; //handles errors from multiple clicks
                StopAllCoroutines();
                experimentController.clickReminder.SetActive(false);
                batAnimatorController.SetBool("BatHold", true);
                batAnimatorController.SetBool("BatBurn", false);
                experimentController.GameState = "holding";
                experimentController.holdBegin = DateTime.Now;
                ZeroMouse = Bat.transform.position.y - cursorStartPosition.y;
                Bat.transform.position = cursorStartPosition;
                experimentController.mouseZeroY = ZeroMouse;
                Target.SetActive(true);
                StartCoroutine(moveCountdown());
            }
        }
    }

    //create a coroutine function that doesn't execute until after 500ms
    IEnumerator moveCountdown()
    {
        yield return new WaitForSeconds(setDelay);
        experimentController.TriggerTarget();
    }

    IEnumerator ClickCountdown()
    {
        yield return new WaitForSeconds(5.0f);
        experimentController.ClickTimeout();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(experimentController.GameState == "setup")
        {
            if(other.tag == "Cursor")
            {
                experimentController.GameState = "entering";
                experimentController.enterBegin = DateTime.Now;
                StartCoroutine(ClickCountdown());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        batAnimatorController.SetBool("BatHold", false);
        if (experimentController.GameState != "moving" && experimentController.GameState != "feedback")
        {
            if(other.tag == "Cursor")
            {
                Target.SetActive(false);
                clicked = false;
                experimentController.clickReminder.SetActive(false);
                experimentController.GameState = "setup";    
                StopAllCoroutines();
            }
        }
        else if(experimentController.GameState == "moving")
        {
            if(other.tag == "Cursor")
            {
                experimentController.moved = true;
                experimentController.initiationBegin = DateTime.Now;
                gameObject.SetActive(false);
            }
        }
    }
}
