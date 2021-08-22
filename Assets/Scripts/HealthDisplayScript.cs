using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplayScript : MonoBehaviour {
	public GameObject mainCamera;
	void Start () {
		mainCamera = GameObject.Find ("PlayerCamera");
	}
	
	void Update () {
		transform.rotation = mainCamera.transform.rotation;
	}
}
