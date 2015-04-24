using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	void Start() {
		rigidbody2D.AddTorque(100f);
	}
	
	void FixedUpdate() {
		transform.position = new Vector2(Mathf.Sin(Time.time)*2, Mathf.Cos(Time.time)*1.2f);
	}

	

	void OnGUI()
	{
		const int buttonWidth = 84;
		const int buttonHeight = 60;
		
		Rect Title = new Rect(Screen.width/2 -(2*Screen.width/3)/2, 50, 2*Screen.width/3, Screen.height/8);
		
		GUI.Box(Title, "<size=70>FARMER MAZE HUNTER\n</size>");
		
		Rect us = new Rect(Screen.width/2 -(2*Screen.width/3)/2, 120, 2*Screen.width/3, Screen.height/8);
		
		GUI.Box(us, "<size=20>By E.M games for CMPT306\n</size>");
		
		
		//guiText.fontSize = 12;
		
		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect exit = new Rect(
			Screen.width - (buttonWidth)-10,
			(Screen.height) - (buttonHeight)-10,
			buttonWidth,
			buttonHeight
			);
		Rect easy = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) + (buttonHeight),
			buttonWidth,
			buttonHeight
			);
		Rect med = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) + (buttonHeight) - 80,
			buttonWidth,
			buttonHeight
			);
		Rect hard = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) + (buttonHeight) - 160,
			buttonWidth,
			buttonHeight
			);
		Rect HARDCORE = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) + (buttonHeight) - 240,
			buttonWidth,
			buttonHeight
			);
		Rect demo = new Rect(
			Screen.width / 2 - (buttonWidth*3 / 2),
			(2 * Screen.height / 3) + (buttonHeight) - 320,
			buttonWidth*3,
			buttonHeight
			);
		
		// Draw a button to start the game
		if(GUI.Button(easy,"Easy"))
			start(1);			
		if(GUI.Button(med,"Medium"))
			start (2);
		if(GUI.Button(hard,"Hard"))
			start(3);
		if(GUI.Button(HARDCORE, "HARDCORE"))
			start (4);
		if(GUI.Button(demo, "Quick Demo Mode"))
			start (0);
		if(GUI.Button(exit, "Exit")) {
			Application.Quit();
		}
		
	}
	
	void start(int i) {
		GameObject.Find("Difficulty").GetComponent<Difficulty>().pushDiff(i);
		
		Application.LoadLevel("area_generation_test");
	}
}
