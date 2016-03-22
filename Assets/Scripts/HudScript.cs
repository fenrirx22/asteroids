using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HudScript : MonoBehaviour {

	public Sprite[] HeartSprites;
	public Text Lives;
	public Text Score;
//	private PlayerSpaceShip playerSpaceShip;
	private LevelManager levelManager;
	// Use this for initialization
	void Start () {
		levelManager=LevelManager.Instance;
//		playerSpaceShip = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerSpaceShip>();
	}
	
	// Update is called once per frame
	void Update () {
		Lives.text=levelManager.playerLives.ToString ();
		Score.text=levelManager.playerScore.ToString ();
	}
}
