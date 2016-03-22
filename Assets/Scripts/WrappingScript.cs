using UnityEngine;
using System.Collections;

public class WrappingScript : MonoBehaviour {
	private Vector3 objectSize;
	private Vector3 min;
	private Vector3 max; 
	void Awake(){
		min = Camera.main.ScreenToWorldPoint(new Vector3(0,0,0));
		max = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, Screen.height, 0));
	}
	void Start(){
		objectSize = (GetComponent<Renderer>().bounds.size)/2;
	}
	void FixedUpdate(){
		float x = transform.position.x;
		float y = transform.position.y;
		if(min.x>(x+objectSize.x)){
			x=max.x;
		}
		else if(max.x<(x-objectSize.x)){
			x=min.x;
		}
		if(min.y>(y+objectSize.y)){
			y=max.y;
		}
		else if(max.y<(y-objectSize.y)){
			y=min.y;
		}
		transform.position=new Vector3(x,y,transform.position.z);
	}
}
