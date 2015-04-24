using UnityEngine;
using System.Collections;

public class LockedDoor : MonoBehaviour {

	public int doorNumber = -1;
	Difficulty diff;
	
	void Awake() {
		diff  = GameObject.Find("Difficulty").GetComponent<Difficulty>();
	}
	
	public void doColour(int gimme) {
		doorNumber = gimme;
		SpriteRenderer r;
		r = GetComponent<SpriteRenderer>();
		r.color = Color.blue;
		
		switch(doorNumber) {
		case -1:
			break;
		case 1:
			r.color = new Color(1f, 0f, 0f, 1f);
			break;
		case 2:
			r.color = new Color(0f, 1f, 0f, 1f);
			break;
		case 3:
			r.color = new Color(0f, 0f, 1f, 1f);
			break;
		case 4:
			r.color = new Color(1f, 0f, 1f, 1f);
			break;
		case 5:
			r.color = new Color(0f, 1f, 1f, 1f);
			break;
		case 6:
			r.color = new Color(1f, 1f, 0f, 1f);
			break;
		case 7:
			r.color = new Color(1f, 1f, 1f, 0.2f);
			break;
		case 8:
			r.color = Color.black;
			break;	
			
		}
	}
	
	Color colourSwitch(int a) {
		switch(a) {
		case 1:
			return new Color( .4f, 0f, 0f,  0.6f);
			 
		case 2:
		return new Color(0f,  .4f, 0f,  0.6f);
		 
		case 3:
		return new Color(0f, 0f,  .4f,  0.6f);
		 
		case 4:
		return new Color( .4f, 0f,  .4f,  0.6f);
		 
		case 5:
		return new Color(0f,  .4f,  .4f,  0.6f);
		 
		case 6:
		return new Color( .4f,  .4f, 0f,  0.6f);
		 
		case 7:
		return new Color( .4f, 1f, 1f, 0.0f);
		 
		case 8:
		return Color.black;
		 
		}
		
		return Color.white;
	}
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.name == "Character" && diff.keys[doorNumber-1]) {
			Camera c = GameObject.Find("Main Camera").GetComponent<Camera>();
			c.backgroundColor = colourSwitch(doorNumber);
			//play sound
			Destroy(gameObject);
		}
	}
	
	
}
