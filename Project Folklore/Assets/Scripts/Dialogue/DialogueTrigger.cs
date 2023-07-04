using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("VR Control")]
    public InputActionProperty L_TriggerButton;
    public InputActionProperty R_TriggerButton;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;

    private void Awake() 
    {
        playerInRange = false;
        //visualCue.SetActive(false);
    }

    private void Update() 
    {
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            //visualCue.SetActive(true);
            if(Input.GetKeyDown(KeyCode.I) || R_TriggerButton.action.WasPressedThisFrame() || L_TriggerButton.action.WasPressedThisFrame())
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
        else
        {
            //visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }


    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
