using UnityEngine;
using System.Collections;

public class FloatingPlatform : MonoBehaviour {
	public float heightIncrease = 0.25f;
	public float floatTime = 0.5f;
	public float startY, endY;
	// Use this for initialization
	void Start () {
		startY = this.transform.position.y;
		endY = startY + heightIncrease;
		StartCoroutine (FloatRoutine ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator FloatRoutine(){
		yield return new WaitForSeconds (UnityEngine.Random.Range (0f, 0.5f));
		float timer = 0f;
		while (true) {
			while (timer < floatTime) {
				timer += Time.deltaTime;
				float yVal = Mathf.Lerp (startY, endY, timer / floatTime);
				this.transform.position = new Vector2 (this.transform.position.x, yVal);
				yield return null;
			}
			timer = 0f;
			while (timer < floatTime) {
				timer += Time.deltaTime;
				float yVal = Mathf.Lerp (endY, startY, timer / floatTime);
				this.transform.position = new Vector2 (this.transform.position.x, yVal);
				yield return null;
			}
			timer = 0f;
		}
	}
}
