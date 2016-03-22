using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour {
	public AudioSource PlayerShootingSound;
	public AudioSource ExplosionSound;
	public AudioSource PlayerThrustSound;

	private static SoundManagerScript instance;
	public static SoundManagerScript Instance{
		get{return instance;}
	}
	void Awake(){
		if(instance!=null && instance!=this){
			Destroy (gameObject);
		}
		else{
			instance=this;
		}
		DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
