using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator playerAnim;
	public Rigidbody playerRigid;
	public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
	public bool walking;
	public Transform playerTrans;

	Vector3 currentPosition, lastPosition;

    private void Start()
    {
		if (GameManager.instance.nextSpawnPoint != "")
        {
			GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint);
			transform.position = spawnPoint.transform.position;

			GameManager.instance.nextSpawnPoint = "";
		}
		else if (GameManager.instance.lastPlayerPosition != Vector3.zero)
        {
			transform.position = GameManager.instance.lastPlayerPosition;
			GameManager.instance.lastPlayerPosition = Vector3.zero;
        }
	}

    void FixedUpdate(){
		if(Input.GetKey(KeyCode.W)){
			playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S)){
			playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
		}

		currentPosition = transform.position;
		if (currentPosition == lastPosition)
        {
			GameManager.instance.isWalking = false;
        }
        else
        {
			GameManager.instance.isWalking = true;
        }
		lastPosition = currentPosition;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.W)){
			playerAnim.SetTrigger("Walking");
			playerAnim.ResetTrigger("Idle");
			walking = true;
			//steps1.SetActive(true);
		}
		if(Input.GetKeyUp(KeyCode.W)){
			playerAnim.ResetTrigger("Walking");
			playerAnim.SetTrigger("Idle");
            playerRigid.velocity = new Vector3(0f,0f,0f);
			walking = false;
			//steps1.SetActive(false);
		}
		if(Input.GetKeyDown(KeyCode.S)){
			playerAnim.SetTrigger("Walking");
			playerAnim.ResetTrigger("Idle");
			//steps1.SetActive(true);
		}
		if(Input.GetKeyUp(KeyCode.S)){
			playerAnim.ResetTrigger("Walking");
			playerAnim.SetTrigger("Idle");
			//steps1.SetActive(false);
		}
		if(Input.GetKey(KeyCode.A)){
			playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
		}
		if(Input.GetKey(KeyCode.D)){
			playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
		}
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "TeleportScene")
        {
			CollisionHandler colHandler = other.gameObject.GetComponent<CollisionHandler>();
			GameManager.instance.nextSpawnPoint = colHandler.spawnPointName;
			GameManager.instance.sceneToLoad = colHandler.sceneToLoad;
			GameManager.instance.LoadNextScene();
		}

		if (other.tag == "EncounterZone")
        {
			RegionData region = other.gameObject.GetComponent<RegionData>();
			GameManager.instance.curr_region = region;
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "EncounterZone")
        {
			GameManager.instance.canGetEncounter = true;
        }
	}

    private void OnTriggerExit(Collider other)
    {
		if (other.tag == "EncounterZone")
		{
			GameManager.instance.canGetEncounter = false;
		}
	}
}
