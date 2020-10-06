using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comboInfoController : MonoBehaviour{
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowComboInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}