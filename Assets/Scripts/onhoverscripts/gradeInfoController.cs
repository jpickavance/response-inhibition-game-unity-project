using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gradeInfoController : MonoBehaviour{
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowGradeInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}