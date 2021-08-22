using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenceScript : MonoBehaviour {
	public float sizeX;
	public float sizeZ;
	public bool isWeapon;
	public bool isHeadquarters;
	public bool isPlayer;
	public bool isFactory;
	public bool isGoldMine;
	public bool isNatural;
	public bool isBarrier;
	public bool isGround;
	public bool isGoldReady; 
	public float buildingCost; 
	public float goldReadyTimer;
	public float goldCollected = 0;
	public bool stopGoldProduction;
	public float radius = 10f;

	public GameObject explosionPrefab;
	public GameObject masterController;
	public GameObject bulletPrefab;
	public int damage;
	public float health; 

	public float enemySpawnTimer;
	GameObject spawnedEnemy;

	float maxHealth;
	bool moveGunBack;
	float amountMoved;
	public float detectionRange;
	public float shootFrequency;
	public GameObject target; 
	GameObject turretBody;
	float shootTimeDelay;

	public float playerTankBuildCooldown;
	public float playerTankBuildTimer;

	public float playerDoubleTankBuildCooldown;
	public float playerDoubleTankBuildTimer;

	public float playerArmouredCarBuildCooldown;
	public float playerArmouredCarBuildTimer;

	string currentArmourName = "tank";
	GameObject currentArmourObject;

	void Start () {
		masterController = GameObject.Find ("MasterController");
		enemySpawnTimer = playerTankBuildCooldown;

		currentArmourObject = masterController.GetComponent<ControllerScript> ().tankPrefab;

		if (isHeadquarters) {
			buildingCost = masterController.GetComponent<ControllerScript> ().findBuildingCostWithName ("headquarters");
		}
		if (isGoldMine) {
			buildingCost = masterController.GetComponent<ControllerScript> ().findBuildingCostWithName ("goldMine");
		}
		if (isNatural) {
			if (goldReadyTimer >= 590) {
				masterController.GetComponent<ControllerScript> ().addObjectToNaturalResourcesArray (gameObject);
				isGoldReady = false;
				goldReadyTimer = 600;
			} else {
				masterController.GetComponent<ControllerScript> ().addObjectToNaturalResourcesArray (gameObject);
			}
		}

		if (!isGoldReady && isNatural) {
			for (int i = 0; i <= 12; i++) {
				transform.GetChild (i).transform.Translate (new Vector3(0, -0.4f, 0), Space.Self);
			}
		}
		if (!isBarrier) {
			if (transform.position.x % (int)transform.position.x >= 0.5f) {
				transform.position = new Vector3 ((int)transform.position.x + 1, transform.position.y, transform.position.z);
			} else {
				transform.position = new Vector3 ((int)transform.position.x, transform.position.y, transform.position.z);
			}
			if (transform.position.z % (int)transform.position.z >= 0.5f) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, (int)transform.position.z + 1);
			} else {
				transform.position = new Vector3 (transform.position.x, transform.position.y, (int)transform.position.z);
			}
			playerTankBuildTimer = playerTankBuildCooldown; 
			playerDoubleTankBuildTimer = playerDoubleTankBuildCooldown;
			playerArmouredCarBuildTimer = playerArmouredCarBuildCooldown;
			transform.Translate(new Vector3(0, -1.5f, 0), Space.Self);
			maxHealth = health;
			shootTimeDelay = shootFrequency;
			if (isWeapon) {
				turretBody = transform.GetChild(0).gameObject;
			}
		}
	}
	public void loseHealth (int amount) {
		FindObjectOfType<AudioManagerScript>().Play("HitSound", 0.4f);
		health -= amount;
		transform.GetChild(2).transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2((float)health / (float)maxHealth, 0.2f); 
	}
	void Update () {
		if (!isPlayer && stopGoldProduction && isGoldMine) {
			health = 0;
			masterController.GetComponent<ControllerScript>().enemyCoins += buildingCost / 3f;
		}
		if (!isGoldReady && isNatural) {
			goldReadyTimer -= Time.deltaTime;
		}
		if (!isGoldReady && goldReadyTimer <= 0 && isNatural) {
			isGoldReady = true;
			for (int i = 0; i <= 12; i++) {
				transform.GetChild (i).transform.Translate (new Vector3(0, 0.4f, 0), Space.Self);
			}

			goldReadyTimer = 100;
		}
		if (playerTankBuildTimer > 0) {
			playerTankBuildTimer -= Time.deltaTime;
		}
		if (playerTankBuildTimer < 0) {
			playerTankBuildTimer = 0;
		}
		if (playerDoubleTankBuildTimer > 0) {
			playerDoubleTankBuildTimer -= Time.deltaTime;
		}
		if (playerDoubleTankBuildTimer < 0) {
			playerDoubleTankBuildTimer = 0;
		}
		if (playerArmouredCarBuildTimer > 0) {
			playerArmouredCarBuildTimer -= Time.deltaTime;
		}
		if (playerArmouredCarBuildTimer < 0) {
			playerArmouredCarBuildTimer = 0;
		}

		if (!isPlayer && isFactory) {
			enemySpawnTimer -= Time.deltaTime;
			if (enemySpawnTimer <= 0 && masterController.GetComponent<ControllerScript> ().enemyCoins >= masterController.GetComponent<ControllerScript>().findBuildingCostWithName(currentArmourName) && masterController.GetComponent<ControllerScript>().currentEnemyBuilding != "goldMine" && masterController.GetComponent<ControllerScript>().prioritizeTankBuilding) {
				if (spawnedEnemy == null || Vector3.Distance(spawnedEnemy.transform.position, transform.position) > 1) {
					GameObject insItem = masterController.GetComponent<ControllerScript> ().addObjectToEnemyArmourArray(currentArmourObject, transform.position);
					spawnedEnemy = insItem;
				}
				masterController.GetComponent<ControllerScript> ().enemyCoins -= masterController.GetComponent<ControllerScript> ().findBuildingCostWithName (currentArmourName);
				int rand = Random.Range (0, 2);
				if (rand == 0) {
					enemySpawnTimer = playerTankBuildCooldown;
					currentArmourName = "tank";
					currentArmourObject = masterController.GetComponent<ControllerScript> ().tankPrefab;
				} else if (rand == 1) {
					enemySpawnTimer = playerDoubleTankBuildCooldown;
					currentArmourName = "doubleTank";
				} else if (rand == 2) {
					enemySpawnTimer = playerArmouredCarBuildCooldown;
					currentArmourName = "armouredCar";
				}
			}
		}
		if (isGoldMine && !stopGoldProduction) {
			if (isPlayer) {
				masterController.GetComponent<ControllerScript> ().playerCoins += 5f * Time.deltaTime;
				goldCollected += 5f * Time.deltaTime;
			} else {
				masterController.GetComponent<ControllerScript> ().enemyCoins += (PlayerPrefs.GetInt("difficulty")) * Time.deltaTime;
				goldCollected += 5f * Time.deltaTime;
			}
		}
		if (goldCollected > 1500) {
			stopGoldProduction = true;
			transform.GetChild (24).GetComponent<Renderer>().materials[0].color = new Color32(255, 0, 0, 255);
			for (int i = 6; i <= 21; i++) {
				transform.GetChild (i).GetComponent<Renderer> ().enabled = false;
			}
		}
		if (!isBarrier) {
			if (isNatural) {
				if (transform.position.y < -0.5f) {
					transform.Translate(new Vector3(0, Time.deltaTime * 2.5f, 0), Space.Self);
				}
				if (transform.position.y > -0.5f) {
					transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);
				}
			} else {
				if (transform.position.y < 0) {
					transform.Translate(new Vector3(0, Time.deltaTime * 2.5f, 0), Space.Self);
				}
				if (transform.position.y > 0) {
					transform.position = new Vector3(transform.position.x, 0, transform.position.z);
				}
			}
		}

		if (health <= 0) {
			FindObjectOfType<AudioManagerScript>().Play("ArmourPierce", 0.4f);
			if (!isPlayer) {
				masterController.GetComponent<ControllerScript>().removeObjectFromDefenceArray(gameObject); 
				Instantiate(explosionPrefab, transform.position, Quaternion.identity);
				if (isHeadquarters) {
					int headQuartersAmount = 0;
					for (int i = 0; i < masterController.GetComponent<ControllerScript> ().defenceArray.Length; i++) {
						if (masterController.GetComponent<ControllerScript> ().defenceArray [i] != null) {
							if (masterController.GetComponent<ControllerScript> ().defenceArray [i].GetComponent<DefenceScript> ().isHeadquarters) {
								headQuartersAmount++;
							}
						}
					}
					if (headQuartersAmount <= 1) {
						for (int i = 0; i < masterController.GetComponent<ControllerScript> ().defenceArray.Length; i++) {
							if (masterController.GetComponent<ControllerScript> ().defenceArray [i] != null) {
								masterController.GetComponent<ControllerScript> ().defenceArray [i].GetComponent<DefenceScript> ().health = 0;
							}
						}
						masterController.GetComponent<ControllerScript> ().endGame (true, 3.7f);
					}
				} else if (isGoldMine) {
					GameObject insItem = Instantiate (masterController.GetComponent<ControllerScript>().goldDepositPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity) as GameObject;
					masterController.GetComponent<ControllerScript> ().addObjectToNaturalResourcesArray (insItem);
					insItem.GetComponent<DefenceScript> ().isGoldReady = false;
					insItem.GetComponent<DefenceScript> ().goldReadyTimer = 600f;
				}
			} else {
				int headQuartersAmount = 0;
				for (int i = 0; i < masterController.GetComponent<ControllerScript>().myDefenceArray.Length; i++) {
					if (masterController.GetComponent<ControllerScript>().myDefenceArray[i] != null) {
						if (masterController.GetComponent<ControllerScript>().myDefenceArray[i].GetComponent<DefenceScript>().isHeadquarters) {
							headQuartersAmount++;
						}
					}
				}
				masterController.GetComponent<ControllerScript>().removeObjectFromMyDefenceArray(gameObject);
				Instantiate(explosionPrefab, transform.position, Quaternion.identity);
				if (headQuartersAmount <= 1) {
					if (isHeadquarters) {
						for (int i = 0; i < masterController.GetComponent<ControllerScript>().myDefenceArray.Length; i++) {
							if (masterController.GetComponent<ControllerScript>().myDefenceArray[i] != null) {
								masterController.GetComponent<ControllerScript>().myDefenceArray[i].GetComponent<DefenceScript>().health = 0;
							}
						}
						masterController.GetComponent<ControllerScript> ().endGame (false, 3.7f);
					}
				}
				if (isGoldMine) {
					GameObject insItem = Instantiate (masterController.GetComponent<ControllerScript>().goldDepositPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity) as GameObject;
					masterController.GetComponent<ControllerScript> ().addObjectToNaturalResourcesArray (insItem);
					insItem.GetComponent<DefenceScript> ().isGoldReady = false;
					insItem.GetComponent<DefenceScript> ().goldReadyTimer = 600f;
				}
			}
			Destroy(gameObject);
		}
		if (isWeapon) {
			if (moveGunBack) {
				turretBody.transform.GetChild(0).gameObject.transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
				amountMoved += Time.deltaTime;
				if (amountMoved >= 0.5f) {
					turretBody.transform.GetChild(0).gameObject.transform.Translate(new Vector3(0, 0, -(amountMoved - 0.5f)), Space.Self);
					amountMoved = 0;
					moveGunBack = false;
				}
			}
			shootTimeDelay -= Time.deltaTime;
			if (target == null) {
				float closestDistance = 1000;
				if (!isPlayer) {
					for (int i = 0; i < masterController.GetComponent<ControllerScript>().armourArray.Length; i++) {
						if (masterController.GetComponent<ControllerScript>().armourArray[i] != null) {
							if (Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().armourArray[i].gameObject.transform.position) <= detectionRange && Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().armourArray[i].gameObject.transform.position) < closestDistance) {
								closestDistance = Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().armourArray[i].gameObject.transform.position);
								target = masterController.GetComponent<ControllerScript>().armourArray[i].gameObject;
							}
						}
					}
					for (int i = 0; i < masterController.GetComponent<ControllerScript>().myDefenceArray.Length; i++) {
						if (masterController.GetComponent<ControllerScript>().myDefenceArray[i] != null) {
							if (Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject.transform.position) <= detectionRange && Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject.transform.position) < closestDistance) {
								closestDistance = Vector3.Distance(transform.position, masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject.transform.position);
								target = masterController.GetComponent<ControllerScript>().myDefenceArray[i].gameObject;
							}
						}
					}
				} else {
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
				}
			} else if (Vector3.Distance(transform.position, target.transform.position) > detectionRange) {
				target = null;
			} else {
				turretBody.transform.LookAt(target.transform.position);
				if (shootTimeDelay <= 0) {
					shootTimeDelay = shootFrequency;
					shootBullet();
				}
			}
		}
		
	}
	void shootBullet () {
		FindObjectOfType<AudioManagerScript>().Play("ArtilleryFire", 0.5f);
		GameObject insItem = Instantiate(bulletPrefab, turretBody.transform.position, turretBody.transform.rotation) as GameObject;
		insItem.GetComponent<BulletScript>().damage = damage;
		insItem.GetComponent<BulletScript>().target = target;
		turretBody.transform.GetChild(0).gameObject.transform.Translate(Vector3.back * 0.5f, Space.Self);
		moveGunBack = true;
	}
}