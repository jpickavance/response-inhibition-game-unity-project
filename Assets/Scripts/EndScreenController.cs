using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class EndScreenController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenWindow(string link);
    public GameObject Leaderboard;
    public GameObject EndScreenContainer;
    public CameraController cameraController;

    public void Start()
    {
        cameraController.ScaleFullScreenCamera();
    }
    public void SetLeaderboardActive()
    {
        EndScreenContainer.SetActive(false);
        Leaderboard.SetActive(true);
    }
    public void ExitLeaderboard()
    {
        Leaderboard.SetActive(false);
        EndScreenContainer.SetActive(true);
    }

    public void GoTwitter()
    {
        string link = "https://twitter.com/icon_uol";
        OpenWindow(link);
    }

    public void GoFeedback()
    {
        OpenWindow("https://forms.gle/RqsEDWzDzMCWV1Vi6");
    }

    public void GoFinish()
    {
        OpenWindow("https://app.prolific.co/submissions/complete?cc=DDFBF3F8");
    }
}
