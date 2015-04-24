using UnityEngine;
using System.Collections;

public class End : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void Awake () {
		Difficulty df = GameObject.Find("Difficulty").GetComponent<Difficulty>();
		time = Time.time - df.timeAtStart;
		time = Mathf.Floor(time);	
	}
	
	float time;
	
	void OnGUI() {
		Rect youWin = new Rect(Screen.width/2 -(2*Screen.width/3)/2, 50, 2*Screen.width/3, Screen.height/8);
		
		Rect stats = new Rect(Screen.width/2 - Screen.height/4, Screen.height/2 - Screen.height/4, Screen.width/4, Screen.height/4);
		
		GUI.Box(youWin, "<size=70>YOU WIN!</size>");
		
		
		Difficulty df = GameObject.Find("Difficulty").GetComponent<Difficulty>();
		GUI.Box(stats, "<size=30>Times died:"+ df.timesDied +"\ntime take: " + time +"</size>");
		
		Rect easy = new Rect(
			Screen.width / 2 - (84 / 2),
			(2 * Screen.height / 3) + (60),
			84,
			60
			);
			
		if(GUI.Button(easy,"Main Menu")) {
			Application.LoadLevel("Title_Screen");
		}
			
		
	}
	
}
