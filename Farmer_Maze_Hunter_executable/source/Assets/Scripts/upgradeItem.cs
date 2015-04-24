using UnityEngine;
using System.Collections;

public class upgradeItem : MonoBehaviour {
	private GameObject flame;

	void Start(){
		flame = GameObject.Find("Character/hitzone/fire_hitzone");

	}
	void OnCollisionEnter2D(Collision2D coll) {
		flame.SetActive (true);
		Destroy (gameObject);
	}
}
