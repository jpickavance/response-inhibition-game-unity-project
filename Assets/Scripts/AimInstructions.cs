using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class AimInstructions : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void UpdateAimVars   (string tableName, 
                                                string tokenId,
                                                string order,
                                                bool zeroAimQuestions,
                                                float CertainTargetPos,
                                                float CertainTargetHitX,
                                                float CertainTargetHitY,
                                                float CertainBatHitX,
                                                float CertainBatHitY,
                                                float UncertainTargetPos,
                                                float UncertainTargetHitX,
                                                float UncertainTargetHitY,
                                                float UncertainBatHitX,
                                                float UncertainBatHitY,
                                                string ZeroOrder,
                                                float ZeroCertainTargetPos,
                                                float ZeroCertainTargetHitX,
                                                float ZeroCertainTargetHitY,
                                                float ZeroCertainBatHitX,
                                                float ZeroCertainBatHitY,
                                                float ZeroUncertainTargetPos,
                                                float ZeroUncertainTargetHitX,
                                                float ZeroUncertainTargetHitY,
                                                float ZeroUncertainBatHitX,
                                                float ZeroUncertainBatHitY);

    public bool debug;
    public bool webBuild;
    public bool secondRodeo;
    public string hand;
    public int Order;
    public int Block;
    public string CertaintyCondition;
    public Text targetPrimeText;
    public bool moveTarget;
    public bool dragTarget;
    public bool hitTarget;
    public bool hitBat;
    public Vector2 StartPos = new Vector2(-6.5f, 3.5f);
    public Vector2 EndPos = new Vector2(11.0f, 3.5f);
    public Vector2 centerPos = new Vector2(0f, 3.5f);
    public Vector2 caveStartPos = new Vector2(4.5f, -3.5f);
    public float speed;
    public float sensitivity;
    public Vector2 targetPos;

    public GameObject UFO;
    public GameObject Bat;
    public GameObject Cave;
    public Sprite caveLeft;
    public GameObject Mountains_F;
    public GameObject Mountains_B;
    public GameObject Sky;
    public Sprite DayMountains_F;
    public Sprite NightMountains_F;
    public Sprite DayMountains_B;
    public Sprite NightMountains_B;
    public Sprite DaySky;
    public Sprite NightSky;
    public Sprite leftDayMountains_F;
    public Sprite leftNightMountains_F;
    public Sprite leftDayMountains_B;
    public Sprite leftNightMountains_B;
    public Sprite leftNightSky;
    public Sprite leftDaySky;
    public GameObject Instructions;
    public GameObject targetDragInstructions;
    public Text targetDragInstructionsText;
    public GameObject targetDragSubmitButton;
    public GameObject targetHitInstructions;
    public Text targetHitInstructionsText;
    public GameObject targetHitSubmitButton;
    public GameObject batHitInstructions;
    public Text batHitInstructionsText;
    public GameObject batHitSubmitButton;
    public TargetDragger targetDragger;
    public BatClicker batClicker;

    public float CertainTargetPos;
    public float CertainTargetHitX;
    public float CertainTargetHitY;
    public float CertainBatHitX;
    public float CertainBatHitY;
    public float UncertainTargetPos;
    public float UncertainTargetHitX;
    public float UncertainTargetHitY;
    public float UncertainBatHitX;
    public float UncertainBatHitY;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        if(UserInfo.Instance.n_aimQuestions == 1)
        {
            secondRodeo = true;
        }
        if(UserInfo.Instance.GameMode == "debug")
        {
            debug = true;
        }
        else
        {
            debug = false;
        }
        if(!debug)
        {
            hand = UserInfo.Instance.handedness;
        }
        Block = 1;
        if(Order == 1)
        {
            CertaintyCondition = "Certain";
        }
        else if(Order == 2)
        {
            CertaintyCondition = "Uncertain";
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(hand == "left")
        {
            StartPos.x = -StartPos.x;
            EndPos.x = -EndPos.x;

            caveStartPos.x = -caveStartPos.x;
            Cave.GetComponent<SpriteRenderer>().sprite = caveLeft;

            Vector3 skyPos = Sky.transform.position;
            Vector3 fgMountPos = Mountains_F.transform.position;
            Vector3 bgMountPos = Mountains_B.transform.position;
            Sky.transform.position = new Vector3(-skyPos.x, skyPos.y, 0f);
            Mountains_F.transform.position = new Vector3(-fgMountPos.x, fgMountPos.y, 0f);
            Mountains_B.transform.position = new Vector3(-bgMountPos.x, bgMountPos.y, 0f);
        }
        
        Cave.transform.position = caveStartPos;
        UFO.transform.position = StartPos;

        if(CertaintyCondition == "Certain")
        {
            NightTime();
        }
        else
        {
            DayTime();
        }

        Instructions.SetActive(true);
        UFO.SetActive(false);
        Bat.SetActive(false);
        Cave.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveTarget)
        {
            float step = speed * Time.deltaTime;
            UFO.transform.position = Vector2.MoveTowards(UFO.transform.position, EndPos, step);
            if((hand == "right" && UFO.transform.position.x >= EndPos.x) ||(hand == "left" && (UFO.transform.position.x - .001f) <= EndPos.x))
            {
                EndTargetMove();
            }
        }
    }

    public void Ready()
    {
        Instructions.SetActive(false);
        StartCoroutine(TargetAppear());
        Cave.SetActive(true);
    }

    public void EndTargetMove()
    {
        moveTarget = false;
        StopAllCoroutines();
        PlaceTarget();
    }

    public void PlaceTarget()
    {
        StartCoroutine(TargetOnRails());
    }

    public void SubmitTargetPos()
    {
        targetPos = UFO.transform.position;
        Debug.Log(targetPos.x);
        dragTarget = false;
        targetDragInstructions.SetActive(false);
        targetDragSubmitButton.SetActive(false);
        if(CertaintyCondition == "Certain")
        {
            CertainTargetPos = targetPos.x;
        }
        else
        {
            UncertainTargetPos = targetPos.x;
        }
        StartTargetHit();
    }

    public void StartTargetHit()
    {
        hitTarget = true;
        targetHitInstructions.SetActive(true);
        UFO.transform.position = centerPos;
    }

    public void SubmitTargetHit()
    {
        Debug.Log(targetDragger.strikePos);
        if(CertaintyCondition == "Certain")
        {
            CertainTargetHitX = targetDragger.strikePos.x;
            CertainTargetHitY = targetDragger.strikePos.y - 3.5f;
        }
        else
        {
            UncertainTargetHitX = targetDragger.strikePos.x;
            UncertainTargetHitY = targetDragger.strikePos.y - 3.5f;
        }
        UFO.SetActive(false);
        UFO.transform.position = StartPos;
        hitTarget = false;
        targetHitInstructions.SetActive(false);
        targetHitSubmitButton.SetActive(false);
        targetDragger.Strike.SetActive(false);
        StartBatHit();
    }

    public void StartBatHit()
    {
        hitBat = true;
        batHitInstructions.SetActive(true);
        Bat.SetActive(true);
        Bat.transform.position = centerPos;
    }

    public void SubmitBatHit()
    {
        if(CertaintyCondition == "Certain")
        {
            CertainBatHitX = batClicker.strikePos.x;
            CertainBatHitY = batClicker.strikePos.y - 3.5f;
        }
        else
        {
            UncertainBatHitX = batClicker.strikePos.x;
            UncertainBatHitY = batClicker.strikePos.y - 3.5f;
        }

        Bat.SetActive(false);
        batClicker.Strike.SetActive(false);
        hitBat = false;
        batHitInstructions.SetActive(false);
        batHitSubmitButton.SetActive(false);
        if(Block > 1)
        {
            if(debug)
            {
                if(!UserInfo.Instance.zeroAimQuestions || secondRodeo)
                {
                    Debug.Log("Order = " + Order + 
                    ", Certain Target Pos:" + CertainTargetPos + ", Certain Target Hit: " + CertainTargetHitX + "," + CertainTargetHitY + 
                    ", Certain Bat Hit: " + CertainBatHitX + "," + CertainBatHitY +
                    " Uncertain Target Pos:" + UncertainTargetPos + ", Uncertain Target Hit: " + UncertainTargetHitX + "," + UncertainTargetHitY + 
                    ", Uncertain Bat Hit: " + UncertainBatHitX + "," + UncertainBatHitY);
                }
                else
                {
                    Debug.Log("ZeroOrder = " + Order + 
                    ", ZeroCertain Target Pos:" + CertainTargetPos + ", ZeroCertain Target Hit: " + CertainTargetHitX + "," + CertainTargetHitY + 
                    ", ZeroCertain Bat Hit: " + CertainBatHitX + "," + CertainBatHitY +
                    " ZeroUncertain Target Pos:" + UncertainTargetPos + ", ZeroUncertain Target Hit: " + UncertainTargetHitX + "," + UncertainTargetHitY + 
                    ", ZeroUncertain Bat Hit: " + UncertainBatHitX + "," + UncertainBatHitY);
                }
            }
            else
            {
                if(!UserInfo.Instance.zeroAimQuestions || secondRodeo)
                {
                    UpdateAimVars(UserInfo.Instance.UserTable,
                        UserInfo.Instance.tokenId,
                        Order.ToString(),
                        UserInfo.Instance.zeroAimQuestions,
                        CertainTargetPos,
                        CertainTargetHitX,
                        CertainTargetHitY,
                        CertainBatHitX,
                        CertainBatHitY,
                        UncertainTargetPos,
                        UncertainTargetHitX,
                        UncertainTargetHitY,
                        UncertainBatHitX,
                        UncertainBatHitY,
                        UserInfo.Instance.ZeroOrder,
                        UserInfo.Instance.ZeroCertainTargetPos,
                        UserInfo.Instance.ZeroCertainTargetHitX,
                        UserInfo.Instance.ZeroCertainTargetHitY,
                        UserInfo.Instance.ZeroCertainBatHitX,
                        UserInfo.Instance.ZeroCertainBatHitY,
                        UserInfo.Instance.ZeroUncertainTargetPos,
                        UserInfo.Instance.ZeroUncertainTargetHitX,
                        UserInfo.Instance.ZeroUncertainTargetHitY,
                        UserInfo.Instance.ZeroUncertainBatHitX,
                        UserInfo.Instance.ZeroUncertainBatHitY);
                }
                else
                {
                        UserInfo.Instance.ZeroOrder = Order.ToString();
                        UserInfo.Instance.ZeroCertainTargetPos = CertainTargetPos;
                        UserInfo.Instance.ZeroCertainTargetHitX = CertainTargetHitX;
                        UserInfo.Instance.ZeroCertainTargetHitY = CertainTargetHitY;
                        UserInfo.Instance.ZeroCertainBatHitX = CertainBatHitX;
                        UserInfo.Instance.ZeroCertainBatHitY = CertainBatHitY;
                        UserInfo.Instance.ZeroUncertainTargetPos = UncertainTargetPos;
                        UserInfo.Instance.ZeroUncertainTargetHitX = UncertainTargetHitX;
                        UserInfo.Instance.ZeroUncertainTargetHitY = UncertainTargetHitY;
                        UserInfo.Instance.ZeroUncertainBatHitX = UncertainBatHitX;
                        UserInfo.Instance.ZeroUncertainBatHitY = UncertainBatHitY;
                }
            }
            if(!UserInfo.Instance.zeroAimQuestions || secondRodeo)
            {
                SceneManager.LoadScene("EndScreen");
            }
            else if(UserInfo.Instance.zeroAimQuestions && !secondRodeo)
            {
                UserInfo.Instance.n_aimQuestions++;
                SceneManager.LoadScene("Experiment");
            }
        }
        else
        {
            Block += 1;
            if(CertaintyCondition == "Certain")
            {
                CertaintyCondition = "Uncertain";
                DayTime();
            }
            else
            {
                CertaintyCondition = "Certain";
                NightTime();
            }
            Instructions.SetActive(true);           
        }
    }

    public void DayTime()
    {
        targetPrimeText.text = "You will now see the <b>FRUIT</b> fly overhead at <b>SUNRISE</b>\n\nPay attention to where the <b>FRUIT</b> is when you would hit it";
        targetDragInstructionsText.text = "Drag the <b>FRUIT</b> to the point you tried to leave the cave at <b>SUNRISE</b>";
        targetHitInstructionsText.text = "Click the point of the <b>FRUIT</b> you tried to hit at <b>SUNRISE</b>";
        batHitInstructionsText.text = "Click the point of the <b>BAT</b> you were using to hit the <b>FRUIT</b> at <b>SUNRISE</b>";
        if(hand == "left")
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
        targetPrimeText.text = "You will now see the <b>FRUIT</b> fly overhead at <b>NIGHT</b>\n\nPay attention to where the <b>FRUIT</b> is when you would hit it";
        targetDragInstructionsText.text = "Drag the <b>FRUIT</b> to the point you tried to leave the cave at <b>NIGHT</b>";
        targetHitInstructionsText.text = "Click the point of the <b>FRUIT</b> you tried to hit at <b>NIGHT</b>";
        batHitInstructionsText.text = "Click the point of the <b>BAT</b> you were using to hit the <b>FRUIT</b> at <b>NIGHT</b>";
        if(hand == "left")
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

    IEnumerator TargetAppear()
    {
        yield return new WaitForSeconds(1.0f);
        UFO.SetActive(true);
        StartCoroutine(TargetMove());
    }

    IEnumerator TargetMove()
    {
        yield return new WaitForSeconds(0.5f);
        moveTarget = true;
    }

    IEnumerator TargetOnRails()
    {
        yield return new WaitForSeconds(1.0f);
        UFO.transform.position = StartPos;
        dragTarget = true;
        targetDragInstructions.SetActive(true);
    }

    
}
