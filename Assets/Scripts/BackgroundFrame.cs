using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFrame : MonoBehaviour {

    public Transform startP, endP;
    public float SpeedModifier = 0.25f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(new Vector2(GameManager.instance.CurrentSpeed * SpeedModifier, 0f) * Time.deltaTime);
        if(this.transform.position.x < GameManager.instance.backEndVector.x)
        {
            Despawn();
        }
	}

    public void Despawn()
    {
        GameObject obj = GameObject.Instantiate(this.gameObject, new Vector3(100f, 0f, 0f), Quaternion.identity);
        Destroy(this.gameObject);
    }
}
