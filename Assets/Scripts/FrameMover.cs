using UnityEngine;
using System.Collections;

public class FrameMover : MonoBehaviour {

    public Transform start, end;
	// Use this for initialization
	void Start () {
      
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
            this.transform.Translate(new Vector2(GameManager.instance.CurrentSpeed, 0f) * Time.deltaTime);
        }
	}

    public void Despawn()
    {
        GameManager.instance.Despawn(this);
    }
}
