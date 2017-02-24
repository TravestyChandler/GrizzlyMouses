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
    public PhotonView photView;
    public float speedReduction = -0.5f;
    public string[] frameNames;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("Nothing implementated in OnPhotonSerialization");
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
                    //photView.RPC("RPCSpawn", PhotonTargets.All, val);
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
        if (phase == GamePhase.Starting)
        {
            CurrentSpeed = 0f;
        }
        else if (phase == GamePhase.Running)
        {
            if (CurrentSpeed > maxSpeed)
            {
                CurrentSpeed -= (speedIncreasePerSec * Time.deltaTime);
            }
            else if (CurrentSpeed < maxSpeed)
            {
                CurrentSpeed = maxSpeed;
            }
        }
        if (phase == GamePhase.GameOver)
        {
            CurrentSpeed = 0f;
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
        Destroy(frame.gameObject);
    }

    [PunRPC]
    public void RPCSpawn(int value, int photonID)
    {

        
        Debug.Log("Spawning frames: " + value);
        GameObject frameObj = Instantiate(FramePrefabs[value], Vector3.one * 100f, Quaternion.identity);
        PhotonView nviews = frameObj.GetComponent<PhotonView>();
        nviews.viewID = photonID;
        FrameMover frame = frameObj.GetComponent<FrameMover>();
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


	public void PlayerImpact(){
		//reduce game speed and delay before player can be hit again(maybe handle the second part in the player controller

	}

	public void TimeTravelPlayer(){

	}
}
