using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSceneSelector : MonoBehaviour
{
    public static NewSceneSelector instance;

    public string newSceneSelector;

    private void Awake()
    {
        instance = this;
    }
}
