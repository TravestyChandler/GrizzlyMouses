using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public RectTransform instructions;
	public bool instructionsOpen = false;
	public float instructionsTimer = 0.5f;
	public bool openingInst = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        SceneManager.LoadScene("mainGame");
    }


	public IEnumerator InstructionsRoutine(){
		openingInst = true;
		if (instructionsOpen) {
			float timer = 0f;
			while (timer < instructionsTimer) {
				timer += Time.deltaTime;
				float val = Mathf.Lerp (1f, 0f, timer / instructionsTimer);
				instructions.localScale = new Vector3 (val, val, val);
				yield return null;
			}
			instructions.localScale = Vector3.zero;
		} else {
			float timer = 0f;
			while (timer < instructionsTimer) {
				timer += Time.deltaTime;
				float val = Mathf.Lerp (0f, 1f, timer / instructionsTimer);
				instructions.localScale = new Vector3 (val, val, val);
				yield return null;
			}
			instructions.localScale = Vector3.one;
		}
		openingInst = false;
		instructionsOpen = !instructionsOpen;
	}

	public void InstructionsButton(){
		if (!openingInst) {
			StartCoroutine (InstructionsRoutine ());
		}
	}
    public void OpenInstructions()
    {
        instructions.gameObject.SetActive(true);
    }

    public void CloseInstructions()
    {
        instructions.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
