using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatClicker : MonoBehaviour
{
    public GameObject Strike;
    public Vector2 strikePos;
    public AudioSource audioSource;
    public AudioClip batSound;
    public AimInstructions aimInstructions;
    public Texture2D crosshair;
    public Vector2 crosshairOffset;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        crosshairOffset = new Vector2(crosshair.width/2, crosshair.height/2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
                {   
                    strikePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Strike.SetActive(true);
                    Strike.transform.position = new Vector2 (strikePos.x, strikePos.y);
                    audioSource.PlayOneShot(batSound, 0.5f);
                    aimInstructions.batHitSubmitButton.SetActive(true);
                }
    }

    public void OnMouseEnter()
    {
        if(aimInstructions.hitBat)
        {
            if(aimInstructions.webBuild == false)
            {
                Cursor.SetCursor (crosshair, crosshairOffset, CursorMode.Auto);
            }
        }
    }

    public void OnMouseExit()
    {
        if(aimInstructions.webBuild == false)
        {
            Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
        }
    }
}
