using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {
	public GameObject particleEffect;
	public float timeToDestroy = 0.25f;
	public float particleDestroyTime = 0.5f;
	public bool canCollect = true;
	public string coinSoundEffect;
	private bool viewIdSet = false;
	public PhotonView phot;
	// Use this for initialization
	void Start () {
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
		if (!viewIdSet) {
			phot.viewID = PhotonNetwork.AllocateViewID ();
			viewIdSet = true;
		} else {
			return;
		}
	}

	[PunRPC]
	public void Collected(){
		canCollect = false;
		SoundManager.Instance.PlaySFX (coinSoundEffect, 100);
		Destroy (this.gameObject, timeToDestroy);
		Destroy (Instantiate (particleEffect, this.transform), particleDestroyTime);
	}
}
