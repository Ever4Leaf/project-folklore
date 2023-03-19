using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class battleUIBehav : MonoBehaviour
{
    [Header("Panel List")]
    public GameObject leftTriggerPanel;
    public GameObject rightTriggerPanel;
    public GameObject pausePanel;
    public GameObject settingPanel;
    public GameObject playerStatusPanel;
    public GameObject inventoryPanel;          //for accesing inventory, player status, movesets, etc.

    //Start is called before the first frame update
    void Start() 
    {
        leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(false);

        Time.timeScale = 1;
    }

    //Update is called once per frame
    void Update() 
    {
        leftTriggerInputReceive();  //left trigger input receive
        rightTriggerInputReceive();  //right trigger input receive

        Time.timeScale = 1;
    }

    public void standBy() //stand by screen
    {
        leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void leftTriggerOpen() //activate leftTrigger panel
    { 
        leftTriggerPanel.SetActive(true);
        //rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void rightTriggerOpen() //activate rightTrigger panel
    { 
        //leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(true);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void pausePanelOpen() //activate pause panel
    { 
        leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(true);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void settingPanelOpen() //activate setting panel
    { 
        leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(true);
        playerStatusPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    /*void playerStatusPanelOpen() //activate player status panel
    { 
        leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(true);
        masterPanel.SetActive(false);
    }*/

    public void inventoryPanelOpen() //activate inventory panel
    { 
        leftTriggerPanel.SetActive(false);
        rightTriggerPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        playerStatusPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    void leftTriggerInputReceive()
    {
        if (Input.GetKey(KeyCode.Q)) //left trigger active
        {
            leftTriggerOpen();
            if (Input.GetKeyDown(KeyCode.J))
            {
                pausePanelOpen();
                Input.ResetInputAxes();
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                inventoryPanelOpen();
                Input.ResetInputAxes();
            }

        }
        else if (Input.GetKeyUp(KeyCode.Q))
        { 
            standBy(); 
        }
    }

    void rightTriggerInputReceive()
    {
        if (Input.GetKey(KeyCode.E)) //right trigger active
        {
            rightTriggerOpen();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            standBy();
        }
    }

    //Change Move
}
