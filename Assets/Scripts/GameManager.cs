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
}
