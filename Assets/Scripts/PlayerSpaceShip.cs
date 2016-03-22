using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WrappingScript),typeof(Rigidbody2D), typeof(PolygonCollider2D))]
public class PlayerSpaceShip : MonoBehaviour
{
	public float RotationSpeed;
	public float AccelerateSpeed;
	public ParticleSystem ParticleThrustEmmiter;
	public GameObject BulletPrefab;
	public GameObject Explosion;
	public int spawnDelay = 2;
	public float invincibilityTime = 4.0f;
	private Rigidbody2D rb;
	private Renderer playerRenderer;
	private Vector3 startingPosition = new Vector3 (0, 0, 0);
	public bool invicible = false;
	private bool isActive = true;
	private LevelManager levelManager;
	private SoundManagerScript soundManager;

	// Use this for initialization
	void Start ()
	{
		levelManager = LevelManager.Instance;
		soundManager = SoundManagerScript.Instance;
		rb = GetComponent<Rigidbody2D> ();
		playerRenderer = GetComponent<Renderer> ();
	}
	
	void Update ()
	{
		float horizontalArrows = Input.GetAxis ("Horizontal");
		bool verticalArrows = Input.GetButton ("Vertical");
		transform.Rotate (new Vector3 (0, 0, -horizontalArrows * RotationSpeed * Time.deltaTime));
		if (verticalArrows) {
			if (!soundManager.PlayerThrustSound.isPlaying) {
				soundManager.PlayerThrustSound.Play ();
			}
			ParticleThrustEmmiter.Emit (5);

			rb.AddForce (transform.up * AccelerateSpeed * Time.deltaTime);

		} else {
			soundManager.PlayerThrustSound.Stop ();
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			soundManager.PlayerShootingSound.Play ();
			Instantiate (BulletPrefab, transform.position, transform.rotation);
		}
	}


	void OnTriggerStay2D (Collider2D collider)
	{

		// in case if player will stay behind an asteroid after the  invincibility will went off
		if (collider.gameObject.tag == "MeshAsteroid") {
			KillPlayer ();		
		}
	}

	void KillPlayer ()
	{
		if (!invicible) {
			if (soundManager.PlayerThrustSound.isPlaying)
				soundManager.PlayerThrustSound.Stop ();
			levelManager.playerLives -= 1;
			invicible = true;
			Instantiate (Explosion, transform.position, new Quaternion ());
			transform.gameObject.SetActive (false);
			InvokeRepeating ("CountDown", 1, 1);
		}
	}

	void CountDown ()
	{
		if (--spawnDelay < 0) {
			spawnDelay = 2;
			CancelInvoke ("CountDown");
			Respawn ();
		}
	}

	void Respawn ()
	{
		GameObject player = GameObject.Find ("Player");
		if (!player) {
			gameObject.SetActive (true);
			transform.position = startingPosition;
			InvokeRepeating ("BlinkShip", 0f, 0.5f);
		}
	}

	void BlinkShip ()
	{
		invincibilityTime -= 0.5f;
		if (invincibilityTime < 0) {
			invicible = false;
			CancelInvoke ("BlinkShip");
			playerRenderer.enabled = true;
			invincibilityTime = 4.0f;
		} else {
			isActive = !isActive;
			playerRenderer.enabled = isActive;
		}
		
	}
}
