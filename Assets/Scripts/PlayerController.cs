using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public string JumpButton;
	public float groundedDist = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.phase == GameManager.GamePhase.Running && IsGrounded()) {	
			Jump ();
		}
	}

	public void Jump(){

	}

	public bool IsGrounded(){
		Ray ray = new Ray (transform.position, Vector2.down);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, groundedDist) ){
			return true;
		} else {
			return false;
		}
	}
}
