using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CircleCollider2D),typeof(Rigidbody2D),typeof(WrappingScript))]
public class BulletScript : MonoBehaviour
{
	public float BulletSpeed = 40f;
	public float BulletLifeTime = 0.5f;

	private float life;
	private ParticleSystem ps;
	private Rigidbody2D rb;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		ps = GetComponent<ParticleSystem>();
		rb.AddForce (transform.up * BulletSpeed);
		life = BulletLifeTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		ps.Emit (10);
		life -= Time.deltaTime;
		if (life < 0)
			Destroy (gameObject);
	}
}
