using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControlManager : MonoBehaviour
{
    [Header("VR Headset")]
    public GameObject headSetPrefab;
    public float spawnDistance;

    [Header("Canvas UI")]
    public GameObject playerStatus;
    public GameObject actionList;
    public GameObject skillList;
    public GameObject targetList;
    //public GameObject playerInventory;
    //public GameObject enemiesStatus;
    //public GameObject announcerCorner;

    [Header("VR Control Input")]
    public InputActionProperty showButton;

    void Update()
    {
        headSetPrefab = GameObject.FindGameObjectWithTag("VRCamera");
        playerStatusVRUI();
        actionListVRUI();

        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.E))
        {
            skillListVRUI();
        }

        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.Escape))
        {
            targetListVRUI();
        }

        //if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.I))
        //{
        //    playerInventoryUpdate();
        //}

        //enemiesStatusStaticUpdate();
        //announcerCornerUpdate();
    }

    void playerStatusVRUI()
    {
        playerStatus.transform.position = headSetPrefab.transform.position + new Vector3(1.39f, -0.35f, 0.7f) * spawnDistance;
    }

    
    void actionListVRUI()
    {
        //actionList.SetActive(!actionList.activeSelf);

        actionList.transform.position = headSetPrefab.transform.position + new Vector3(2.2f, -0.4f, -1.1f) * spawnDistance;
    }

    void skillListVRUI()
    {
        skillList.SetActive(!skillList.activeSelf);

        skillList.transform.position = headSetPrefab.transform.position + new Vector3(2.2f, -0.4f, -1.1f) * spawnDistance;
    }

    void targetListVRUI()
    {
        targetList.SetActive(!targetList.activeSelf);

        targetList.transform.position = headSetPrefab.transform.position + new Vector3(2.2f, -0.4f, -1.1f) * spawnDistance;

        targetList.transform.LookAt(new Vector3(headSetPrefab.transform.position.x, targetList.transform.position.y, headSetPrefab.transform.position.z));
        targetList.transform.forward *= -1;
    }

    //void playerInventoryUpdate()
    //{
    //    playerInventory.SetActive(!playerInventory.activeSelf);

    //    playerInventory.transform.position = headSetPrefab.transform.position + new Vector3(headSetPrefab.transform.forward.x, 0.4f, 0.2f) * spawnDistance;
    //}

    //void enemiesStatusStaticUpdate()
    //{
    //    enemiesStatus.transform.position = headSetPrefab.transform.position + new Vector3(2.115f, 1.223f, 2.529f);
    //}

    //void announcerCornerUpdate()
    //{
    //    announcerCorner.transform.position = headSetPrefab.transform.position + new Vector3(2.377f, 1.758f, 2.989f);
    //}
}
