using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Camera PastCamera, PresentCamera, Player2UI;
	public static CameraController Instance;

	void Awake(){
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy (this);
			return;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnOnP2UI()
    {
        Player2UI.gameObject.SetActive(true);
    }


	public void SwapCameras(bool present){
		if (present) {
			PastCamera.enabled = false;
			PresentCamera.enabled = true;
			PresentCamera.rect = new Rect (0, 0, 1, 1);
		} else {
			PresentCamera.enabled = false;
			PastCamera.enabled = true;
			PastCamera.rect = new Rect (0, 0, 1, 1);
		}
	}
}
