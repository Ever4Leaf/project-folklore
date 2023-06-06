using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSceneChange : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnterTown")
        {
            CollisionHandler colHandler = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextPlayerPosition = colHandler.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = colHandler.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "LeaveTown")
        {
            CollisionHandler colHandler = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextPlayerPosition = colHandler.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = colHandler.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
    }
}
