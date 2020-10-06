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
    public GameObject Instructions1;
    public GameObject Instructions2;
    public GameObject splatLogo;
    public GameObject StartLine;
    public GameObject FinishLine;
    public PauseController pauseController;
    AudioSource audioSource;
    public AudioClip splatJingle;
    public Slider sensitivitySlider;
    public Text sensValue;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
