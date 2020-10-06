using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proactiveInfoController : MonoBehaviour{
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowProactiveInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}