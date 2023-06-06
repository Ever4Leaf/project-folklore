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

    private void Start()
    {
		transform.position = GameManager.instance.nextPlayerPosition;
    }

    void FixedUpdate(){
		if(Input.GetKey(KeyCode.W)){
			playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.S)){
			playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
		}
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
