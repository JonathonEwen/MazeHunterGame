using UnityEngine;
using System.Collections;

public class Difficulty : MonoBehaviour {
//pass through method from title screen to end screen.
//Mostly just stores information about difficulty level
//and the keys the player has.
//Also handles the gui during gameplay

	public int diff;
	
	public int timesDied=0;
	public float timeAtStart;
	
	public bool[] keys;
	
	GameObject[] items;
	
	public Texture ithe;
	public bool pause;
	
	public bool gotFirepower;
	

	public bool doGui;
	public void pushDiff(int i) {
		timeAtStart = Time.time;
		Debug.Log("Time at start" +timeAtStart);
		diff = i;
		
		

	}
	
	float gotItemAt;
	public void youGotItem() {
		gotItemAt = Time.time;
	}
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		items = Resources.LoadAll<GameObject> ("Items");
		
	}
	
	Color colourSwitch(int a) {
		switch(a) {
		case 1:
			return new Color( 1f, 0f, 0f,  1f);
			
		case 2:
			return new Color(0f,  1f, 0f,  1f);
			
		case 3:
			return new Color(0f, 0f,  1f,  1f);
			
		case 4:
			return new Color( 1f, 0f,  1f,  1f);
			
		case 5:
			return new Color(0f,  1f,  1f,  1f);
			
		case 6:
			return new Color( 1f,  1f, 0f,  1f);
			
		case 7:
			return new Color( 1f, 1f, 1f, 1.0f);
			
		case 8:
			return Color.black;
			
		}
		
		return Color.white;
	}
	
	//for showing what keys the player has
	void OnGUI() {
		//show message for about 3 seconds
		if(gotFirepower && (Time.time-gotItemAt)<3) {
			GUI.Box(new Rect(0,0,Screen.width, Screen.height), "<size=70>\nHOLD ATTACK FOR FIREPOWER!</size>");
		}
	
		if(doGui) {
			for(int i=0; i<keys.Length; i++) {
				if(keys[i]) {
					//make block
					Rect aKey = new Rect(Screen.width-45, 20*i + 10, 20, 20);
					//ithe.filterMode
					GUI.color = colourSwitch(i+1);
					GUI.Box(aKey, ithe);
				}
			}
			Rect easy = new Rect(Screen.width-50, 10, 30, 150);
			
			GUI.Box(easy,"");
		}
	}
}
