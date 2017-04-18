using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	public GameObject particlePrefab;
	public PhotonView phot;
	// Use this for initialization
	void Start () {
		phot = this.GetComponent<PhotonView> ();
		phot.viewID = PhotonNetwork.AllocateViewID ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool destroying = false;

	public void OnCollisionEnter2D(Collision2D col){
		if (!destroying) {
			if (col.gameObject.tag.Equals ("Player")) {
				phot.RPC ("DestroyObstacle", PhotonTargets.All, null);
				destroying = true;
			}
		}
	}
	[PunRPC]
	public void DestroyObstacle(){
		foreach(Collider2D col in this.GetComponents<Collider2D>()){
			col.enabled = false;
		}
		GameObject game = GameObject.Instantiate (particlePrefab, this.transform.position, Quaternion.identity);
		Destroy (this.gameObject, 0.1f);
		Destroy (game,3f);

	}
}
