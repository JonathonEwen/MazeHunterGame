using UnityEngine;
using System.Collections;

public class doublejumpitem : MonoBehaviour {
	private GameObject hero;
	move moveScript;
	
	void Start(){
		hero = GameObject.Find("Character");
		moveScript = hero.GetComponent<move>();
		
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.name == "Character"){
			moveScript.gotItem = true;
			Destroy (gameObject);
		}
	}
}
