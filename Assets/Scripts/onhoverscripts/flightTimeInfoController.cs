using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flightTimeInfoController : MonoBehaviour{
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowFlightTimeInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}