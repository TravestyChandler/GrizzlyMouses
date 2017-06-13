using UnityEngine;
using System.Collections;

public class FrameMover : MonoBehaviour {

    public Transform start, end;
    public int frameUpdateTime = 1;
    public int frameUpdateCount = 0;
	public Collectible[] collectibles;
	public Obstacle[] obstacles;
    public bool viewIdSet = false;
	// Use this for initialization
	void Start () {
      frameUpdateTime = Random.Range(30, 75);
        
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
        if (GameManager.instance.inRoom && !viewIdSet && PhotonNetwork.isMasterClient)
        {
            //StartCoroutine(SetIDRoutine());   
        }
    }


    public IEnumerator SetIDRoutine()
    {
        viewIdSet = true;
        yield return null;
        for (int i = 0; i < collectibles.Length; i++)
        {
            int val = PhotonNetwork.AllocateViewID();
            if (val == 0)
            {
                Debug.LogError("AllocateViewID came back with 0, illegal ID for: " + collectibles[i].gameObject.name);
            }
            this.GetComponent<PhotonView>().RPC("SetCollectibleID", PhotonTargets.AllBuffered, val, i);

        }
        for (int i = 0; i < obstacles.Length; i++)
        {
            int val = PhotonNetwork.AllocateViewID();
            if (val == 0)
            {
                Debug.LogError("AllocateViewID came back with 0, illegal ID for: " + obstacles[i].gameObject.name);
            }
            this.GetComponent<PhotonView>().RPC("SetObstacleID", PhotonTargets.AllBuffered, val, i);
        }
    }
    [PunRPC]
    public void SetObstacleID(int val, int arrayVal)
    {
        obstacles[arrayVal].phot.viewID = val;
    }

    [PunRPC]
    public void SetCollectibleID(int val, int arrayVal)
    {
        collectibles[arrayVal].phot.viewID = val;
    }

    public void Despawn()
    {
        GameManager.instance.Despawn(this);
    }
}
