using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public RectTransform instructions;
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
