using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;

public class LevelManager : MonoBehaviour
{
	public int asteroidsOnMap = 4;
	public float spawnRadius = 50.0f;
	public GameObject asteroid;
	public int playerScore = 0;
	public int playerLives = 3;
	public GameObject PauseUI;
	private static LevelManager instance;
	private float radius = 3f;

	public static LevelManager Instance {
		get{ return instance;}
	}

	// Singleton
	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start ()
	{
//		Instantiate(MeshGenerator.Instance.CreateMesh(3),new Vector3(10,10,0), new Quaternion());
	}
	
	// Update is called once per frame
	void Update ()
	{

		// Old Asteroids :)
//		if (!GameObject.FindGameObjectWithTag ("Asteroid")) {
//			RespawnAsteroids ();
//		}
		if (!GameObject.FindGameObjectWithTag ("MeshAsteroid")) {
			RespawnAsteroids ();
		}
		if(playerLives<=0){
			PauseUI.SetActive(true);
			PauseUI.GetComponent<PauseScript>().paused=true;
		}
	}

	void RespawnAsteroids ()
	{
		for (int i = 0; i<asteroidsOnMap; i++) {
			//Random position
			Vector3 spawnPosition = (Vector3)Random.insideUnitCircle * spawnRadius;
			int failsafe = 0;
//			Collider2D[] asteroidsOnWay = Physics2D.OverlapCircleAll (spawnPosition,asteroid.GetComponent<CircleCollider2D>().radius);

			//Check if that position is an empty space;
			while (!(Physics2D.OverlapCircle (spawnPosition, radius)==null) && failsafe <100) {
				spawnPosition = (Vector3)Random.insideUnitCircle * spawnRadius;
				failsafe++;
			}
			Instantiate (asteroid, spawnPosition, Quaternion.identity);
		}
	}
}
