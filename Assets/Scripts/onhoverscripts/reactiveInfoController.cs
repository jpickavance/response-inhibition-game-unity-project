using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reactiveInfoController : MonoBehaviour{
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowReactiveInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}