using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    public GameObject ConfettiPx;
    public ExperimentController experimentController;
    // Start is called before the first frame update

    void Start()
    {
        UserInfo.Instance.fanfareCount += 1;
        if(!(UserInfo.Instance.fanfareCount == 2 && UserInfo.Instance.zeroAimQuestions))
        {
            GameObject obj = Instantiate(ConfettiPx);
            Destroy(obj, 2.5f);
        }
    }
}
