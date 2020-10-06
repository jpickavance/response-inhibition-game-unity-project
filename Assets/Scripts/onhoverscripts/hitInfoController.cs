using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
 
public class hitInfoController : MonoBehaviour {
    public InfoController infoController;
    public void OnMouseOver()
    {
        infoController.ShowHitInfo();
    }
    public void OnMouseExit()
    {
        infoController.HideInfo();
    }
   
}
