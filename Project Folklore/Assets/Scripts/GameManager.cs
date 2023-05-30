using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerPrefab;

    private void Awake()
    {
        //check if instance exist
        if (instance == null)
        {
            //if not then set instance to this
            instance = this;
        }
        //if there is but not this instance
        else if (instance != this)
        {
            //then destroy it
            Destroy(gameObject);
        }
        //set this instance to be not destroyable
        DontDestroyOnLoad(gameObject);
    }
}
