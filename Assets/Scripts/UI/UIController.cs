﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController Instance;

    public GameOverPanel gameOver;
    public RoomList room;
    public Text countdownText;
    public Text playerText;
    public Text distTraveled;
    public RectTransform deathMenu;
	public Button ReadyUpButton;
    public float deathPanelTimer = 0.25f;
    public float roomListPanelTimer = 0.25f;
    public float distanceTraveled = 0f;
	// Use this for initialization
	void Awake () {
		if(Instance == null)
        {
            Instance = this;
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

    public void RoomListPopUp()
    {
        room.Open(roomListPanelTimer);
    }

    public void CloseRoomList()
    {
        room.Close(roomListPanelTimer);
    }
    // Update is called once per frame
    void Update () {
        distanceTraveled += (GameManager.instance.CurrentSpeed * Time.deltaTime);
        distTraveled.text = (Mathf.Abs((int)distanceTraveled)).ToString() + " meters";
	}

    public void PauseGame()
    {
        GameManager.instance.photView.RPC("PauseGame", PhotonTargets.All);
    }
    public void SetPlayerText(string playerNum)
    {
        playerText.text = "Player " + playerNum;
    }

	public void ShowReadyButton(){
		ReadyUpButton.enabled = true;
		//GameManager.instance.photView.RPC ("ReadyUp", PhotonTargets.All);
	}

	public void CloseReadyButton(){
		ReadyUpButton.enabled = false;
	}
    
}
