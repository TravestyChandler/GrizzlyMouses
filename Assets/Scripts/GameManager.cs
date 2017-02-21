using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public Vector2 StartVector, EndVector;
    public float CurrentSpeed = 1f;
    public float startSpeed, maxSpeed;
    public float speedIncreasePerSec;
    public static GameManager instance;
    public List<GameObject> FramePrefabs;
    public List<FrameMover> frames;
    public int startFrames = 3;
    public GamePhase phase = GamePhase.Starting;
    public bool networkedGame = false;
    public GameObject NetworkObjects;
    
    public enum GamePhase {
        Starting,
        Running,
        GameOver,
        Invalid
    }
    // Use this for initialization
    void Start() {
        phase = GamePhase.Starting;
        if (instance == null) {
            instance = this;
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
    public void RPCStartGame()
    {
        for (int i = 0; i < startFrames; i++)
        {
            Spawn();
        }
        StartCoroutine(StartGameRoutine());

    }

    public void StartGame()
    {
        for (int i = 0; i < startFrames; i++)
        {
            Spawn();
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

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update () {
		if (phase == GamePhase.Starting) {
			CurrentSpeed = 0f;
		} else if (phase == GamePhase.Running) {
            if(CurrentSpeed > maxSpeed)
            {
                CurrentSpeed -= (speedIncreasePerSec * Time.deltaTime);
            }
            else if(CurrentSpeed < maxSpeed)
            {
                CurrentSpeed = maxSpeed;
            }
		}
        if(phase == GamePhase.GameOver)
        {
            CurrentSpeed = 0f;
        }
	}

    public void PlayerDeath()
    {
        phase = GamePhase.GameOver;
        UIController.Instance.DeathPopUp();
    }

    public void Despawn(FrameMover frame)
    {
        Spawn();
        frames.Remove(frame);
        Destroy(frame.gameObject);
    }

    public void Spawn()
    {
        int rand = Random.Range(0, FramePrefabs.Count);
        GameObject frameObj = Instantiate(FramePrefabs[rand], Vector3.one * 100f, Quaternion.identity);
        FrameMover frame = frameObj.GetComponent<FrameMover>();
        frames.Add(frame);
        frame.FramePlacement(frames[frames.Count - 2]);
    }

	public void PlayerImpact(){
		//reduce game speed and delay before player can be hit again(maybe handle the second part in the player controller

	}

	public void TimeTravelPlayer(){

	}
}
