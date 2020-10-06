using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterceptController : MonoBehaviour
{
    public ExperimentController experimentController;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(experimentController.GameState == "moving")
        {
            if(other.tag == "Cursor")
            {
                gameObject.SetActive(false);
                experimentController.hit = false;
                experimentController.GiveFeedback();
            }   
        }
    }
}
