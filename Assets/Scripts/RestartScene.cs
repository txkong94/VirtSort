using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour {

	// Use this for initialization
	public void RestartGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
	}

	void Update()
	{
		if(Input.GetButtonUp("Restart")) 
		{
            //RestartGame();
        }
	}

}
