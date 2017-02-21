using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController Instance;

    public GameOverPanel gameOver;
    public Text countdownText;
    public Text playerText;
    public RectTransform deathMenu;
    public float deathPanelTimer = 0.25f;
	// Use this for initialization
	void Awake () {
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

    public void DeathPopUp()
    {
        gameOver.Open(deathPanelTimer);
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerText(string playerNum)
    {
        playerText.text = "Player " + playerNum;
    }
    
}
