using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControlManager : MonoBehaviour
{
    [Header("VR Headset")]
    public GameObject headSetPrefab;
    public float spawnDistance = 1f;

    [Header("UIs")]
    public GameObject playerStatus;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject gameSetting;
    public GameObject playerInventory;
    public GameObject enemiesStatus;
    public GameObject announcerCorner;

    [Header("VR Control Input")]
    public InputActionProperty showButton;

    // Update is called once per frame
    void Update()
    {
        headSetPrefab = GameObject.FindGameObjectWithTag("Player1");

        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.Q))
        {
            leftHandUpdate();
        }

        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.E))
        {
            rightHandUpdate();
        }

        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.Escape))
        {
            gameSettingUpdate();
        }

        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.I))
        {
            playerInventoryUpdate();
        }

        headSetPrefab.transform.position = headSetPrefab.transform.position;
        Debug.Log(headSetPrefab.transform.position);

        playerStatusUpdate();
        enemiesStatusStaticUpdate();
        announcerCornerUpdate();
    }

    void leftHandUpdate()
    {
        leftHand.SetActive(!leftHand.activeSelf);

        leftHand.transform.position = headSetPrefab.transform.position + new Vector3(-0.77f, 0.415f, 1.98f) * spawnDistance;
    }

    void rightHandUpdate()
    {
        rightHand.SetActive(!rightHand.activeSelf);

        rightHand.transform.position = headSetPrefab.transform.position + new Vector3(0.77f, 0.415f, 1.98f) * spawnDistance;
    }

    void gameSettingUpdate()
    {
        gameSetting.SetActive(!gameSetting.activeSelf);

        gameSetting.transform.position = headSetPrefab.transform.position + new Vector3(headSetPrefab.transform.forward.x, 0.5f, headSetPrefab.transform.forward.z).normalized * spawnDistance;

        gameSetting.transform.LookAt(new Vector3(headSetPrefab.transform.position.x, gameSetting.transform.position.y, headSetPrefab.transform.position.z));
        gameSetting.transform.forward *= -1;
    }

    void playerInventoryUpdate()
    {
        playerInventory.SetActive(!playerInventory.activeSelf);

        playerInventory.transform.position = headSetPrefab.transform.position + new Vector3(headSetPrefab.transform.forward.x, 0.4f, 0.2f) * spawnDistance;
    }

    void playerStatusUpdate()
    {
        playerStatus.transform.position = headSetPrefab.transform.position + new Vector3(-2.63f, 1.95f, 3.31f);
    }

    void enemiesStatusStaticUpdate()
    {
        enemiesStatus.transform.position = headSetPrefab.transform.position + new Vector3(2.115f, 1.223f, 2.529f);
    }

    void announcerCornerUpdate()
    {
        announcerCorner.transform.position = headSetPrefab.transform.position + new Vector3(2.377f, 1.758f, 2.989f);
    }
}
