using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public Text countdownText;
    public static UIController Instance;
	// Use this for initialization
	void Start () {
		if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
