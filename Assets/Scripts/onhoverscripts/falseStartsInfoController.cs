using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class falseStartsInfoController : MonoBehaviour{
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowFalseStartInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}