using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CalibrationController : MonoBehaviour
{
    public float mouseSensitivity;
    public bool locked;
    public float sensitivityTransformer = 0.1f;
    public float maxY;
    public float minY;
    public Text Instructions1;
    public GameObject splatLogo;
    public GameObject StartLine;
    public GameObject FinishLine;
    public GameObject MousePose;
    public GameObject TrackpadPose;
    public PauseController pauseController;
    AudioSource audioSource;
    public AudioClip splatJingle;
    public Slider sensitivitySlider;
    public Text sensValue;
    public Sprite MousePose1;
    public Sprite MousePose2;
    public Sprite MouseLeft1;
    public Sprite MouseLeft2;
    public Sprite TrackpadRight1;
    public Sprite TrackpadRight2;
    public Sprite TrackpadLeft1;
    public Sprite TrackpadLeft2;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(UserInfo.Instance.pointer == "mouse")
        {
            Instructions1.text = "Begin with the bat between the <i>Start</i> lines\n\nMove the mouse upwards until the bat lands between the <i>Finish</i> lines\n\nUse the arrow keys to adjust the bat speed (<b>LEFT</b>: slower) until a single flick upward moves the bat from <i>Start</i> to <i>Finish</i>";
            TrackpadPose.SetActive(false);
            if(UserInfo.Instance.handedness == "left")
            {
                MousePose.GetComponent<SpriteRenderer>().sprite = MouseLeft1;
            }
            else
            {
                MousePose.GetComponent<SpriteRenderer>().sprite = MousePose1;
            }
            MousePose.SetActive(true);
        }
        else
        {
            Instructions1.text = "Begin with the bat between the <i>Start</i> lines. Stroke the trackpad until the bat lands between the <i>Finish</i> lines\n\nUse the arrow keys to adjust the bat speed (<b>LEFT</b>: slower) until a single, vertical stroke the length of the trackpad moves the bat from <i>Start</i> to <i>Finish</i>";
            MousePose.SetActive(false);
            if(UserInfo.Instance.handedness == "left")
            {
                TrackpadPose.GetComponent<SpriteRenderer>().sprite = TrackpadLeft1;
            }
            else
            {
                TrackpadPose.GetComponent<SpriteRenderer>().sprite = TrackpadRight1;
            }
            TrackpadPose.SetActive(true);
        }
        sensitivitySlider.Select();
        if(UserInfo.Instance.GameMode == "debug")
        {
            sensitivityTransformer = sensitivityTransformer*5;
            UserInfo.Instance.fullscreen = true;
            UserInfo.Instance.locked = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Handling movement of bat
        //get mouse gain and transform it into distance moved based on sensitivity
        float yMove = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * sensitivityTransformer * Time.deltaTime;
        //use this to translate cursor position
        transform.Translate(0f, yMove, 0f);
        //ensure bat doesn't leave screen
        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;

        if(UserInfo.Instance.locked && UserInfo.Instance.fullscreen)
        {
            if(!pauseController.oneTime)
            {
                sensValue.text = mouseSensitivity.ToString(); 
                sensitivitySlider.Select();
                if(transform.position.y < -2)
                {
                    if(UserInfo.Instance.pointer == "mouse")
                    {
                        if(UserInfo.Instance.handedness == "left")
                            {
                                MousePose.GetComponent<SpriteRenderer>().sprite = MouseLeft1;
                            }
                            else
                            {
                                MousePose.GetComponent<SpriteRenderer>().sprite = MousePose1;
                            }
                    }
                    else
                    {
                        if(UserInfo.Instance.handedness == "left")
                        {
                            TrackpadPose.GetComponent<SpriteRenderer>().sprite = TrackpadLeft1;
                        }
                        else
                        {
                            TrackpadPose.GetComponent<SpriteRenderer>().sprite = TrackpadRight1;
                        }
                    }
                }
                else if(transform.position.y > 2)
                {
                    if(UserInfo.Instance.pointer == "mouse")
                    {
                        if(UserInfo.Instance.handedness == "left")
                            {
                                MousePose.GetComponent<SpriteRenderer>().sprite = MouseLeft2;
                            }
                            else
                            {
                                MousePose.GetComponent<SpriteRenderer>().sprite = MousePose2;
                            }
                    }
                    else
                    {
                        if(UserInfo.Instance.handedness == "left")
                        {
                            TrackpadPose.GetComponent<SpriteRenderer>().sprite = TrackpadLeft2;
                        }
                        else
                        {
                            TrackpadPose.GetComponent<SpriteRenderer>().sprite = TrackpadRight2;
                        }
                    }
                }
            }
            else
            {
                sensValue.text = "";
            }
        }

        if(Input.GetKeyDown("space") && !pauseController.oneTime)
        {
            pauseController.oneTime = true;
            UserInfo.Instance.mouseSensitivity = mouseSensitivity;
            CompleteCalibration();
        }
    }

    public void OnSliderChange()
    {
        mouseSensitivity = sensitivitySlider.value;
    }

    public void CompleteCalibration()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        StartLine.GetComponent<SpriteRenderer>().enabled = false;
        FinishLine.GetComponent<SpriteRenderer>().enabled = false;
        MousePose.GetComponent<SpriteRenderer>().enabled = false;
        TrackpadPose.GetComponent<SpriteRenderer>().enabled = false;
        splatLogo.SetActive(true);
        audioSource.PlayOneShot(splatJingle, 2.5f);
        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Experiment");
    }
}
