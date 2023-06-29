using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public GameObject panelInventory;

    void Start()
    {
        panelInventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            panelInventory.SetActive(true);

            GameManager.instance.curr_GameState = GameManager.GammeStates.MENU_STATE;
        }
    }
}
