using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class List : MonoBehaviour
{
    public List<GameObject> objectList;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            getGameObjects();
        }
    }

    public void getGameObjects()
    {
        GameObject[] gObject = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject objs in gObject)
        {
            objectList.Add(objs);
        }
    }
}
