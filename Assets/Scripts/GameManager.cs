﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Vector2 StartVector, EndVector;
    public Vector2 BackStartVector, backEndVector;
    public float CurrentSpeed = 1f;
    public float SuperSpeed = 25f;
    public float startSpeed, maxSpeed;
    public float minSpeed = 5f;
    public float speedIncreasePerSec;
    public static GameManager instance;
    public List<GameObject> FramePrefabs;
    public List<FrameMover> frames;
    public int startFrames = 3;
    public GamePhase phase = GamePhase.Starting;
    public bool networkedGame = false;
    public GameObject NetworkObjects;
    public PhotonView photView;
    public float speedReduction = -0.5f;
    public float previousSpeed = 0f;
    public float minPlayerX = -15f;
    public float xPositionIncrease = 1f;
    public float xPositionLoss = 5f;
    public float maxPlayerX = -6f;
	public bool player1Ready = false;
	public bool player2Ready = false;
    public bool inRoom = false;
	public Vector3 NickStart;
    public List<BackgroundFrame> backgrounds;
    public List<MidBackgroundFrame> midBackgrounds;
    public GameObject backgroundPrefab;
    public GameObject midBackgroundPrefab;
    public float powerUpTimer = 5f;
	public int resourcesCollected = 0;

    public enum GamePhase {
        Starting,
        Running,
        GameOver,
        Paused,
        Invalid
    }

    

    // Use this for initialization
    void Start() {
		NickStart = GameObject.Find ("Player").transform.position;
        phase = GamePhase.Starting;
        if (instance == null) {
            instance = this;
            photView = PhotonView.Get(this);
        } else {
            Destroy(this);
            return;
        }
        //Do things with the instance here;
        if (!networkedGame)
        {
            NetworkObjects.SetActive(false);
            StartGame();
        }

    }

	[PunRPC]
	public void IncreaseResources(){
		resourcesCollected++;
		UIController.Instance.ResourcesText.text = "x" + resourcesCollected.ToString ();
	}

    public void PowerUpTimer()
    {
        StartCoroutine(powerTimer());
    }

    public IEnumerator powerTimer()
    {
        Debug.Log("powerup timer running");
        float timer = 0f;
        while(timer < powerUpTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        PlayerController.Instance.photonView.RPC("TurnOffPowerUps", PhotonTargets.All);
    }

    [PunRPC]
    public void UsePowerUp(int type)
    {
        PlayerController.Instance.UsePowerUp(type);
        PowerUpTimer();
    }

    [PunRPC]
    public void ShiftPlayerBack()
    {
        Vector3 pos = PlayerController.Instance.transform.position;
        pos.x -= xPositionLoss;
        PlayerController.Instance.transform.position = pos;
        if (PhotonNetwork.isMasterClient)
        {
            if (PlayerController.Instance.transform.position.x < minPlayerX)
            {
                PlayerDeath();
            }
        }
    }

    [PunRPC]
    public void ReduceSpeed()
    {
        CurrentSpeed -= speedReduction;
        if (PhotonNetwork.isMasterClient)
        {
            if (CurrentSpeed > minSpeed)
            {
                PlayerDeath();
            }
        }
    }

	public void PlayerReady(){
		if (PhotonNetwork.isMasterClient) {
			player1Ready = true;
		} else {
			player2Ready = true;
		}
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

	}

	[PunRPC]
	public void ReadyUp(){
		UIController.Instance.ShowReadyButton ();
	}

    [PunRPC]
    public void RPCStartGame()
    {
        if (networkedGame)
        {
            if (PhotonNetwork.isMasterClient)
            {
                for (int i = 0; i < startFrames; i++)
                {
                    int val = UnityEngine.Random.Range(0, FramePrefabs.Count);
                    int id = PhotonNetwork.AllocateViewID();
                    photView.RPC("RPCSpawn", PhotonTargets.All, val, id);
                }
            }
            else
            {

            }
        }
        else if (!networkedGame)
        {
            for (int i = 0; i < startFrames; i++)
            {
                Spawn();
            }
        }
        StartCoroutine(StartGameRoutine());

    }

    public void SlowCharacter()
    {
        CurrentSpeed -= speedReduction;
    }


    public void StartGame()
    {
        if (networkedGame)
        {
            if (PhotonNetwork.isMasterClient)
            {
                for (int i = 0; i < startFrames; i++)
                {
                    int val = UnityEngine.Random.Range(0, FramePrefabs.Count);
                    int id = PhotonNetwork.AllocateViewID();
                    photView.RPC("RPCSpawn", PhotonTargets.All, val, id);
                }
            }
            else
            {

            }
        }
        else if(!networkedGame)
        {
            for (int i = 0; i < startFrames; i++)
            {
                Spawn();
            }
        }
        StartCoroutine(StartGameRoutine());

    }

    public IEnumerator StartGameRoutine()
    {
        UIController.Instance.countdownText.gameObject.SetActive(true);
        float timer = 3f;
        while (timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime;
            UIController.Instance.countdownText.text = ((int)timer).ToString();
        }
        yield return null;
        phase = GamePhase.Running;
        CurrentSpeed = startSpeed;
        UIController.Instance.countdownText.gameObject.SetActive(false);

    }

	[PunRPC]
	public void RPCRestartGame(int firstID)
    {
        //Delete all frames, place new start frame, reset score, start countdown over
        for (int i = 0; i < frames.Count; ++i)
        {
            GameObject game = frames[i].gameObject;
            frames[i] = null;
			UnAllocateViewIDAfterTime( game.GetComponent<PhotonView> ().viewID, 0.1f);
            Destroy(game);
        }
		frames.Clear ();
		UIController.Instance.gameOver.Close (UIController.Instance.deathPanelTimer);
        GameObject startFrame = Instantiate(FramePrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity);
		frames.Add(startFrame.GetComponent<FrameMover>());
		UIController.Instance.distanceTraveled = 0f;
		ResetNick ();
		if (PhotonNetwork.isMasterClient) {
			photView.RPC ("RPCStartGame", PhotonTargets.All);
		}
	}


	public void UnAllocateViewIDAfterTime(int viewID, float time){
		StartCoroutine(UnallocatedID(viewID, time));
	}

	public IEnumerator UnallocatedID (int viewID, float time){
		yield return new WaitForSeconds (time);
		yield return null;
		PhotonNetwork.UnAllocateViewID (viewID);
	}
    public void RestartGame()
    {
        //Delete all frames, place new start frame, reset score, start countdown over
        for(int i = 0; i < frames.Count; ++i)
        {
            GameObject game = frames[i].gameObject;
            frames[i] = null;
            Destroy(game);
        }
        GameObject startFrame = Instantiate(FramePrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity);
        frames[0] = startFrame.GetComponent<FrameMover>();
		UIController.Instance.ResourcesText.text = "x0";
        StartCoroutine(StartGameRoutine());
    }

    public void ResetNick()
    {
		GameObject Nick = GameObject.Find ("Player");
        PlayerController player = Nick.GetComponent<PlayerController>();
        if (player.inPresent)
        {
            player.TimeTravel();
        }
        Nick.transform.position = NickStart;
		PlayerController np = Nick.GetComponent<PlayerController> ();

        np.rb.gravityScale = np.gravityScale;
    }
    public void ToMainMenu()
    {
		PhotonNetwork.LeaveRoom ();
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update () {
		if (player1Ready && player2Ready && phase == GamePhase.Starting) {
			
		} 
        if(phase == GamePhase.Paused)
        {
            CurrentSpeed = 0f;
            return;
        }
        if (phase == GamePhase.Starting)
        {
            CurrentSpeed = 0f;
        }
        else if (phase == GamePhase.Running)
        {
            if (this.transform.position.x < maxPlayerX)
            {
                Vector3 pos = this.transform.position;
                pos.x += xPositionIncrease;
                this.transform.position = pos;
            }
            else if (this.transform.position.x > maxPlayerX)
            {
                Vector3 pos = this.transform.position;
                pos.x = maxPlayerX;
                this.transform.position = pos;
            }
        }
        if (phase == GamePhase.GameOver)
        {
            CurrentSpeed = 0f;
        }
        foreach(FrameMover fr in frames)
        {
            if (fr != null)
            {
                fr.transform.Translate(new Vector2(CurrentSpeed, 0f) * Time.deltaTime);
            }
        }
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                if (frames[i] != null)
                {
                    float newX = frames[i].transform.position.x;
                    if (frames[i].frameUpdateCount >= frames[i].frameUpdateTime)
                    {
                        frames[i].frameUpdateCount = 0;
                        photView.RPC("SetFrameX", PhotonTargets.Others, newX, i);
                    }
                    else
                    {
                        frames[i].frameUpdateCount++;
                    }
                }
            }
        }
    }

    [PunRPC]
    public void PlayerDeath()
    {
        phase = GamePhase.GameOver;
        UIController.Instance.DeathPopUp();
    }

    public void Despawn(FrameMover frame)
    {
        if (PhotonNetwork.isMasterClient)
        {
            int val = UnityEngine.Random.Range(0, FramePrefabs.Count);
            int id = PhotonNetwork.AllocateViewID();
            photView.RPC("RPCSpawn", PhotonTargets.All, val, id);
        }
        frames.Remove(frame);
		UnAllocateViewIDAfterTime( frame.GetComponent<PhotonView> ().viewID, 0.1f);
        Destroy(frame.gameObject);
    }


    [PunRPC]
    public void PauseGame()
    {
        if (phase == GamePhase.Paused)
        {
			UIController.Instance.TriggerPauseMenu (false);
            phase = GamePhase.Running;
			CurrentSpeed = maxSpeed;
        }
        else if (phase == GamePhase.Running)
        {
            phase = GamePhase.Paused;
			UIController.Instance.TriggerPauseMenu (true);

        }
        else
        {
            Debug.Log("Can't pause while not playing game");
        }
    }
	[PunRPC]
	public void SetFrameX(float x, int frame){
        if (frame > frames.Count)
        {
            return;   
        }
        else if(frames[frame] == null)
        {
            return;
        }
		Vector3 pos = frames [frame].transform.position;
		pos.x = x;
		frames [frame].transform.position = pos;
	}

    [PunRPC]
    public void RPCSpawn(int value, int photonID)
    {

        
        Debug.Log("Spawning frames: " + value);
       	GameObject frameObj = Instantiate(FramePrefabs[value], Vector3.one * 100f, Quaternion.identity);
        PhotonView nviews = frameObj.GetComponent<PhotonView>();
        nviews.viewID = photonID;
        FrameMover frame = frameObj.GetComponent<FrameMover>();
        frame.StartCoroutine(frame.SetIDRoutine());
        frames.Add(frame);
        frame.FramePlacement(frames[frames.Count - 2]);
    }

    public void Spawn()
    {
        Debug.Log("Non-networked spawn");
        int rand = Random.Range(0, FramePrefabs.Count);
        GameObject frameObj = Instantiate(FramePrefabs[rand], Vector3.one * 100f, Quaternion.identity);
        FrameMover frame = frameObj.GetComponent<FrameMover>();
        frames.Add(frame);
        frame.FramePlacement(frames[frames.Count - 2]);
    }


}
