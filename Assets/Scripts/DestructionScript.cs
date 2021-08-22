using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour {
	public float destroyTimeCount; 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		destroyTimeCount -= Time.deltaTime;
		if (destroyTimeCount <= Time.deltaTime) {
			Destroy(gameObject);
		}
	}
}
