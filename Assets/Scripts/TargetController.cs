using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public Vector2 StartPos = new Vector2(-6.5f, 3.5f);
    public Vector2 EndPos = new Vector2(11.0f, 3.5f);
    public float speed;
    public ExperimentController experimentController;
    public GameObject Intercept;
    void Start()
    {
        if(experimentController.handedness == "left")
        {
            StartPos.x = -StartPos.x;
            EndPos.x = -EndPos.x;
        }
        transform.position = StartPos;
    }
    void Update()
    {
        if(experimentController.GameState == "moving")
        {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, EndPos, step);
        }
        if((experimentController.handedness == "right" && transform.position.x >= EndPos.x) ||(experimentController.handedness == "left" && (transform.position.x - .001f) <= EndPos.x))
        {
            EndTrialProcesses();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Cursor")
        {
            experimentController.hit = true;
            EndTrialProcesses();
        }
    }

    void EndTrialProcesses()
    {
        Intercept.SetActive(false);
        gameObject.SetActive(false);
        experimentController.GiveFeedback();
    }
}
