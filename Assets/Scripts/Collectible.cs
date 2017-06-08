using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
	public GameObject particleEffect;
	public float timeToDestroy = 0.25f;
	public float particleDestroyTime = 0.5f;
	public bool canCollect = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals ("Player") && canCollect) {
			canCollect = false;
			Collected ();
		}
	}

	public void Collected(){
		Destroy (this, timeToDestroy);
		Destroy (Instantiate (particleEffect, this.transform), particleDestroyTime);
	}
}
