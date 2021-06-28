using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDragger : MonoBehaviour
{
    public Vector3 targetPos;
    public Vector3 screenPos;

    public AimInstructions aimInstructions;
    public bool drag;
    public float sensitivity;
    public Texture2D grabHand;
    public Texture2D crosshair;
    public Vector2 crosshairOffset;
    public GameObject Strike;
    public Vector3 strikePos;
    public AudioClip splatSound;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        screenPos = Camera.main.WorldToScreenPoint(new Vector3(0f, 3.5f, 0f));
        crosshairOffset = new Vector2(crosshair.width/2, crosshair.height/2);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if(aimInstructions.dragTarget)
        {
            if(aimInstructions.webBuild == false)
            {
                Cursor.SetCursor (grabHand, Vector2.zero, CursorMode.Auto);
            }
            if(Input.GetMouseButton(0))
            {
                drag = true;
                StartCoroutine(Follow());
            }
            else
            {
                drag = false;
                StopCoroutine(Follow());
            }
        }
        if(aimInstructions.hitTarget)
        {
            if(Input.GetMouseButtonDown(0))
            {
                strikePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Strike.SetActive(true);
                Strike.transform.position = new Vector2 (strikePos.x, strikePos.y);
                audioSource.PlayOneShot(splatSound, 0.5f);
                aimInstructions.targetHitSubmitButton.SetActive(true);
            }
        }
    }

    public void OnMouseEnter()
    {
        if(aimInstructions.webBuild == false)
        {
            if(aimInstructions.hitTarget)
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
        StopAllCoroutines();
        if(aimInstructions.dragTarget)
        {
            aimInstructions.targetDragSubmitButton.SetActive(true);
        }
    }

    IEnumerator Follow()
    {
        while(drag)
        {
            targetPos = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, screenPos.y, 123.8f));
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPos, sensitivity);
            yield return new WaitForSeconds (0);
        }
    }
}
