using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {
	public int points =0;
	//public bool[] keys;
	//declarations of some constants
	public GameObject flame;
	Vector2 temp;
	float h;
	protected Animator anim;
	public float attackRate;
	private float nextAttack;
	public float firespeed;
	public GameObject fireBall;
	GameObject fireclone;
	public float maxSpeed=5f;
	public float moveForce = 365f;
	public float jumpSpeed=400.0f;
	public bool gotItem;
	public bool canDoubleJump = false;
	bool facingRight= true;
	bool firePushed = false;
	bool charging = false;
	//for use in detection of ground
	public bool isGrounded;
	//float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	public Transform groundCheck;

	public bool canMoveRight = true;
	public bool canMoveLeft = true;

	public bool buildMap = false;

	//time it takes to complete attack animation
	public float attackAnimTime;
	


	//time that you have been charging your attack
	bool charged = false;
	bool jump;
	float count;
	
	void Start () {
		flame.SetActive (false);
		anim = GetComponent<Animator>();
		count = attackAnimTime;
		
		int getDiff = GameObject.Find ("AreaGen").GetComponent<AreaGeneration>().numAreas;
		//keys = new bool[getDiff];
	}

	

	void Awake()
	{
		// Setting up references.
		//Difficulty df = GameObject.Find("Difficulty").GetComponent<Difficulty>();
		//if (df.diff != 4 && df.timesDied>0) {
		//	keys = df.keys;
		//}
		groundCheck = transform.Find("groundCheck");
	}
	
	//Pause screen.
	void OnGUI() {
		//GUI.matrix = Matrix4x4.TRS (Vector3(0, 0, 0), Quaternion.identity, Vector3 (Screen.height / nativeVerticalResolution, Screen.height / nativeVerticalResolution, 1)); 		
		if(died) {
			GUI.Box(new Rect(0,0,Screen.width, Screen.height), "<size=70>YOU DIED</size>");
		}
		
		if(pause)
		{
			GUI.Box(new Rect(0,0,Screen.width, Screen.height), "<size=70>PAUSED</size>");
			// RenderSettings.fogDensity = 1;
			if(GUI.Button (new Rect(((Screen.width)/2)-70,(2 * Screen.height / 4),140,70), "Quit"))
			{
				print("Quit!");
				Application.Quit();
			}
			if(GUI.Button (new Rect(((Screen.width)/2)-70,(2 * Screen.height / 4)+80,140,70), "Restart"))
			{
				print("Restart");
				Application.LoadLevel("area_generation_test");
				Time.timeScale = 1.0F;
				pause = false;
			}
			if(GUI.Button (new Rect(((Screen.width)/2)-70,(2 * Screen.height / 4)+160,140,70), "Continue"))
			{
				print("Continue");
				Time.timeScale = 1.0F;
				pause = false;   
			}
		}
	}
	
	bool died = false;
	bool pause = false;
	void Update() {
	
		if(Input.GetButtonDown("Escape") && !pause) {
			Time.timeScale = 0.0F;
			pause = true;
		} else if (Input.GetButtonDown("Escape") && pause) {
			Time.timeScale = 1.0F;
			pause = false;
		}


		//if (Input.GetKeyDown("u")){charging = true; anim.SetTrigger ("charged");}
			h = Input.GetAxis ("Horizontal");
		//do idle animation if not input

		//if enough time has passed and fire is pushed, attack
		if (Input.GetButton ("Fire1") ){
			count -= Time.deltaTime;
		if (Input.GetButton ("Fire1") && Time.time > nextAttack) {


		
			firePushed = true;
			
			
			if (flame.activeInHierarchy){

				
				//if you hold down fire for the duration of your attack you will begin to charge up
				if (count < 0 && !charging) {
					anim.SetTrigger ("charge");
					charging = true;
					charged = true;
					
				}
				else if (!charging){
						//do a basic attack

						nextAttack = Time.time + attackRate;
						anim.SetTrigger ("attack");
						anim.SetFloat ("move", 0);

				}
			}
			else{	//do a basic attack

				nextAttack = Time.time + attackRate;
				anim.SetTrigger ("attack");
				anim.SetFloat ("move", 0);
				
				}
	
		} 
		}
		//if you are not pushing fire
		else{
			count = attackAnimTime;

			if (firePushed && charged) {
				Invoke("shootFireball",0.535f);
				charging = false;
				charged = false;
				firePushed = false;
				anim.SetTrigger ("charged");

			}
			else if (firePushed) {

				anim.SetTrigger ("idle");
				firePushed = false;
				charging = false;


			}
			if (Mathf.Abs(h) < 0.01f && !charging) {

				anim.SetTrigger ("idle");
			}
		}



		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("ground"));
		if(isGrounded)
			letOff = false;
		
		if (gotItem && isGrounded) canDoubleJump = true;
				
		if (Input.GetButtonDown ("Vertical") && isGrounded) 
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
			rigidbody2D.AddForce(new Vector2(0f, jumpSpeed));

		}
		else if (Input.GetButtonDown ("Vertical") && canDoubleJump) 
		{
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0f);
			rigidbody2D.AddForce(new Vector2(0f, jumpSpeed));
			canDoubleJump = false;
		}
		
		if(!letOff){
			if(Input.GetButtonUp("Vertical" )&& rigidbody2D.velocity.y>0) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 5f);
				letOff = true;
			}
		}

	}
	bool letOff;

	void FixedUpdate () {
		/*if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.L)) {
			AreaGeneration a = new AreaGeneration();
			a.doThing();
		}
		
		if(Input.GetKeyDown(KeyCode.L)) {
			Application.LoadLevel (Application.loadedLevelName);
		}*/
			
		float h = Input.GetAxis ("Horizontal");

		if(canMoveLeft && canMoveRight)
			rigidbody2D.velocity = new Vector2 (maxSpeed*h, rigidbody2D.velocity.y);
		else if (h>0 && canMoveRight)
			rigidbody2D.velocity = new Vector2 (maxSpeed*h, rigidbody2D.velocity.y);
		else if (h<0 && canMoveLeft)
			rigidbody2D.velocity = new Vector2 (maxSpeed*h, rigidbody2D.velocity.y);
			
		if(!canMoveLeft && rigidbody2D.velocity.x<0) {
			rigidbody2D.velocity  = new Vector2 (0f, rigidbody2D.velocity.y);
			transform.position = new Vector2(transform.position.x+0.1f, transform.position.y);
			}
		if(!canMoveRight && rigidbody2D.velocity.x>0) {
			rigidbody2D.velocity  = new Vector2 (0f, rigidbody2D.velocity.y);
			transform.position = new Vector2(transform.position.x-0.1f, transform.position.y);
			
			}
		

		
			
		
		
		//else {
		//	rigidbody2D.velocity = new Vector2 (0f,0f);
		//}

			
			
//*********disabled wallfix**********	
 	//rigidbody2D.velocity = new Vector2 (maxSpeed * h, rigidbody2D.velocity.y);
		anim.SetFloat ("move",Mathf.Abs(h));


		/*
		if (h * rigidbody2D.velocity.x < maxSpeed)
						rigidbody2D.AddForce (Vector2.right * h * moveForce);

		if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
						rigidbody2D.velocity = new Vector2((Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed), rigidbody2D.velocity.y);
		*/


		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

	}
	void shootFireball(){
		fireclone = Instantiate(fireBall, flame.transform.position, flame.transform.rotation) as GameObject;
		if (facingRight){fireclone.rigidbody2D.velocity=new Vector2(firespeed,0);}
		else {fireclone.rigidbody2D.velocity=new Vector2(-firespeed,0);}
		Destroy (fireclone, 0.5f);
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

	IEnumerator ReloadGame(){
		GameObject.Find("Difficulty").GetComponent<Difficulty>().timesDied ++;
		yield return new WaitForSeconds (2);
		Application.LoadLevel (Application.loadedLevel);
		
		
	}
	//if you run into an object...
	void OnCollisionEnter2D(Collision2D coll) {
		zombieDeath deathscript = coll.gameObject.GetComponentInChildren<zombieDeath> ();	
	
		//if you touch an enemy you die
		if (coll.gameObject.tag == "enemy"&& deathscript.canDamage) {
			died= true;
			
			anim.SetTrigger ("death");	
			StartCoroutine ("ReloadGame");
			this.enabled=false;
		}
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("ground"));
		
		

		if (!isGrounded) {
			rigidbody2D.velocity = new Vector2 (0f, rigidbody2D.velocity.y);
			if(coll.transform.position.x>rigidbody2D.position.x  ) {
				//rigidbody2D.position = new Vector2 (rigidbody2D.position.x -0.05f, rigidbody2D.position.y);
				canMoveRight = false;
			} else if(coll.transform.position.x<rigidbody2D.position.x ) {
				//rigidbody2D.position = new Vector2 (rigidbody2D.position.x +0.05f, rigidbody2D.position.y);
				canMoveLeft = false;
			}
		} else {
			canMoveLeft = true;
			canMoveRight = true;
			}
	}

	void OnCollisionExit2D(Collision2D coll) {
			canMoveLeft = true;
			canMoveRight = true;

		}
	//if you touch the door or the item, stuff can happen


}
