using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourComponentScript : MonoBehaviour {
	public GameObject tank;
	void Start () {
		
	}
	void Update () {
		
	}
	public GameObject selectTank (bool changeSelected) {
		if (tank.GetComponent<ArmourScript>().isPlayer) {
			if (changeSelected) {
				tank.GetComponent<ArmourScript>().selected = true;
			}
			return tank;
		} else {
			return null;
		}
	}
}
