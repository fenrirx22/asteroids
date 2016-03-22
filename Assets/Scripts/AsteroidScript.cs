using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

[RequireComponent(typeof(Rigidbody2D),typeof(WrappingScript))]
public class AsteroidScript : MonoBehaviour
{
	public AsteroidType asteroidType;
//	public float MinTorque = -10f;
//	public float MaxTorque = 10f;
	public float MinForce = 300f;
	public float MaxForce = 600f;
	public int NumOfAsteroidVerts = 8;
	public int points = 250;
	public int NumberOfChilds = 2;
	public GameObject Explosion;
	public GameObject ChildAsteroid;
	public PhysicsMaterial2D asteroidMaterial;
	private MeshFilter mf;
	private Mesh mesh;
	private LevelManager levelManager;
	private Rigidbody2D rb;

	void Awake ()
	{
		if (gameObject.tag == "MeshAsteroid" && ((int)asteroidType) == 1) { // chwilowo :) Sprite'y vs meshowe asteroidy
			mf = GetComponent<MeshFilter> ();
			mesh = new Mesh ();
			mf.mesh = mesh;

			mesh.vertices = CreateVertices (NumOfAsteroidVerts);
			mesh.triangles = CreateTriangles (NumOfAsteroidVerts);
			mesh.RecalculateBounds ();
			Vector2[] table = new Vector2[mesh.vertices.Length];
			for (int i =0; i<mesh.vertices.Length; i++) {
				table [i] = new Vector2 (mesh.vertices [i].x, mesh.vertices [i].y);
			}
			gameObject.AddComponent<PolygonCollider2D> ().points = table;
			gameObject.GetComponent<PolygonCollider2D> ().sharedMaterial = asteroidMaterial;

		}
	}

	// Use this for initialization
	void Start ()
	{
		levelManager = LevelManager.Instance;
		rb = GetComponent<Rigidbody2D> ();
		float randomX = Random.Range (-1f, 1f);
		float randomY = Random.Range (-1f, 1f);
		float force = Random.Range (MinForce, MaxForce) * rb.mass;
//		float torque = Random.Range (MinTorque, MaxTorque) * 10;

		rb.AddForce (new Vector2 (randomX, randomY) * force);
//		rb.AddTorque (torque);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		float goScale = gameObject.transform.localScale.x;
		Vector3 hitPosition = collider.transform.position;
		Vector3 hitVelocity = collider.attachedRigidbody.velocity;
		if (collider.gameObject.tag == "PlayerBullet") {

			// Visualize the contact point
			gameObject.layer = LayerMask.NameToLayer ("HitLayer");
			var lMask = 1 << gameObject.layer;
			RaycastHit2D rayHit = Physics2D.Raycast (hitPosition - (hitVelocity.normalized * 10), hitVelocity, 20f, lMask);
			RaycastHit2D rayHit2 = Physics2D.Raycast (hitPosition + hitVelocity.normalized * 10, -hitVelocity, 20f, lMask);

			Debug.DrawRay (hitPosition, -hitVelocity, Color.red, 1);
			Debug.DrawRay (hitPosition + hitVelocity.normalized * 8, hitVelocity, Color.blue, 1);
			Debug.DrawLine (rayHit.point, rayHit2.point, Color.white, 10f);
			if (ChildAsteroid != null) {
				List<Vector3> newVerticesOne = new List<Vector3> ();
				List<Vector3> newVerticesTwo = new List<Vector3> ();
				if (hitVelocity.x == 0) {
					// then its a parrarel line to OY
					// check left or right side
					foreach (var vert in mesh.vertices) {
						Vector3 vertice = transform.TransformPoint (vert);
						if (vertice.x < ((rayHit.point.x))) {//left
							newVerticesOne.Add (vert);
						} else if (vertice.x > ((rayHit.point.x))) {//right
							newVerticesTwo.Add (vert);
						}
						
					}	
				} else if (hitVelocity.y == 0) {
					// then its a parrarel line to OX
					// check if its above or under
					foreach (var vert in mesh.vertices) {
						Vector3 vertice = transform.TransformPoint (vert);
						if (vertice.y < ((rayHit.point.y))) {//down
							newVerticesOne.Add (vert);
						} else if (vertice.y > ((rayHit.point.y))) {//up
							newVerticesTwo.Add (vert);
						}
						
					}
				} else {
					float a, b;
					a = hitVelocity.y / hitVelocity.x; // wspolczynnik kierunkowy
					b = (-a * ((rayHit.point.x))) + (rayHit.point.y); // b = -ax + y
					foreach (var vert in mesh.vertices) {
						Vector3 vertice = transform.TransformPoint (vert);
						if (vertice.y < (a * vertice.x) + b) {//below
							newVerticesOne.Add (vert);
						} else if (vertice.y > (a * vertice.x) + b) {//above
							newVerticesTwo.Add (vert);
						}	
					}
				}
				if (newVerticesOne.Count == 0 || newVerticesTwo.Count == 0) {
					// If hit was registered exacly in vertex place then return;	
					return;
				}

				// Add 2 new vertexes ( hit points )
				Vector2 hitVert = new Vector3 ((rayHit.point.x - gameObject.transform.position.x) / goScale, (rayHit.point.y - gameObject.transform.position.y) / goScale, 0);
				Vector2 hitVert2 = new Vector3 ((rayHit2.point.x - gameObject.transform.position.x) / goScale, (rayHit2.point.y - gameObject.transform.position.y) / goScale, 0);
				newVerticesOne.Add (hitVert);
				newVerticesTwo.Add (hitVert);
				newVerticesOne.Add (hitVert2);
				newVerticesTwo.Add (hitVert2);
//
				var child1 = Instantiate (ChildAsteroid, transform.position, Quaternion.identity) as GameObject;
				var child2 = Instantiate (ChildAsteroid, transform.position, Quaternion.identity) as GameObject;
//				ChildAsteroid.GetComponent<AsteroidScript>().SetVertices(newVertices);
				child1.GetComponent<AsteroidScript> ().SetVertices (newVerticesOne);
				child2.GetComponent<AsteroidScript> ().SetVertices (newVerticesTwo);
				child1.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
				child2.GetComponent<Renderer> ().material = this.GetComponent<Renderer> ().material;
			}
			gameObject.layer = LayerMask.NameToLayer ("Default");

			levelManager.playerScore += points * (int)asteroidType;
			Instantiate (Explosion, transform.position, new Quaternion ());
			Destroy (gameObject);
			Destroy (collider.gameObject);
		}
	}

