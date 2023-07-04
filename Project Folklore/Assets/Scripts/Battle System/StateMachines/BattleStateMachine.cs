using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleStateMachine : MonoBehaviour
{
    public enum BattleStates
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
        CHECKALIVE,
        WIN,
        LOSE
    }
    public BattleStates curr_battleState;

    [Header("Manageable Game Objects")]
    public List<TurnHandler> performList = new List<TurnHandler>();
    public List<GameObject> playerInBattle = new List<GameObject>();
    public List<GameObject> enemyInBattle = new List<GameObject>();

    public enum PlayerGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }
    public PlayerGUI playerInput;
    public List<GameObject> playerManageable = new List<GameObject>();
    private TurnHandler playerActionSelect;

    [Header("Player UI: Panel")]
    public GameObject actionPanel;
    public GameObject skillPanel;
    public GameObject enemySelectPanel;
    public GameObject inventoryPanel;

    [Header("Player UI: Button")]
    public GameObject actionButton;
    public GameObject targetButton;
    public GameObject skillButton;
    
    [Header("Player UI: Spacer")]
    public Transform actionSpacer;
    public Transform skillSpacer;
    public Transform targetEnemySpacer;

    //List of buttons
    [Header("Button List")]
    public List<GameObject> actButtons = new List<GameObject>();
    public List<GameObject> enemySelectButtons = new List<GameObject>();

    [Header("Spawn Points")]
    public List<Transform> spawnPoints = new List<Transform>();
    public List<Transform> playerSP = new List<Transform>();

    private void Awake()
    {
        //instantiate player
        for (int i = 0; i < GameManager.instance.playerParty.Count; i++)
        {
            GameObject newPlayer = Instantiate(GameManager.instance.playerParty[i], playerSP[i].position, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
            newPlayer.name = newPlayer.GetComponent<PlayerStateMachine>().player.unitName;
            newPlayer.GetComponent<PlayerStateMachine>().player.unitName = newPlayer.name;
            playerInBattle.Add(newPlayer);
        }

        //instatntiate enemy
        for (int i = 0; i < GameManager.instance.enemyAmount; i++)
        {
            GameObject newEnemy = Instantiate(GameManager.instance.enemyToBattle[i], spawnPoints[i].position, Quaternion.Euler(0f, -90f, 0f)) as GameObject;
            newEnemy.name = newEnemy.GetComponent<EnemyStateMachine>().enemy.unitName + "_" + (i + 1);
            newEnemy.GetComponent<EnemyStateMachine>().enemy.unitName = newEnemy.name;
            enemyInBattle.Add(newEnemy);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        curr_battleState = BattleStates.WAIT;
        //find go then populate to respective list
        //enemyInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        //playerInBattle.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        playerInput = PlayerGUI.ACTIVATE;

        //deactivate panel
        actionPanel.SetActive(false);
        skillPanel.SetActive(false);
        enemySelectPanel.SetActive(false);

        //create button
        CreateEnemyButtons();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curr_battleState)
        {
            case (BattleStates.WAIT):
                    if (performList.Count > 0)
                    {
                        curr_battleState = BattleStates.TAKEACTION;
                    }
                break;

            case (BattleStates.TAKEACTION):
                    GameObject actionPerformer = GameObject.Find(performList[0].attackerName);

                    if (performList[0].attackerType == "Enemy")
                    {
                        EnemyStateMachine enemyStateMachine = actionPerformer.GetComponent<EnemyStateMachine>();
                        for(int i = 0; i < playerInBattle.Count; i++)
                        {
                            if(performList[0].attackTarget == playerInBattle[i])
                            {
                                enemyStateMachine.playerTarget = performList[0].attackTarget;
                                enemyStateMachine.currentState = EnemyStateMachine.TurnState.ACTION;
                                break;
                            }
                            else
                            {
                                performList[0].attackTarget = playerInBattle[Random.Range(0, playerInBattle.Count)];
                                enemyStateMachine.playerTarget = performList[0].attackTarget;
                                enemyStateMachine.currentState = EnemyStateMachine.TurnState.ACTION;
                            }
                        }
                    }

                    if (performList[0].attackerType == "Player")
                    {
                        PlayerStateMachine playerStateMachine = actionPerformer.GetComponent<PlayerStateMachine>();

                        playerStateMachine.enemyTarget = performList[0].attackTarget;
                        playerStateMachine.currentState = PlayerStateMachine.TurnState.ACTION;
                    }

                    curr_battleState = BattleStates.PERFORMACTION;
                break;

            case (BattleStates.PERFORMACTION):
                //idle state
                break;

            case (BattleStates.CHECKALIVE):
                    if(playerInBattle.Count < 1)
                    {
                        //battle lose
                        curr_battleState = BattleStates.LOSE;
                    }
                    else if(enemyInBattle.Count < 1)
                    {
                        //battle win
                        curr_battleState = BattleStates.WIN;
                    }
                    else
                    {
                        //call function
                        ClearActionPanel();
                        playerInput = PlayerGUI.ACTIVATE; ;
                    }

                break;

            case (BattleStates.WIN):
                //update player stats
                WinUpdatePlayerStats();

                Debug.Log("WIN");
                for (int i = 0; i < playerInBattle.Count; i++)
                {
                    playerInBattle[i].GetComponent<PlayerStateMachine>().currentState = PlayerStateMachine.TurnState.WAITING;
                }

                GameManager.instance.LoadSceneAfterBattle();
                GameManager.instance.curr_GameState = GameManager.GammeStates.WORLD_STATE;
                GameManager.instance.enemyToBattle.Clear();
                break;

            case (BattleStates.LOSE):
                //update player stats
                LoseUpdatePlayerStats();

                Debug.Log("LOSE");
                for (int i = 0; i < enemyInBattle.Count; i++)
                {
                    enemyInBattle[i].GetComponent<EnemyStateMachine>().currentState = EnemyStateMachine.TurnState.WAITING;
                }

                GameManager.instance.LoadSceneAfterBattle();
                GameManager.instance.curr_GameState = GameManager.GammeStates.WORLD_STATE;
                GameManager.instance.enemyToBattle.Clear();
                break;
        }

        switch (playerInput)
        {
            case (PlayerGUI.ACTIVATE):
                    if (playerManageable.Count > 0)
                    {
                        playerManageable[0].transform.Find("PlayerSelector").gameObject.SetActive(true);
                        //create new handleturn instance
                        playerActionSelect = new TurnHandler();  

                        actionPanel.SetActive(true);
                        //create action button
                        CreateAttackButton();

                        playerInput = PlayerGUI.WAITING;
                    }
                break;

            case (PlayerGUI.WAITING):
                //idle state
                break;

            case (PlayerGUI.DONE):
                    PlayerInputDone();
                break;
        }
    }

    public void GetActionInfoFrom(TurnHandler actionInfo)
    {
        performList.Add(actionInfo);
    }

    public void CreateEnemyButtons()
    {
        //clean
        foreach (GameObject targetBtn in enemySelectButtons)
        {
            Destroy(targetBtn);
        }
        enemySelectButtons.Clear();

        //create button
        foreach (GameObject enemy in enemyInBattle)
        {
            GameObject newButton = Instantiate(targetButton) as GameObject;
            EnemySelectButton enemySelectButton = newButton.GetComponent<EnemySelectButton>();

            EnemyStateMachine curr_enemy = enemy.GetComponent<EnemyStateMachine>();

            TextMeshProUGUI buttonText = newButton.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
            buttonText.text = curr_enemy.enemy.unitName;

            enemySelectButton.EnemyPrefab = enemy;

            newButton.transform.SetParent(targetEnemySpacer, false);

            enemySelectButtons.Add(newButton);
        }       
    }

    public void Input1()//Action Button
    {
        playerActionSelect.attackerName = playerManageable[0].name;
        playerActionSelect.attackerGO = playerManageable[0];
        playerActionSelect.attackerType = "Player";

        playerActionSelect.usedAttack = playerManageable[0].GetComponent<PlayerStateMachine>().player.attackList[0];
        playerManageable[0].GetComponent<PlayerStateMachine>().player.currentAP += playerManageable[0].GetComponent<PlayerStateMachine>().player.attackList[0].movesetRecoverValue;

        actionPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    public void Input2(GameObject tergetEnemy)//Enemy Selection
    {
        playerActionSelect.attackTarget = tergetEnemy;
        playerInput = PlayerGUI.DONE;
    }

    public void Input3(MovesetBase selectedSkill)//selected skill attack
    {
        playerActionSelect.attackerName = playerManageable[0].name;
        playerActionSelect.attackerGO = playerManageable[0];
        playerActionSelect.attackerType = "Player";

        playerActionSelect.usedAttack = selectedSkill;
        playerManageable[0].GetComponent<PlayerStateMachine>().player.currentAP -= selectedSkill.movesetCost;

        skillPanel.SetActive(false);
        enemySelectPanel.SetActive(true);
    }

    public void Input4(ItemBase item)//use item
    {

    }

    public void SwitchToSkillPanel()//switch to skill attack select
    {
        actionPanel.SetActive(false);
        skillPanel.SetActive(true);
    }

    public void SwitchToInventoryPanel()//switch to item select
    {
        actionPanel.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void SwitchToActionPanel()//switch back to action select
    {
        actionPanel.SetActive(true);
        inventoryPanel.SetActive(false);
    }

    public void PlayerInputDone()
    {
        performList.Add(playerActionSelect);

        //clean the action panel
        ClearActionPanel();

        playerManageable[0].transform.Find("PlayerSelector").gameObject.SetActive(false);
        playerManageable.RemoveAt(0);

        playerInput = PlayerGUI.ACTIVATE;
    }

    public void ClearActionPanel()
    {
        actionPanel.SetActive(false);
        skillPanel.SetActive(false);
        enemySelectPanel.SetActive(false);

        foreach (GameObject atkBtn in actButtons)
        {
            Destroy(atkBtn);
        }
        actButtons.Clear();
    }

    //create action button
    void CreateAttackButton()
    {
        //attack button
        GameObject AttackButton = Instantiate(actionButton) as GameObject;
        TextMeshProUGUI AttackButtonText = AttackButton.transform.Find("ActionText").gameObject.GetComponent<TextMeshProUGUI>();
        AttackButtonText.text = "Attack";
        AttackButton.GetComponent<Button>().onClick.AddListener(() => Input1());
        AttackButton.transform.SetParent(actionSpacer, false);
        actButtons.Add(AttackButton);

        //skill button
        GameObject SkillButton = Instantiate(actionButton) as GameObject;
        TextMeshProUGUI SkillButtonText = SkillButton.transform.Find("ActionText").gameObject.GetComponent<TextMeshProUGUI>();
        SkillButtonText.text = "Skill";
        SkillButton.GetComponent<Button>().onClick.AddListener(() => SwitchToSkillPanel());
        SkillButton.transform.SetParent(actionSpacer, false);
        actButtons.Add(SkillButton);

        //item button
        GameObject ItemButton = Instantiate(actionButton) as GameObject;
        TextMeshProUGUI ItemButtonText = ItemButton.transform.Find("ActionText").gameObject.GetComponent<TextMeshProUGUI>();
        ItemButtonText.text = "Item";
        ItemButton.GetComponent<Button>().onClick.AddListener(() => SwitchToInventoryPanel());
        ItemButton.transform.SetParent(actionSpacer, false);
        actButtons.Add(ItemButton);

        if (playerManageable[0].GetComponent<PlayerStateMachine>().player.skillList.Count > 0)
        {
            foreach (MovesetBase skillAtk in playerManageable[0].GetComponent<PlayerStateMachine>().player.skillList)
            {
                if (skillAtk.movesetCost < playerManageable[0].GetComponent<PlayerStateMachine>().player.currentAP)
                {
                    GameObject skillAtkButton = Instantiate(skillButton) as GameObject;
                    TextMeshProUGUI skillAtkText = skillAtkButton.transform.Find("SkillText").gameObject.GetComponent<TextMeshProUGUI>();
                    skillAtkText.text = skillAtk.movesetName;

                    SkillAttackButton skillBtn = skillAtkButton.GetComponent<SkillAttackButton>();
                    skillBtn.skillToPerform = skillAtk;
                    skillAtkButton.transform.SetParent(skillSpacer, false);
                    actButtons.Add(skillAtkButton);
                }
                else
                {
                    SkillButton.GetComponent<Button>().interactable = false;
                }
            }
        }
        else
        {
            //SkillButton.GetComponent<Button>().interactable = false;
        }

        if (InventoryManager.instance.itemList.Count > 0)
        {
            //ItemButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            ItemButton.GetComponent<Button>().interactable = false;
        }
    }

    public void WinUpdatePlayerStats()
    {
        foreach (GameObject playerChara in GameManager.instance.playerParty)
        {
            for (int i = 0; i < playerInBattle.Count; i++)
            {
                //update hp
                GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentHP = playerInBattle[i].GetComponent<PlayerStateMachine>().player.currentHP;

                //get drop exp
                GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentEXP += 10f;
                //check can lvl up
                if (GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentEXP >= GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.maxEXP)
                {
                    //lvl up
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.unitLevel += 1;
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.maxEXP += 20;
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.maxHP += 20;
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.attackStat += 1f;
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.defendStat += 0.5f;
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.speedStat += 0.1f;

                    //reset currentEXP
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentEXP = 0f;

                    //set currentHP to maxHP
                    GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentHP = GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.maxHP;
                }
            }
        }
    }

    public void LoseUpdatePlayerStats()
    {
        foreach (GameObject playerChara in GameManager.instance.playerParty)
        {
            for (int i = 0; i < playerInBattle.Count; i++)
            {
                GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentHP = playerInBattle[i].GetComponent<PlayerStateMachine>().player.currentHP;

                GameManager.instance.playerParty[i].GetComponent<PlayerStateMachine>().player.currentEXP -= 10f;
            }
        }
    }
}
