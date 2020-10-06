using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    public GameObject ConfettiPx;
    // Start is called before the first frame update

    void Start()
    {
        GameObject obj = Instantiate(ConfettiPx);
        Destroy(obj, 2.5f);
    }
}
