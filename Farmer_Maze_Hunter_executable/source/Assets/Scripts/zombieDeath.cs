using UnityEngine;
using System.Collections;

public class zombieDeath : MonoBehaviour {
	protected Animator anim;
	private GameObject hero;
	move moveScript;
	public bool canDamage;

	
	void Start(){
		hero = GameObject.Find("Character");
		moveScript = hero.GetComponent<move>();
		canDamage = true;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//if the player attacks the zombie it dies
	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.name == "hitzone" || coll.gameObject.name == "fire_hitzone" || coll.gameObject.name == "fireball(Clone)" ) {

						anim = transform.parent.gameObject.GetComponent<Animator> ();
						anim.SetTrigger ("death");
						canDamage = false;

						Destroy (transform.parent.gameObject, 1);
				
		}
	}





}