	private Vector3[] CreateVertices (int numberOfVerts) //=3
	{
		List<Vector3> listOfVerts = new List<Vector3> ();
		float radStep = 2 * Mathf.PI / numberOfVerts;
		for (int i = 0; i<numberOfVerts; i++) {
			listOfVerts.Add (new Vector3 (Mathf.Cos (radStep * i), Mathf.Sin (radStep * i), 0));
		}
		return listOfVerts.ToArray ();
	}

	public void SetVertices (List<Vector3> vertices)
	{
		Vector3 center = CalculateCentroid (vertices.ToArray ());
		vertices.Sort ((a,b) => GetAngle (a, center).CompareTo (GetAngle (b, center)));
		mf = GetComponent<MeshFilter> ();
		mesh = new Mesh ();
		mesh.vertices = vertices.ToArray ();
		Triangulator tr = new Triangulator (vertices.ToArray ());
		mesh.triangles = tr.Triangulate ();
		mf.mesh = mesh;
		Vector2[] table = new Vector2[mesh.vertices.Length];
		for (int i =0; i<mesh.vertices.Length; i++) {
			table [i] = new Vector2 (mesh.vertices [i].x, mesh.vertices [i].y);
		}
		gameObject.AddComponent<PolygonCollider2D> ().points = table;
		gameObject.GetComponent<PolygonCollider2D> ().sharedMaterial = asteroidMaterial;
	}
//		mesh.vertices=vertices.ToArray ();
	private int[] CreateTriangles (int numberOfVerts)//=3
	{
		List<int> listOfTriangles = new List<int> ();
		for (int i = 0; i<numberOfVerts-2; i++) {
			listOfTriangles.Add (0);
			listOfTriangles.Add (i + 1);
			listOfTriangles.Add (i + 2);
		}
		return listOfTriangles.ToArray ();
	}

	public Vector3 CalculateCentroid (Vector3[] points)
	{
		Vector3 temp = Vector3.zero;
		for (int i = 0; i < points.Length; i++) {
			temp += points [i];
		}

		return temp / points.Length;
	}

	public  float GetAngle (Vector3 a, Vector3 center)
	{
		return (Mathf.Atan2 (a.y - center.y, a.x - center.x)) * 180 / Mathf.PI;
	}
}

public enum AsteroidType
{
	Large=1,
	Medium,
	Small
}
