using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	/*
	float shiftMult = 1;
	public GameObject masterController;
	void Start () {
		if (PlayerPrefs.GetInt("map") == 1) {
			transform.position = new Vector3 (71, 18, 66);
		} else if (PlayerPrefs.GetInt("map") == 2) {
			transform.position = new Vector3 (81, 18, 40);
		} else if (PlayerPrefs.GetInt("map") == 3) {
			transform.position = new Vector3 (17, 18, 30);
		}
		stoppedDrag = true;
	}
	
	void Update () {
		lastMousePos = currentMousePos;
		currentMousePos = Input.mousePosition;
		if (Input.touchCount == 2) {
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			Vector3 hit1Point = Vector3.zero;

			RaycastHit hit1;
			Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(touchOne.position.x, touchOne.position.y, 0));
			if (Physics.Raycast (ray1, out hit1, 1000f)) {
				hit1Point = hit1.point;
			}

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchZero.position.x, touchZero.position.y, 0));
			if (Physics.Raycast (ray, out hit, 1000f)) {
				if (!(transform.position.y >= 35 && deltaMagnitudeDiff > 0 || transform.position.y <= 8 && deltaMagnitudeDiff <= 0)) {
					Quaternion originalRotation = transform.rotation; 
					transform.LookAt ((hit.point + hit1Point) / 2);
					transform.Translate (new Vector3(0, 0, -deltaMagnitudeDiff / 10), Space.Self);
					transform.rotation = originalRotation;
				}
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			stoppedDrag = true;
		}
		if (!masterController.GetComponent<ControllerScript>().dragUnits && !masterController.GetComponent<ControllerScript>().touchContainsUI && !masterController.GetComponent<ControllerScript>().isConstructing) {
			if (Input.touchCount < 2) {
				if (Input.GetMouseButton (0)) {
					if (stoppedDrag) {
						lastMousePos = currentMousePos;
						stoppedDrag = false;
					}
					movePos = -(new Vector3 (lastMousePos.y, 0, -lastMousePos.x) - new Vector3 (currentMousePos.y, 0, -currentMousePos.x)) / 40;
				}
			} else {
				stoppedDrag = true;
			}

			transform.position += movePos;
			movePos *= 0.9f;
		}
		if (Input.GetKey(KeyCode.LeftShift)) {
			shiftMult = 4;
		} else {
			shiftMult = 2;
		}
		if (Input.GetKey(KeyCode.UpArrow)) {
			transform.position = new Vector3(transform.position.x - Time.deltaTime * 16f * shiftMult, transform.position.y, transform.position.z);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			transform.position = new Vector3(transform.position.x + Time.deltaTime * 16f * shiftMult, transform.position.y, transform.position.z);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * 16f * shiftMult);
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * 16f * shiftMult);
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0) {
			transform.Translate (new Vector3(0, 0, Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 60), Space.Self);
		}
		if (transform.position.x > masterController.GetComponent<ControllerScript>().mapBorders[3].x) {
			transform.position = new Vector3 (masterController.GetComponent<ControllerScript>().mapBorders[3].x, transform.position.y, transform.position.z);
		}
		if (transform.position.x < masterController.GetComponent<ControllerScript>().mapBorders[0].x) {
			transform.position = new Vector3 (masterController.GetComponent<ControllerScript>().mapBorders[0].x, transform.position.y, transform.position.z);
		}
		if (transform.position.z > masterController.GetComponent<ControllerScript>().mapBorders[3].z) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, masterController.GetComponent<ControllerScript>().mapBorders[3].z);
		}
		if (transform.position.z < masterController.GetComponent<ControllerScript>().mapBorders[0].z) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, masterController.GetComponent<ControllerScript>().mapBorders[0].z);
		}
		if (transform.position.y < 8) {
			transform.position = new Vector3 (transform.position.x, 8, transform.position.z);
		}
		if (transform.position.y > 35) {
			transform.position = new Vector3 (transform.position.x, 35, transform.position.z);
		}
	}*/
}
