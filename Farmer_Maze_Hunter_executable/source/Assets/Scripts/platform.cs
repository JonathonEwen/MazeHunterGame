using UnityEngine;
using System.Collections;

public class platform : MonoBehaviour {

	bool inside;
	
	bool pressed;
	
	
	void OnCollisionStay2D(Collision2D coll) {
		if(pressed || coll.transform.position.y < gameObject.transform.position.y) {			
			inside = true;
			pressed = false;
			gameObject.transform.collider2D.isTrigger = true;
		}
	}
	
	void OnColliderExit2D(Collision2D coll) {
		inside = false;
		pressed = false;
	}
	
	void Update() {
		if (Input.GetButtonDown ("Fall") || Input.GetButton("Fall")) {
			pressed = true;
		} else {
			pressed = false;
		}
		if(GameObject.Find ("Character").rigidbody2D.velocity.y>0) {
			gameObject.transform.collider2D.isTrigger = true;
			
		}
	}
	
	
	void OnTriggerExit2D(Collider2D coll) {
		inside = false;
		pressed = false;
	}
	
	
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.transform.position.y > gameObject.transform.position.y && !inside)
			gameObject.transform.collider2D.isTrigger = false;
		if(pressed || coll.transform.position.y < gameObject.transform.position.y) {			
			inside = true;
			pressed = false;
			gameObject.transform.collider2D.isTrigger = true;
		}
	}

}
