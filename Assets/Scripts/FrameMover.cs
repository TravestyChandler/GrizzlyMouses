using UnityEngine;
using System.Collections;

public class FrameMover : MonoBehaviour {

    public Transform start, end;
    public int frameUpdateTime = 1;
    public int frameUpdateCount = 0;
	// Use this for initialization
	void Start () {
      frameUpdateTime = Random.Range(3, 5);
	}

    public void FramePlacement(FrameMover other)
    {
        Transform otherEnd = other.end;
        float dist = this.transform.position.x - this.start.position.x;
        this.transform.position = new Vector3(otherEnd.position.x + dist, 0);
    }

	// Update is called once per frame
	void Update () {
        if (transform.position.x < GameManager.instance.EndVector.x)
        {
            Despawn();
        }
        else
        {
            
        }
    }

    public void Despawn()
    {
        GameManager.instance.Despawn(this);
    }
}
