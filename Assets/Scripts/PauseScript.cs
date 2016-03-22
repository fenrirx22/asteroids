using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	public GameObject PauseUI;
	public GameObject LevelManager;
	public bool paused = true;
	// Use this for initialization
	void Start () {
		PauseUI.SetActive(true);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			paused=false;
		}
		if(paused){
			Time.timeScale=0;
			PauseUI.SetActive(true);
		}
		else{
			Time.timeScale=1;
			PauseUI.SetActive(false);
			var lmScript = LevelManager.GetComponent<LevelManager>();
			lmScript.playerLives=3;
			lmScript.playerScore=0;
			var asteroids = GameObject.FindGameObjectsWithTag("MeshAsteroid");
			foreach (var asteroid in asteroids){
				Destroy (asteroid);
			}

		}
	}
}
