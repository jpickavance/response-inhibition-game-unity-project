using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public ExperimentController experimentController;
    public GameObject Mountains_F;
    public GameObject Mountains_B;
    public GameObject Sky;
    public Sprite DayMountains_F;
    public Sprite NightMountains_F;
    public Sprite StopMountains_F;
    public Sprite DayMountains_B;
    public Sprite NightMountains_B;
    public Sprite StopMountains_B;
    public Sprite DaySky;
    public Sprite NightSky;
    public Sprite StopSky;
    public Sprite leftDayMountains_F;
    public Sprite leftNightMountains_F;
    public Sprite leftStopMountains_F;
    public Sprite leftDayMountains_B;
    public Sprite leftNightMountains_B;
    public Sprite leftStopMountains_B;
    public Sprite leftNightSky;
    public Sprite leftDaySky;
 
    void Start()
    {
        if(experimentController.handedness == "left")
        {
            //ensure background elements are in the correct position relative to default
            Vector3 skyPos = Sky.transform.position;
            Vector3 fgMountPos = Mountains_F.transform.position;
            Vector3 bgMountPos = Mountains_B.transform.position;
            Sky.transform.position = new Vector3(-skyPos.x, skyPos.y, 0f);
            Mountains_F.transform.position = new Vector3(-fgMountPos.x, fgMountPos.y, 0f);
            Mountains_B.transform.position = new Vector3(-bgMountPos.x, bgMountPos.y, 0f);
            //override default assets with mirrored assets
            Mountains_F.GetComponent<SpriteRenderer>().sprite = leftNightMountains_F;
            Mountains_B.GetComponent<SpriteRenderer>().sprite = leftNightMountains_B;
            Sky.GetComponent<SpriteRenderer>().sprite = leftNightSky;
        }
    }

    public void DayTime()
    {
        if(experimentController.handedness == "left")
        {
        Sky.GetComponent<SpriteRenderer>().sprite = leftDaySky;
        Mountains_F.GetComponent<SpriteRenderer>().sprite = leftDayMountains_F;
        Mountains_B.GetComponent<SpriteRenderer>().sprite = leftDayMountains_B;
        }
        else
        {
        Sky.GetComponent<SpriteRenderer>().sprite = DaySky;
        Mountains_F.GetComponent<SpriteRenderer>().sprite = DayMountains_F;
        Mountains_B.GetComponent<SpriteRenderer>().sprite = DayMountains_B;
        }
    }
    public void NightTime()
    {
        if(experimentController.handedness == "left")
        {
            Sky.GetComponent<SpriteRenderer>().sprite = leftNightSky;
            Mountains_F.GetComponent<SpriteRenderer>().sprite = leftNightMountains_F;
            Mountains_B.GetComponent<SpriteRenderer>().sprite = leftNightMountains_B;
        }
        else
        {
            Sky.GetComponent<SpriteRenderer>().sprite = NightSky;
            Mountains_F.GetComponent<SpriteRenderer>().sprite = NightMountains_F;
            Mountains_B.GetComponent<SpriteRenderer>().sprite = NightMountains_B;
        }
    }

    public void StopTime()
    {
        if(experimentController.handedness == "left")
        {
            Sky.GetComponent<SpriteRenderer>().sprite = StopSky;
            Mountains_F.GetComponent<SpriteRenderer>().sprite = leftStopMountains_F;
            Mountains_B.GetComponent<SpriteRenderer>().sprite = leftStopMountains_B;
        }
        else
        {
            Sky.GetComponent<SpriteRenderer>().sprite = StopSky;
            Mountains_F.GetComponent<SpriteRenderer>().sprite = StopMountains_F;
            Mountains_B.GetComponent<SpriteRenderer>().sprite = StopMountains_B;
        }

    }
}
