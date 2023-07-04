using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEncounterCollider : MonoBehaviour
{
    private void Update()
    {
        GameManager.instance.isStaticEncounter = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.isStaticEncounter = true;
            Destroy(this.gameObject);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.isStaticEncounter = false;
        }
    }
}
