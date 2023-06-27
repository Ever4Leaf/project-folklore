using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_FollowCamera : MonoBehaviour
{
    [Header("VR Headset")]
    public GameObject headCam;
    public float spawnDistance;

    [Header("Canvas UI")]
    //public GameObject playerStatus;
    public GameObject menu;

    //[Header("VR Control Input")]
    //public InputActionProperty showButton;

    void Update()
    {
        headCam = GameObject.FindGameObjectWithTag("VRCamera");

        menu.transform.position = headCam.transform.position + new Vector3(headCam.transform.forward.x, 0f, headCam.transform.forward.z).normalized * spawnDistance;
        menu.transform.LookAt(new Vector3(headCam.transform.position.x, menu.transform.position.y, headCam.transform.position.z));
        menu.transform.forward *= -1;
    }



    //void playerStatusVRUI()
    //{
    //    playerStatus.transform.position = headSetPrefab.transform.position + new Vector3(1.39f, -0.35f, 0.7f) * spawnDistance;
    //}

}
