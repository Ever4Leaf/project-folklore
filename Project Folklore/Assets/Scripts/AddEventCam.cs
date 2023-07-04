using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEventCam : MonoBehaviour
{
    void LateUpdate()
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("PlayerCam").GetComponent<Camera>();
    }
}
