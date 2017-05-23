using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidBackgroundFrame : MonoBehaviour {
    public Transform startP, endP;
    public float SpeedModifier = 0.25f;
    public bool despawning = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector2(GameManager.instance.CurrentSpeed * SpeedModifier, 0f) * Time.deltaTime);
        if (this.transform.position.x < GameManager.instance.backEndVector.x && !despawning)
        {
            despawning = true;
            Despawn();
        }
    }

    public void Despawn()
    {
        GameObject obj = GameObject.Instantiate(GameManager.instance.midBackgroundPrefab, new Vector3(100f, 0f, 0f), Quaternion.identity);
        MidBackgroundFrame other = obj.GetComponent<MidBackgroundFrame>();
        float dist = other.transform.position.x - other.startP.position.x;
        obj.transform.position = new Vector3(GameManager.instance.midBackgrounds[1].endP.position.x + dist, 0);
        GameManager.instance.midBackgrounds.Add(obj.GetComponent<MidBackgroundFrame>());
        GameManager.instance.midBackgrounds.Remove(this);
        Destroy(this.gameObject);
    }
}
