using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArmourScript : MonoBehaviour {
	public GameObject explosionPrefab;
	public float health; 
	public bool isPlayer; 
	float maxHealth;
	public GameObject masterController;
	public bool selected;
	public int damage; 
	public float speed;
	public float rotationSpeed;
	public GameObject bulletPrefab; 
	public float shootFrequency;
	Vector3 previousPosition;
	Vector3 targetPosition;
	Quaternion previousRotation;
	Quaternion targetRotation;
	public bool rotate;
	float rotateCount;
	public bool move;
	public float moveCount;
	public float detectionRange;  
	public GameObject previousRaycastObject;
	public Vector3 previousRaycastPosition = Vector3.zero;

	public bool attackPlayer;

	public Vector3 originalPos;
	public Vector3[] pathFindPoints;
	int pathFindIndex;

	Vector3 finalDestination;

	Quaternion previousTurretRotation;
	Quaternion targetTurretRotation;
	bool turretRotate;
	bool turretRotateBack;
	float turretRotateCount; 
	float turretRotateBackCount;
	GameObject selectedRing;
	GameObject rotateBody;
	public GameObject target; 
	GameObject turret;

	public float delayTime = 0;
	float circleResetDelay = 0; 

	float shootDelay;
	bool canShoot; 
	float height;

	bool startingMove = true;

	GameObject previousCircledObject = null;


	void Start () {
		PathFindGridScript pathFindScript = masterController.GetComponent<PathFindGridScript> ();
		maxHealth = health; 
		shootDelay = shootFrequency; 
		turret = transform.GetChild(1).gameObject;
		height = 0; 
		rotateBody = transform.GetChild(4).gameObject;
		selectedRing = transform.GetChild(0).gameObject; 
		if (!isPlayer) {
			startingMove = false;
			float x = transform.position.x;
			float z = transform.position.z;
			Vector2 pos = pathFindScript.gridArray [(int)(x + (pathFindScript.gridSize.x / 2)), (int)(z + (pathFindScript.gridSize.y / 2))].position;

			if (!moveTank (pos.x + 2, pos.y, true)) {
				if (!moveTank (pos.x - 2, pos.y, true)) {
					if (!moveTank (pos.x, pos.y + 2, true)) {
						moveTank (pos.x, pos.y - 2, true);
					}
				}
			}
		}
	}
	void delayMovement (float time) {
		delayTime = time;
	}
	void stopTankIfCollided (GameObject[] array) {
		if (circleResetDelay < 0) {
			previousCircledObject = null;
			circleResetDelay = 0;
		} 
		if (Vector3.Distance(originalPos, transform.position) > 0.5f && startingMove) {
			foreach (GameObject i in array) {
				if (i != null && move) {
					if (i.transform.position != transform.position) {
						rotateBody.transform.position = transform.position;
						rotateBody.transform.rotation = transform.rotation;

						rotateBody.transform.Translate (new Vector3(0.6f, 0, 0.8f), Space.Self);
						float p1 = Vector3.Distance(rotateBody.transform.position, i.transform.position);
						rotateBody.transform.Translate (new Vector3(-1.2f, 0, 0), Space.Self);
						float p2 = Vector3.Distance(rotateBody.transform.position, i.transform.position);
						rotateBody.transform.Translate (new Vector3(0, 0, -1.6f), Space.Self);
						float p3 = Vector3.Distance(rotateBody.transform.position, i.transform.position);
						rotateBody.transform.Translate (new Vector3(1.2f, 0, 0), Space.Self);
						float p4 = Vector3.Distance(rotateBody.transform.position, i.transform.position);


						bool a = true;
						if (previousCircledObject != null) {
							if (previousCircledObject == i) {
								a = false;
							}
						}
						if (p1 < 0.8f || p2 < 0.8f || p3 < 0.8f || p4 < 0.8f) {
							if (Vector3.Distance(finalDestination, transform.position) < 3) {
								move = false;
								rotate = false;
								delayTime = 0f;
								print ("cancelled");
								return;
							}
						}
						if ((p1 < 0.8f || p2 < 0.8f || p3 < 0.8f || p4 < 0.8f) && a) {
							if (i.GetComponent<ArmourScript> ().move/* && i.GetComponent<ArmourScript> ().delayTime <= 0*/) {
								if (i.GetComponent<ArmourScript> ().finalDestination == finalDestination) { 
									int myCount = pathFindPoints.Length - pathFindIndex;
									int otherCount = i.GetComponent<ArmourScript> ().pathFindPoints.Length - i.GetComponent<ArmourScript> ().pathFindIndex;
									if (otherCount < myCount) {
										delayMovement (0.8f);
									} else {
										i.GetComponent<ArmourScript>().moveTank (finalDestination.x, finalDestination.z, true);
										continue;
									}
								} else {
									delayMovement (0.8f);
								}

							} else/* if (i.GetComponent<ArmourScript> ().delayTime <= 0) */{
								circleResetDelay = 3;
								previousCircledObject = i;
								moveTank (finalDestination.x, finalDestination.z, true); 
							}
						} 
					}
				}
			}
		}
	}
	public void alignCoordinates () {
		if (transform.position.x % 1 > 0.5f) {
			transform.position = new Vector3 ((int)transform.position.x + 1, transform.position.y, transform.position.z);
		} else {
			transform.position = new Vector3((int)transform.position.x, transform.position.y, transform.position.z);
		}
		if (transform.position.z % 1 > 0.5f) {
			transform.position = new Vector3(transform.position.x, transform.position.y, (int)transform.position.z + 1);
		} else {
			transform.position = new Vector3(transform.position.x, transform.position.y, (int)transform.position.z);
		}
	}
	void Update () {
		if (masterController.GetComponent<ControllerScript>().gameEnded) {
			this.enabled = false;
			return;
		}
		if (circleResetDelay > 0) {
			circleResetDelay -= Time.deltaTime;
		}
		if (!isPlayer) {
			selected = false;
		}
		if (isPlayer && move) {
			stopTankIfCollided (masterController.GetComponent<ControllerScript> ().armourArray);
		} else if (move) {
			stopTankIfCollided (masterController.GetComponent<ControllerScript> ().enemyArmourArray);
		}
		bool d = false;
		bool b = false;
		if (target == null) {
			d = true;
		} else if (Vector3.Distance(target.transform.position, transform.position) > detectionRange) {
			b = true;
		}
		if (!isPlayer && (d || b) && attackPlayer && !move && !rotate) {
			startingMove = true;
			float minDistance = 100000f;
			Vector3 moveDestination = new Vector3(0, 0, 0);
			foreach (GameObject a in masterController.GetComponent<ControllerScript> ().myDefenceArray) {
				if (a != null) { 
					if (Vector3.Distance(a.transform.position, transform.position) < minDistance) {
						minDistance = Vector3.Distance(a.transform.position, transform.position);
						moveDestination = a.transform.position;
					}	
				}	
			}	
			moveTank (moveDestination.x, moveDestination.z, true);
		}	
		if (health <= 0) {	
			FindObjectOfType<AudioManagerScript>().Play("ArmourPierce", 0.4f);	
			if (isPlayer) {	
				masterController.GetComponent<ControllerScript>().removeObjectFromArmourArray(gameObject);	
			} else {	
				masterController.GetComponent<ControllerScript>().removeObjectFromEnemyArmourArray(gameObject);	
			}
			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
		if (selected) {
			selectedRing.GetComponent<Renderer>().enabled = true;
		} else {
			selectedRing.GetComponent<Renderer>().enabled = false;
		}
		if (delayTime <= 0) {
			if (rotate) {
				transform.rotation = Quaternion.Slerp (previousRotation, targetRotation, rotateCount); 
				rotateCount += Time.deltaTime * rotationSpeed / Quaternion.Angle (Quaternion.Slerp (previousRotation, targetRotation, 0), Quaternion.Slerp (previousRotation, targetRotation, 1f));
				if (rotateCount >= 1) {
					transform.rotation = targetRotation;
					rotateCount = 0;
					rotate = false;
					move = true;
					moveCount = 0;
				}
			} else if (move) {
				transform.Translate (Vector3.back * Time.deltaTime * speed);
				moveCount += Time.deltaTime * speed;
				if (moveCount >= Vector3.Distance (previousPosition, targetPosition)) {
					transform.position = targetPosition;
					pathFindIndex++;
					move = false;
					rotate = false;
					rotateCount = 0;
					moveCount = 0;
					if (pathFindIndex < pathFindPoints.Length) {
						moveTank (pathFindPoints [pathFindIndex].x, pathFindPoints [pathFindIndex].z, false);
					} else {
						print ("finished 1 grid");
						startingMove = true;
						previousCircledObject = null;
					}
				}
			}
		} else {
			delayTime -= Time.deltaTime;
		}
		shootDelay -= Time.deltaTime;
		if (canShoot && shootDelay <= 0) {
			FindObjectOfType<AudioManagerScript>().Play("ArtilleryFire", 0.5f);
			GameObject insItem = Instantiate(bulletPrefab, turret.transform.position, turret.transform.rotation) as GameObject; 
			insItem.GetComponent<BulletScript>().damage = damage; 
			insItem.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
			insItem.GetComponent<BulletScript>().target = target;
			shootDelay = shootFrequency; 
		}

		if (target == null) {
			canShoot = false; 
			if (Quaternion.Angle(turret.transform.rotation, transform.rotation) > 2f && !turretRotateBack) {
				turretRotateBack = true;
				previousTurretRotation = turret.transform.rotation;
				targetTurretRotation = transform.rotation; 
				turretRotateBackCount = 0;                 
			} else if (turretRotateBack && Quaternion.Angle(turret.transform.rotation, transform.rotation) > 2f) {
				turret.transform.rotation = Quaternion.Slerp(previousTurretRotation, targetTurretRotation, turretRotateBackCount);
				turretRotateBackCount += Time.deltaTime * rotationSpeed / Quaternion.Angle(previousTurretRotation, targetTurretRotation);
				if (turretRotateBackCount >= 1) {
					turretRotateBackCount = 0;
					turretRotateBack = false;
				}
			} else {
				turretRotate = false;
				turretRotateCount = 0;
			}
			float closestDistance = 1000;
			if (isPlayer) {
				for (int i = 0; i < masterController.GetComponent<ControllerScript>().defenceArray.Length; i++) {
					if (masterController.GetComponent<ControllerScript>().defenceArray[i] != null) {
						if (Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().defenceArray[i].gameObject.transform.position) <= detectionRange && Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().defenceArray[i].gameObject.transform.position) < closestDistance) {
							closestDistance = Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().defenceArray[i].gameObject.transform.position);
							target = masterController.GetComponent<ControllerScript>().defenceArray[i].gameObject;
						}
					}                                             
				}
				for (int i = 0; i < masterController.GetComponent<ControllerScript>().enemyArmourArray.Length; i++) {
					if (masterController.GetComponent<ControllerScript>().enemyArmourArray[i] != null) {
						if (Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().enemyArmourArray[i].gameObject.transform.position) <= detectionRange && Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().enemyArmourArray[i].gameObject.transform.position) < closestDistance) {
							closestDistance = Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().enemyArmourArray[i].gameObject.transform.position);
							target = masterController.GetComponent<ControllerScript>().enemyArmourArray[i].gameObject;
						}           
					}                                             
				}
			} else {
				for (int i = 0; i < masterController.GetComponent<ControllerScript>().myDefenceArray.Length; i++) {
					if (masterController.GetComponent<ControllerScript>().myDefenceArray[i] != null) {
						if (Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject.transform.position) <= detectionRange && Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject.transform.position) < closestDistance) {
							closestDistance = Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject.transform.position);
							target = masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject;
						}
					}                                             
				}
				for (int i = 0; i < masterController.GetComponent<ControllerScript>().armourArray.Length; i++) {
					if (masterController.GetComponent<ControllerScript>().armourArray[i] != null) {
						if (Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().armourArray[i].gameObject.transform.position) <= detectionRange && Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().armourArray[i].gameObject.transform.position) < closestDistance) {
							closestDistance = Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().armourArray[i].gameObject.transform.position);
							target = masterController.GetComponent<ControllerScript>().armourArray[i].gameObject;
						}
					}                                             
				}
			}
		} else if (Vector3.Distance(transform.position, target.transform.position) > detectionRange) {
			target = null;
		} else {
			if (!turretRotate) {
				previousTurretRotation = turret.transform.rotation;
				turret.transform.LookAt(target.transform.position);
				turret.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
				targetTurretRotation = turret.transform.rotation;
				turret.transform.rotation = previousTurretRotation; 
				turretRotate = true;
			} else {
				turret.transform.rotation = Quaternion.Slerp(previousTurretRotation, targetTurretRotation, turretRotateCount);
				turretRotateCount += Time.deltaTime * 130 / Quaternion.Angle(previousTurretRotation, targetTurretRotation);
				if (turretRotateCount >= 1) {
					canShoot = true;
					turretRotateCount = 0;
					turretRotate = false;
				} 
			}
		}
	}
	public void loseHealth (int amount) {
		FindObjectOfType<AudioManagerScript>().Play("HitSound", 0.4f);
		health -= amount; 
		transform.GetChild(5).transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2((float)health / (float)maxHealth, 0.2f); 
	}
	public bool moveTank (float x, float z, bool resetPathFind) {
		if (!resetPathFind) {
			rotateBody.transform.rotation = transform.rotation;
			rotateBody.transform.Rotate (new Vector3 (0, 180, 0), Space.Self);
			previousRotation = transform.rotation;
			previousPosition = transform.position;
			rotateBody.transform.position = transform.position;
			rotateBody.transform.rotation = transform.rotation;
			rotateBody.transform.LookAt (new Vector3 (x, 0, z));
			rotateBody.transform.Rotate (new Vector3 (0, 180, 0), Space.Self);
			targetRotation = rotateBody.transform.rotation;
			rotateCount = 0;
			moveCount = 0;

			targetPosition = new Vector3 (x, 0, z);

			rotateBody.transform.rotation = transform.rotation;
			previousPosition = new Vector3 ((int)previousPosition.x, (int)previousPosition.y, (int)previousPosition.z);
			rotate = true;
			return true;
		}
		if (resetPathFind) {
			print ("Moving to " + x + ", " + z);
			originalPos = transform.position;
			if (!isPlayer) {
				print ("enemy moving");
			}

			rotateBody.transform.position = transform.position;
			rotateBody.transform.rotation = transform.rotation;
			rotateBody.transform.LookAt(new Vector3(x, 0, z));
			rotateBody.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
			rotate = true;
			rotateCount = 0;
			moveCount = 0; 
			move = false;
			previousRotation = transform.rotation;
			previousPosition = transform.position;
			rotateBody.transform.rotation = transform.rotation;

			float x1 = 0;
			float y1 = 0;
			float z1 = 0;
			if (previousPosition.x % 1 > 0.5f) {
				x1 = 1;
			}
			if (previousPosition.z % 1 > 0.5f) {
				z1 = 1;
			}
			previousPosition = new Vector3 ((int)previousPosition.x + x1, (int)previousPosition.y + y1, (int)previousPosition.z + z1);
			pathFindIndex = 0;

			finalDestination = new Vector3(x, 0, z);

			Grid[] placedGrids = new Grid[5000];
			PathFindGridScript pathFindScript = masterController.GetComponent<PathFindGridScript> ();

			pathFindScript.gridArray [(int)(previousPosition.x + pathFindScript.gridSize.x / 2), (int)(previousPosition.z + pathFindScript.gridSize.y / 2)].isStart = true;
			pathFindScript.gridArray [(int)(previousPosition.x + pathFindScript.gridSize.x / 2), (int)(previousPosition.z + pathFindScript.gridSize.y / 2)].placed = true;
			placedGrids [0] = pathFindScript.gridArray [(int)(previousPosition.x + pathFindScript.gridSize.x / 2), (int)(previousPosition.z + pathFindScript.gridSize.y / 2)];

			float minWeight = 1000000;  
			int count = 0;
			while (count < 9000) {
				if (count >= 8999) {
					return false;
				}
				if (!pathFindScript.gridArray [(int)(x + pathFindScript.gridSize.x / 2), (int)(z + pathFindScript.gridSize.y / 2)].placable && (isPlayer || !startingMove)) {
					rotate = false;
					print ("cannot move");
					break;
				}
				count++;
				Grid lightestWeightGrid = null;
				int prevGridIndex = 1;
				for (int i = 0; i < placedGrids.Length; i++) {
					if (placedGrids [i] != null) {
						if (!placedGrids [i].closed) {
							bool close = true;
							for (int j = 0; j < 8; j++) {
								int a = (int)placedGrids[i].arrayIndex.x;
								int b = (int)placedGrids[i].arrayIndex.y;

								if (j == 0) {
									a -= 1;
								}
								if (j == 1) {
									a -= 1;
									b -= 1;
								}
								if (j == 2) {
									a -= 1;
									b += 1;
								}
								if (j == 3) {
									b -= 1;
								}
								if (j == 4) {   
									b += 1;
								}
								if (j == 5) {  
									a += 1;
									b -= 1;
								}
								if (j == 6) { 
									a += 1;
									b += 1;
								}
								if (j == 7) {
									a += 1;
								}
								bool originatedFromSelf = false;
								if (pathFindScript.gridArray [a, b].originateFrom == gameObject) {
									originatedFromSelf = true;
								}
								if ((pathFindScript.gridArray [a, b].placable && !pathFindScript.gridArray[a, b].placed) || originatedFromSelf && !pathFindScript.gridArray[a, b].placed) {
									close = false;
									int weight = calculateWeight(new Vector2(finalDestination.x, finalDestination.z), pathFindScript.gridArray [a, b].position) + calculateWeight (new Vector2(transform.position.x, transform.position.z), pathFindScript.gridArray [a, b].position);
									if (weight < minWeight) {
										lightestWeightGrid = pathFindScript.gridArray [a, b];
										prevGridIndex = i;
										minWeight = weight;
									}
								}
							}
							if (close) {
								placedGrids [i].closed = true;
							}
						}
					} else {
						break;
					}
				}
				if (lightestWeightGrid != null) {
					lightestWeightGrid.previousGrid = placedGrids [prevGridIndex];
					lightestWeightGrid.placed = true;
					for (int c = 0; c < placedGrids.Length; c++) {
						if (placedGrids [c] == null) {
							placedGrids [c] = lightestWeightGrid;
							minWeight = 100000;
							break;
						}
					}
					if (Vector2.Distance (lightestWeightGrid.position, new Vector2 (finalDestination.x, finalDestination.z)) < 1 || (!isPlayer && startingMove && Vector2.Distance (lightestWeightGrid.position, new Vector2 (finalDestination.x, finalDestination.z)) < detectionRange - 1)) {
						Grid lastGrid = lightestWeightGrid;
						int count1 = 0;
						while (lastGrid != null) {
							count1++;
							lastGrid = lastGrid.previousGrid;
						}
						pathFindPoints = new Vector3[count1];

						int count2 = 0;
						Grid lastGrid1 = lightestWeightGrid;
						while (lastGrid1 != null) {
							pathFindPoints[pathFindPoints.Length - count2 - 1] = new Vector3(lastGrid1.position.x, 0, lastGrid1.position.y);
							if (count2 >= 2) {
								if (Mathf.Abs(pathFindPoints[pathFindPoints.Length - count2 - 1].x - pathFindPoints[pathFindPoints.Length - count2].x) == 1 && pathFindPoints[pathFindPoints.Length - count2 - 1].x == pathFindPoints[pathFindPoints.Length - count2 + 1].x) {
									pathFindPoints [pathFindPoints.Length - count2].x = pathFindPoints [pathFindPoints.Length - count2 - 1].x;
								}
								if (Mathf.Abs(pathFindPoints[pathFindPoints.Length - count2 - 1].z - pathFindPoints[pathFindPoints.Length - count2].z) == 1 && pathFindPoints[pathFindPoints.Length - count2 - 1].z == pathFindPoints[pathFindPoints.Length - count2 + 1].z) {
									pathFindPoints [pathFindPoints.Length - count2].z = pathFindPoints [pathFindPoints.Length - count2 - 1].z;
								}
							}

							count2++;
							lastGrid1 = lastGrid1.previousGrid;
						}
						targetPosition = pathFindPoints [pathFindIndex];
						rotateBody.transform.LookAt(new Vector3(x, 0, z));
						rotateBody.transform.Rotate(new Vector3(0, 180, 0), Space.Self);
						targetRotation = rotateBody.transform.rotation;
						pathFindScript.clearGridArray (placedGrids);
						print (count);
						return true;
					}
					lightestWeightGrid = null;
				} else {
					print ("cannot find solution");
					rotate = false;
					break;
				}
			}
			pathFindScript.clearGridArray (placedGrids);
		}
		return false;
	}
	int calculateWeight (Vector2 targetPos, Vector2 gridPos) {
		int weight = 0;
		bool xIsLarger;
		if (Mathf.Abs (targetPos.x - gridPos.x) > Mathf.Abs (targetPos.y - gridPos.y)) {
			xIsLarger = true;
		} else {
			xIsLarger = false;
		}
		if (xIsLarger) {
			weight = (int)((Mathf.Abs (targetPos.x - gridPos.x) - (Mathf.Abs (targetPos.x - gridPos.x) - Mathf.Abs (targetPos.y - gridPos.y))) * 14 + (Mathf.Abs (targetPos.x - gridPos.x) - Mathf.Abs (targetPos.y - gridPos.y)) * 10);
		} else {
			weight = (int)((Mathf.Abs (targetPos.y - gridPos.y) - (Mathf.Abs (targetPos.y - gridPos.y) - Mathf.Abs (targetPos.x - gridPos.x))) * 14 + (Mathf.Abs (targetPos.y - gridPos.y) - Mathf.Abs (targetPos.x - gridPos.x)) * 10);
		}
		return weight;
	}
}
