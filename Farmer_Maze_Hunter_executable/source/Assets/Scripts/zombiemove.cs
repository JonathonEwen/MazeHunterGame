using UnityEngine;
using System.Collections;

public class zombiemove : MonoBehaviour {
	private Transform hero;
	public float speed;
	bool facingRight= false;
	float distance;
	float walktime = 2;
	// Use this for initialization
	void Start () {
		hero = GameObject.Find("Character").transform;
	}
	
	// Update is called once per frame
	void Update () {

		distance = Vector2.Distance (transform.position, hero.position);
		if (rigidbody2D.velocity.x > 0 && !facingRight)Flip();
		else if (rigidbody2D.velocity.x < 0 && facingRight) Flip ();
		if (distance < 5) {
			if (transform.position.x - hero.position.x > 0 )
				rigidbody2D.velocity = new Vector2 (-1,rigidbody2D.velocity.y);
			else {rigidbody2D.velocity = new Vector2 (1,rigidbody2D.velocity.y);}
				
		}
		else{
			walktime += Time.deltaTime;
			if (walktime >2){
				Vector2 walkDir = Random.insideUnitCircle;
				rigidbody2D.velocity = new Vector2 (walkDir.x,rigidbody2D.velocity.y);
				walktime = 0;
			}
		}
		//walk back and forth
	}
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
