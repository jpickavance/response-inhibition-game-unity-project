using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class OnCloseListener : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void UnloadListener();

    [DllImport("__Internal")]
    private static extern void SaveProgress (string tableName, 
                                            string tokenId,
                                            string gameProgress,
                                            int trial,
                                            int SSD,
                                            int score,
                                            bool windowClose);
    
    public ExperimentController experimentController;

    public void Start()
    {
        UnloadListener();
    }

    public void OnClose()
     {
        SaveProgress("JP_FBS_Pilot_TokenTable",
                     UserInfo.Instance.tokenId,
                     experimentController.GameProgress.ToString(),
                     experimentController.trial,
                     experimentController.SSD,
                     experimentController.score,
                     true);
     }
}

/* MUST INCLUDE THE BELOW IN TEMPLATE HTML SCRIPT LINE 14

 window.onbeforeunload = function(e) {
     gameInstance.SendMessage("OnCloseListener", "OnClose");
     var dialogText = "You game has been saved!  Would you like to continue unloading the page?";
     e.returnValue = dialogText;
     return dialogText;
 };
 */
 
