using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("VR Control")]
    public InputActionProperty L_TriggerButton;
    public InputActionProperty R_TriggerButton;

    [Header("Params")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    //[SerializeField] private GameObject speakerBackground;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    //[SerializeField] private Animator portraitAnimator;

    [Header("Objective UI")]
    [SerializeField] private GameObject objectivePanel;

    private Animator layoutAnimator;
    public Animator loadingAnimator;

    private InkExternalFunctions inkExternalFunctions;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    private bool canContinueToNextLine = false;
    private bool canSkip = false;
    private bool submitSkip;

    private Coroutine displayLineCoroutine;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    //private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene!");
        }

        instance = this;

        inkExternalFunctions = new InkExternalFunctions();
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start() 
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        //get layout animator
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
    }

    private void Update() 
    {
        if(Input.GetMouseButtonDown(0) || R_TriggerButton.action.WasPressedThisFrame() || L_TriggerButton.action.WasPressedThisFrame())
        {
            submitSkip = true;
        }

       //return if dialogue not playing
       if(!dialogueIsPlaying)
       {
            return;
       }

       if(canContinueToNextLine && Input.GetMouseButtonDown(0) || R_TriggerButton.action.WasPressedThisFrame() || L_TriggerButton.action.WasPressedThisFrame())
       {
            ContinueStory();
       }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        //objectivePanel.SetActive(false);

        //inkExternalFunctions.Bind(currentStory);
        
        ExternalFunctionLoading();
        ExternalFunctionMoveScene();
        ExternalFunctionMoveNextScene();
        

        ContinueStory();
    }

    // private void ExitDialogueMode()
    // {
        
    // }

    private void ContinueStory()
    {
        if(currentStory.canContinue)
        {
            //set text for the current dialogue line
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            //handle case where last line is external function
            if(nextLine.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            //otherwise handle normal case for continue story
            else
            {
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
                //handle tags
                HandleTags(currentStory.currentTags);
            }
            
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void ExternalFunctionLoading()
    {
        currentStory.BindExternalFunction("playLoading", () =>
        {
            
            if(loadingAnimator != null)
            {
                StartCoroutine(Loading());
            }
            else
            {
                Debug.LogWarning("Loading animator not found");
            }

            Debug.Log("External function works");
        });
    }

    private void ExternalFunctionMoveScene()
    {
        currentStory.BindExternalFunction("moveBattleScene", (string sceneToLoad) =>
        {
            GameManager.instance.isStaticEncounter = true;

            //SceneManager.LoadScene(sceneToLoad);

            Debug.Log("External function works");
        });
    }

    private void ExternalFunctionMoveNextScene()
    {
        currentStory.BindExternalFunction("moveScene", (string sceneToLoad) =>
        {
            //GameManager.instance.isStaticEncounter = true;

            SceneManager.LoadScene(sceneToLoad);

            Debug.Log("External function works");
        });
    }

    private IEnumerator DisplayLine(string line)
    {
        //empty the dialogue text
        dialogueText.text = "";
        canContinueToNextLine = false;
        submitSkip = false;

        StartCoroutine(CanSkip());

        //display each letter one at a time
        foreach(char letter in line.ToCharArray())
        {
            //if the submit button it pressed, finish up displaying line right away
            if(canSkip && submitSkip)
            {
                submitSkip = false;
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        canContinueToNextLine = true;
        canSkip = false;
    }

    private IEnumerator CanSkip()
    {
        canSkip = false;
        yield return new WaitForSeconds(0.05f);
        canSkip = true;
    }

    private IEnumerator Loading()
    {
        loadingAnimator.SetTrigger("Start");
                
        yield return new WaitForSeconds(3f);

        loadingAnimator.SetTrigger("End");
    }

    private IEnumerator MoveScene()
    {
        yield return new WaitForSeconds(3f);
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        //objectivePanel.SetActive(true);
        dialogueText.text = "";

        currentStory.UnbindExternalFunction("playLoading");
    }

    private void HandleTags(List<string> currentTags)
    {
        //loop through each tag and handle it accordingly
        foreach(string tag in currentTags)
        {
            //parse the tag
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag
            switch(tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                //case PORTRAIT_TAG:
                //    portraitAnimator.Play(tagValue);
                //    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
}
