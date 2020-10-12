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
        OpenWindow("https://twitter.com/ICON_UoL");
    }
}
