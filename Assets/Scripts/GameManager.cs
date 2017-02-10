using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public Vector2 StartVector, EndVector;
    public float CurrentSpeed = 1f;
	public static GameManager instance;
    public List<GameObject> FramePrefabs;
    public List<FrameMover> frames;
    public int startFrames = 3;
	public GamePhase phase = GamePhase.Starting;

	public enum GamePhase{
		Starting,
		Running,
		GameOver,
		Invalid
	}
	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (this);
			return;
		}
        //Do things with the instance here;
        StartGame();

    }

    public void StartGame()
    {
        for(int i =0;  i < startFrames; i++)
        {
            Spawn();
        }
    }


	// Update is called once per frame
	void Update () {
		if (phase == GamePhase.Starting) {
			CurrentSpeed = 0f;
		} else if (phase == GamePhase.Running) {

		}
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
