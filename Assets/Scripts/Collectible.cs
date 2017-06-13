using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
	public GameObject particleEffect;
	public float timeToDestroy = 0.25f;
	public float particleDestroyTime = 0.5f;
	public bool canCollect = true;
	public string coinSoundEffect;
	public PhotonView phot;
	// Use this for initialization
	void Awake () {
		phot = this.GetComponent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D col){
		if (col.tag.Equals ("Player") && canCollect) {
			
		}
	}
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

	}

	[PunRPC]
	public void SetID(int val){
        if (val == 0)
        {
            Debug.Log("Not sure why this is being set to 0");
        }
        else {
            phot.viewID = val;
        }
	}

	[PunRPC]
	public void Collected(){
		canCollect = false;
		SoundManager.Instance.PlaySFX (coinSoundEffect, 100);
		GameObject game = Instantiate (particleEffect);
		GameManager.instance.UnAllocateViewIDAfterTime (phot.viewID, timeToDestroy);
		Destroy (this.gameObject, timeToDestroy);
		Destroy (game, particleDestroyTime);
	}
}
