using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour {

    RectTransform rect;
	// Use this for initialization
	void Start () {
        rect = this.GetComponent<RectTransform>();
        rect.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Open(float openTime)
    {
        StartCoroutine(OpenPanel(openTime));
    }

    IEnumerator OpenPanel(float openTime)
    {
        float timer = 0f;
        while (timer < openTime)
        {
            yield return null;
            timer += Time.deltaTime;
            float val = Mathf.Lerp(0, 1, timer / openTime);
            rect.localScale = new Vector3(val, val, val);
        }
        rect.localScale = Vector3.one;
    }

    public void Close(float closeTime)
    {
        StartCoroutine(ClosePanel(closeTime));
    }
    IEnumerator ClosePanel(float closeTime)
    {
        float timer = 0f;
        while (timer < closeTime)
        {
            yield return null;
            timer += Time.deltaTime;
            float val = Mathf.Lerp(1, 0, timer / closeTime);
            rect.localScale = new Vector3(val, val, val);
        }
        rect.localScale = Vector3.zero;
    }

    public void Restart()
    {
        Close(UIController.Instance.deathPanelTimer);
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(UIController.Instance.deathPanelTimer);
        GameManager.instance.photView.RPC("RPCRestartGame", PhotonTargets.All);
    }

    public void Load()
    {
        Close(UIController.Instance.deathPanelTimer);
        StartCoroutine(LoadMenu());
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds (UIController.Instance.deathPanelTimer);
        GameManager.instance.ToMainMenu();
    }
}
