using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Vector2 StartVector, EndVector;
	public static GameManager instance;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
			return;
		}
		//Do things with the instance here;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
