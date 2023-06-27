using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleStateMachine>().Input2(EnemyPrefab);
    }

    public void ShowSelector()
    {
        EnemyPrefab.transform.Find("EnemySelector").gameObject.SetActive(true);
    }

    public void HideSelector()
    {
        EnemyPrefab.transform.Find("EnemySelector").gameObject.SetActive(false);
    }
}
