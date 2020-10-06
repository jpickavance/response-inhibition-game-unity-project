using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoController : MonoBehaviour
{
    public GameObject InfoBox;
    public Text InfoText;
 
    public void ShowHitInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "The percentage of targets you hit at night and sunrise";
    }

    public void ShowFlightTimeInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "The average time you took to fly from the cave to the fruit at night and sunrise";
    }
    public void ShowComboInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "Your best streak of hitting the target at night and sunrise, and stopping at day";
    }
    public void ShowFalseStartInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "The number of times you flew out at day. If you performed the experiment as intended, this should be around 1/12 the total number of trials";
    }
    public void ShowProactiveInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "The average amount of time you delayed your response to sunrise trials compared to night trials";
    }
    public void ShowReactiveInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "The average time prior to movement you were able to inhibit movement in response to the change to day. Lower is better";
    }

    public void ShowGradeInfo()
    {
        InfoBox.SetActive(true);
        InfoText.text = "How well you followed instructions. Higher grades are awarded for missing fewer targets and moving at a flight time closer to 200ms";
    }

    public void HideInfo()
    {
        InfoBox.SetActive(false);
    }
}
