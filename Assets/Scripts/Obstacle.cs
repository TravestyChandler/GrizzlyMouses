using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	public GameObject particlePrefab;
	public PhotonView phot;
	// Use this for initialization
	void Awake () {
		phot = this.GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool destroying = false;

	[PunRPC]
	public void SetID(int val){
		phot.viewID = val;
	}

	[PunRPC]
	public void DestroyObstacle(){
		foreach(Collider2D col in this.GetComponents<Collider2D>()){
			col.enabled = false;
		}
		GameObject game = GameObject.Instantiate (particlePrefab, this.transform.position, Quaternion.identity);
		GameManager.instance.UnAllocateViewIDAfterTime (phot.viewID, 0.1f);
		Destroy (this.gameObject, 0.1f);
		Destroy (game,3f);

	}
}
