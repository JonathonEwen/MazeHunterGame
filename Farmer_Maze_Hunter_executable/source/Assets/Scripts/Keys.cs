using UnityEngine;
using System.Collections;

public class Keys : MonoBehaviour {
	move moveScript;
	public int keyNumber = -1;
	GameObject flame;
	// Use this for initialization
	void Start () {
		moveScript = GameObject.Find("Character").GetComponent<move>();
		flame = moveScript.flame;

		SpriteRenderer r = GetComponent<SpriteRenderer>();
		r.color = Color.blue;

		switch(keyNumber) {
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
			r.color = new Color(1f, 1f, 1f, 1f);
			break;
		case 8:
			r.color = Color.white;
			//gameObject.transform.localScale += new Vector3(3f, 3f, 0f);
			break;	

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setKeyNumber(int i) {
		keyNumber = i;
	}
	

		
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.name == "Character") {
			//moveScript.keys[keyNumber-1] = true;
			
			//back up keys in case of death;
			Difficulty df = GameObject.Find("Difficulty").GetComponent<Difficulty>();
			df.keys[keyNumber-1] = true;
			
			
			if(keyNumber >= (GameObject.Find ("AreaGen").GetComponent<AreaGeneration>().numAreas )/2) {
				df.gotFirepower = true;
				df.youGotItem();
				flame.SetActive (true);
			}
					
			if(keyNumber == GameObject.Find("AreaGen").GetComponent<AreaGeneration>().numAreas) {
				Application.LoadLevel("End_Screen");
			}
			//play sound
		
			Destroy(gameObject);
		}
	}
}
