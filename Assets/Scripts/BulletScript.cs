using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int damage;
	public GameObject target; 
	float destroyTimer = 3;
	void Start () {
		transform.Translate(Vector3.forward, Space.Self);
	}
	
	void Update () {
		transform.Translate(Vector3.forward * Time.deltaTime * 20f, Space.Self);
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 0) {
			Destroy(gameObject);
		}
		if (target == null) {
			Destroy(gameObject);
		} else if (Vector3.Distance(transform.position, target.transform.position) < 1.5f) {
			if (target.GetComponent<ArmourScript>() != null) {
				target.GetComponent<ArmourScript>().loseHealth((int)Random.Range(damage * 0.65f, damage * 1.35f));
				Destroy(gameObject);
			}
			if (target.GetComponent<DefenceScript>() != null) {
				target.GetComponent<DefenceScript>().loseHealth((int)Random.Range(damage * 0.6f, damage * 1.35f));
				Destroy(gameObject);
			}
		}
	}
}
