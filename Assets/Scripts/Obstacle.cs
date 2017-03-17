using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
	public GameObject particlePrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[PunRPC]
	public void DestroyObstacle(){
		foreach(Collider2D col in this.GetComponents<Collider2D>()){
			col.enabled = false;
		}
		GameObject game = GameObject.Instantiate (particlePrefab, this.transform.position, Quaternion.identity);
		Destroy (game,3f);
		Destroy (this.gameObject, 3f);
	}
}
