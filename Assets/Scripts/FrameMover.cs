using UnityEngine;
using System.Collections;

public class FrameMover : MonoBehaviour {

    public Transform start, end;
    public int frameUpdateTime = 1;
    public int frameUpdateCount = 0;
	public Collectible[] collectibles;
	// Use this for initialization
	void Start () {
      frameUpdateTime = Random.Range(30, 75);
		foreach (Collectible c in collectibles) {
			int val = PhotonNetwork.AllocateViewID ();
			c.phot.RPC ("SetID", PhotonTargets.All, val);
		}
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
