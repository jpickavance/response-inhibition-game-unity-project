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
        string link = "https://twitter.com/intent/tweet?text=How%20well%20can%20you%20control%20your%20impulses%3F%0AMy%20inihibition%20response%20time%20is%20" + (UserInfo.Instance.SSD - 580).ToString() + "%20and%20I%20got%20a%20top%20score%20of%20" + UserInfo.Instance.score.ToString() + "!%0Apic.twitter.com/5YxdEqZkb3&hashtags=FruitbatSplat";
       // https://twitter.com/intent/tweet?text=How%20well%20can%20you%20control%20your%20impulses%3F%0AMy%20inhibition%20response%20time%20is%20100%20ms%20and%20I%20got%20a%20top%20score%20of%209999!%0Apic.twitter.com/5YxdEqZkb3&hashtags=FruitbatSplat
        OpenWindow(link);
    }

    public void GoFeedback()
    {
        OpenWindow("https://forms.gle/RqsEDWzDzMCWV1Vi6");
    }
}
