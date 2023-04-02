using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControlManager : MonoBehaviour
{
    public Transform headSet;
    public float spawnDistance;

    public GameObject mainMenu;
    public InputActionProperty showButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (showButton.action.WasPressedThisFrame() || Input.GetKeyDown(KeyCode.Q))
        {
            mainMenu.SetActive(!mainMenu.activeSelf);

            mainMenu.transform.position = headSet.position + new Vector3(headSet.forward.x, 0, headSet.forward.z).normalized * spawnDistance;
        }

        mainMenu.transform.LookAt(new Vector3(headSet.position.x, mainMenu.transform.position.y, headSet.position.z));
        mainMenu.transform.forward *= -1; 
    }
}
