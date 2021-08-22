using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneScript : MonoBehaviour {
	public void changeScene (int id) {
		Application.LoadLevel (id);
	}
	void Start () {
		
	}
	void Update () {
		
	}
}
