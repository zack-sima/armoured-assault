using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Cost {
	public float cost;  
	public string name; 
	public Cost (float cost1, string name1) {
		cost = cost1;
		name = name1;
	}
}
public class ControllerScript : MonoBehaviour {
	public int map;
	public GameObject endGameUI;

	public GameObject visualSpherePrefab;
	public bool dragUnits;
	public bool sellBuildings;
	public bool touchContainsUI; 
	public GameObject constructionHover;
	public GameObject[] selectedArmour;
	public GameObject[] armourArray; 
	public GameObject[] enemyArmourArray;
	public GameObject[] defenceArray; 
	public GameObject[] myDefenceArray;
	public GameObject[] naturalResourcesArray;

	public GameObject map1Prefab;
	public GameObject map2Prefab;
	public GameObject map3Prefab;

	public GameObject mainCamera;
	public GameObject tankPrefab; 
	public GameObject doubleTankPrefab; 
	public GameObject armouredCarPrefab; 
	public GameObject turretPrefab;
	public GameObject bigTurretPrefab;
	public GameObject commandCenterPrefab;    
	public GameObject factoryPrefab;
	public GameObject goldDepositPrefab;
	public GameObject goldMinePrefab;
	public GameObject groundPrefab;
	public GameObject riverPrefab;

	public float playerCoins;
	public float enemyCoins;
	public string currentEnemyBuilding; 
	public Text playerCoinsDisplay; 
	public Cost[] prefabCosts; 
	public Cost[] enemyPrefabCosts; 
	public Cost[] originalPrefabCosts;

	public bool factorySelected;

	public bool stackSelectedArmour;

	public GameObject[] armourGroup1;
	public GameObject[] armourGroup2;
	public GameObject[] armourGroup3;
	public GameObject[] armourGroup4;
	public GameObject[] armourGroup5;
	public GameObject[] armourGroup6;
	public GameObject[] armourGroup7;
	public GameObject[] armourGroup8;
	public GameObject[] armourGroup9; 

	public GameObject[] naturalBarriersArray; 

	public GameObject selectedFactory;
	public GameObject buyTankButton;
	public GameObject buyFactoryButton;
	public GameObject buyTurretButton;
	public GameObject buyGoldMineButton;
	public GameObject buyHeadquartersButton;
	public GameObject selectAllUnitsButton;
	public GameObject dragUnitsButton;
	public GameObject sellBuildingsButton;
	public GameObject deselectUnitsButton;
	public GameObject goldDisplay;
	public GameObject stopMoveButton;
	public GameObject buyDoubleTankButton;

	public float playerFactoryBuildCooldown;
	public float playerFactoryBuildTimer;
	public float playerGoldMineBuildCooldown;
	public float playerGoldMineBuildTimer;
	public float playerTurretBuildCooldown;
	public float playerTurretBuildTimer;
	public float playerHeadquartersBuildCooldown;
	public float playerHeadquartersBuildTimer;
	bool justConstructed = false;

	public Vector3[] mapBorders;

	public float naturalResourcesGenerateSize = 80;
	public float naturalResourcesGenerateScale = 20f;

	public float enemyAttackCooldown;
	public float enemyAttackTime;

	public bool prioritizeTankBuilding;

	public bool isConstructing; 
	public GameObject constructionItemPrefab;

	public Vector3 dragStartPos;

	RectTransform r1;
	RectTransform r2;
	RectTransform r3;
	RectTransform r4;
	RectTransform r5;
	RectTransform r6;
	RectTransform r7;
	RectTransform r8;
	RectTransform r10;
	RectTransform r11;
	RectTransform r12;
	public Rect rect1;
	public Rect rect2;
	public Rect rect3;
	public Rect rect4;
	public Rect rect5;
	public Rect rect6;
	public Rect rect7;
	public Rect rect8;
	public Rect rect10;
	public Rect rect11;
	public Rect rect12;

	public Vector3 touchDownPosition;
	public Vector3 touchUpPosition;

	float endGameDelay = 0f;
	public bool gameEnded = false;
	bool playerWon = false;

	public Vector2 loadBarOriginalSize;

    public void returnToMenu () {
		Application.LoadLevel (0);
	}
	public void dragUnitsToggle () {
		dragUnits = !dragUnits;
	}
	public void sellBuildingsToggle () {
		sellBuildings = !sellBuildings;
	}
	public void stopTanks () {
		foreach (GameObject i in selectedArmour) {
			if (i != null) {
				i.GetComponent<ArmourScript> ().move = false;
				i.GetComponent<ArmourScript> ().rotate = false;
				i.GetComponent<ArmourScript> ().delayTime = 0f;
			}
		}
	}
	void updateButtonsText () {
		if (PlayerPrefs.GetString("language") == "chinese") {
			buyTankButton.transform.GetChild (0).GetComponent<Text> ().text = "坦克 (" + prefabCosts [0].cost.ToString () + "金币)";
			buyDoubleTankButton.transform.GetChild (0).GetComponent<Text> ().text = "双炮坦克 (" + prefabCosts [6].cost.ToString () + "金币)";
			buyTurretButton.transform.GetChild (0).GetComponent<Text> ().text = "炮塔 (" + prefabCosts [1].cost.ToString () + "金币)";
			buyFactoryButton.transform.GetChild (0).GetComponent<Text> ().text = "工厂 (" + prefabCosts [2].cost.ToString () + "金币)";
			buyGoldMineButton.transform.GetChild (0).GetComponent<Text> ().text = "金矿 (" + prefabCosts [3].cost.ToString () + "金币)";
			buyHeadquartersButton.transform.GetChild (0).GetComponent<Text> ().text = "大本营 (" + prefabCosts [5].cost.ToString () + "金币)";
		} else {
			buyTankButton.transform.GetChild(0).GetComponent<Text>().text = "Tank (" + prefabCosts[0].cost.ToString() + " Gold)";
			buyDoubleTankButton.transform.GetChild(0).GetComponent<Text>().text = "Double Tank (" + prefabCosts[6].cost.ToString() + " Gold)";
			buyTurretButton.transform.GetChild(0).GetComponent<Text>().text = "Turret (" + prefabCosts[1].cost.ToString() + " Gold)";
			buyFactoryButton.transform.GetChild(0).GetComponent<Text>().text = "Factory (" + prefabCosts[2].cost.ToString() + " Gold)";
			buyGoldMineButton.transform.GetChild(0).GetComponent<Text>().text = "Gold Mine (" + prefabCosts[3].cost.ToString() + " Gold)";
			buyHeadquartersButton.transform.GetChild (0).GetComponent<Text> ().text = "HQ (" + prefabCosts [5].cost.ToString () + " Gold)";
		}
	}
	void dontShowDeselectUnitsButton () {
		deselectUnitsButton.GetComponent<Image>().enabled = false;
		deselectUnitsButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
	}
	void showStopMoveButton () {
		stopMoveButton.GetComponent<Image>().enabled = true;
		stopMoveButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
	}
	void dontShowStopMoveButton () {
		stopMoveButton.GetComponent<Image>().enabled = false;
		stopMoveButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
	}
	void showDeselectUnitsButton () {
		deselectUnitsButton.GetComponent<Image>().enabled = true;
		deselectUnitsButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
	}
	public void cancelConstruction () {
		isConstructing = false;
	}
	public void endGame (bool playerWin, float delay) {
		playerWon = playerWin;
		endGameDelay = delay;
		gameEnded = true;
	}
	void Start () {
		loadBarOriginalSize = buyTurretButton.transform.GetChild (1).GetComponent<RectTransform> ().sizeDelta;
		r1 = dragUnitsButton.GetComponent<RectTransform> ();
		r2 = selectAllUnitsButton.GetComponent<RectTransform> ();
		r3 = buyFactoryButton.GetComponent<RectTransform> ();
		r4 = buyGoldMineButton.GetComponent<RectTransform> ();
		r5 = buyTankButton.GetComponent<RectTransform> ();
		r6 = buyTurretButton.GetComponent<RectTransform> ();
		r7 = deselectUnitsButton.GetComponent<RectTransform> ();
		r8 = buyHeadquartersButton.GetComponent<RectTransform> ();
		r10 = sellBuildingsButton.GetComponent<RectTransform> ();
		r11 = stopMoveButton.GetComponent<RectTransform> ();
		r12 = buyDoubleTankButton.GetComponent<RectTransform> ();

		rect1 = new Rect (new Vector2 (r1.position.x - r1.rect.width / 2, r1.position.y - r1.rect.height / 2), new Vector2 (r1.rect.width, r1.rect.height));
		rect2 = new Rect (new Vector2 (r2.position.x - r2.rect.width / 2, r2.position.y - r2.rect.height / 2), new Vector2 (r2.rect.width, r2.rect.height));
		rect3 = new Rect (new Vector2 (r3.position.x - r3.rect.width / 2, r3.position.y - r3.rect.height / 2), new Vector2 (r3.rect.width, r3.rect.height));
		rect4 = new Rect (new Vector2 (r4.position.x - r4.rect.width / 2, r4.position.y - r4.rect.height / 2), new Vector2 (r4.rect.width, r4.rect.height));
		rect5 = new Rect (new Vector2 (r5.position.x - r5.rect.width / 2, r5.position.y - r5.rect.height / 2), new Vector2 (r5.rect.width, r5.rect.height));
		rect6 = new Rect (new Vector2 (r6.position.x - r6.rect.width / 2, r6.position.y - r6.rect.height / 2), new Vector2 (r6.rect.width, r6.rect.height));
		rect7 = new Rect (new Vector2 (r7.position.x - r7.rect.width / 2, r7.position.y - r7.rect.height / 2), new Vector2 (r7.rect.width, r7.rect.height));
		rect8 = new Rect (new Vector2 (r8.position.x - r8.rect.width / 2, r8.position.y - r8.rect.height / 2), new Vector2 (r8.rect.width, r8.rect.height));
		rect10 = new Rect (new Vector2 (r10.position.x - r10.rect.width / 2, r10.position.y - r10.rect.height / 2), new Vector2 (r10.rect.width, r10.rect.height));
		rect11 = new Rect (new Vector2 (r11.position.x - r11.rect.width / 2, r11.position.y - r11.rect.height / 2), new Vector2 (r11.rect.width, r11.rect.height));
		rect12 = new Rect (new Vector2 (r12.position.x - r12.rect.width / 2, r12.position.y - r12.rect.height / 2), new Vector2 (r12.rect.width, r12.rect.height));
	}
	void Awake () { 
		mapBorders = new Vector3[4];
		map = PlayerPrefs.GetInt ("map");
		currentEnemyBuilding = "goldMine";

		dontShowDeselectUnitsButton ();


		naturalResourcesGenerateScale *= Random.Range(0.9f, 1.1f);

		playerFactoryBuildTimer = playerFactoryBuildCooldown;
		playerGoldMineBuildTimer = playerGoldMineBuildCooldown;
		playerTurretBuildTimer = playerTurretBuildCooldown;
		playerHeadquartersBuildTimer = playerHeadquartersBuildCooldown;

		playerCoins = 2150 - PlayerPrefs.GetInt ("difficulty") * 150;
		enemyCoins = PlayerPrefs.GetInt ("difficulty") * 150;

		prefabCosts = new Cost[8];
		originalPrefabCosts = new Cost[prefabCosts.Length];
		enemyPrefabCosts = new Cost[prefabCosts.Length];
		prefabCosts[0] = new Cost(200, "tank");
		prefabCosts[1] = new Cost(160, "turret");
		prefabCosts[2] = new Cost(700, "factory");
		prefabCosts[3] = new Cost(400, "goldMine");
		prefabCosts[4] = new Cost(280, "bigTurret");
		prefabCosts[5] = new Cost(3000, "headquarters");
		prefabCosts[6] = new Cost(350, "doubleTank");
		prefabCosts[7] = new Cost(130, "armouredCar");

		for (int i = 0; i < prefabCosts.Length; i++) {
			originalPrefabCosts[i] = new Cost(prefabCosts[i].cost, prefabCosts[i].name);
			enemyPrefabCosts[i] = new Cost(prefabCosts[i].cost, prefabCosts[i].name);
		}
		updateButtonsText();

		naturalResourcesArray = new GameObject[(int)naturalResourcesGenerateSize * (int)naturalResourcesGenerateSize / 20];

		enemyArmourArray = new GameObject[300];
		armourGroup1 = new GameObject[300];
		armourGroup2 = new GameObject[300];
		armourGroup3 = new GameObject[300];
		armourGroup4 = new GameObject[300];     
		armourGroup5 = new GameObject[300];
		armourGroup6 = new GameObject[300];
		armourGroup7 = new GameObject[300];
		armourGroup8 = new GameObject[300];
		armourGroup9 = new GameObject[300];
		selectedArmour = new GameObject[300];
		armourArray = new GameObject[300];
		defenceArray = new GameObject[300];
		myDefenceArray = new GameObject[300];
		naturalBarriersArray = new GameObject[(int)naturalResourcesGenerateSize * (int)naturalResourcesGenerateSize / 20]; 
		GameObject insMap = null;
		if (map == 1) {
			insMap = Instantiate (map1Prefab, map1Prefab.transform.position, map1Prefab.transform.rotation) as GameObject;
			addObjectToDefenceArray (commandCenterPrefab, new Vector3(-45, 0, -66), findBuildingCostWithName("headquarters"));
			addObjectToMyDefenceArray (commandCenterPrefab, new Vector3(56, 0, 66), findBuildingCostWithName("headquarters"));
		} else if (map == 2) {
			insMap = Instantiate (map2Prefab, map2Prefab.transform.position, map2Prefab.transform.rotation) as GameObject;
			addObjectToDefenceArray (commandCenterPrefab, new Vector3(-30, 0, -40), findBuildingCostWithName("headquarters"));
			addObjectToMyDefenceArray (commandCenterPrefab, new Vector3(50, 0, 40), findBuildingCostWithName("headquarters"));
		} else {
			insMap = Instantiate (map3Prefab, map3Prefab.transform.position, map3Prefab.transform.rotation) as GameObject;
			addObjectToDefenceArray (commandCenterPrefab, new Vector3(-9, 0, -62), findBuildingCostWithName("headquarters"));
			addObjectToMyDefenceArray (commandCenterPrefab, new Vector3(-9, 0, 30), findBuildingCostWithName("headquarters"));
		}
		for (int i = 0; i < 4; i++) {
			mapBorders[i] = insMap.transform.GetChild(i).position;
		}
	}
	public void constructItem (GameObject prefab) {
		constructionItemPrefab = prefab; 
		isConstructing = true; 
	}
	public void removeObjectFromArmourArray (GameObject item) {
		for (int i = 0; i < armourArray.Length; i++) { 
			if (armourArray[i] == item) { 
				armourArray[i] = null;
				break;
			}
		}
	}
	public void removeObjectFromEnemyArmourArray (GameObject item) {
		for (int i = 0; i < enemyArmourArray.Length; i++) { 
			if (enemyArmourArray[i] == item) { 
				enemyArmourArray[i] = null;
				break;
			}
		}
	}
	public void removeObjectFromDefenceArray (GameObject item) {
		for (int i = 0; i < defenceArray.Length; i++) {
			if (defenceArray[i] == item) {
				defenceArray[i] = null;
				break;
			}
		}
	}
	public void removeObjectFromMyDefenceArray (GameObject item) {
		for (int i = 0; i < myDefenceArray.Length; i++) {
			if (myDefenceArray[i] == item) {
				myDefenceArray[i] = null;
				break;
			}
		}
	}
	void addObjectToArmourArray (GameObject insItem, Vector3 position) {
		GameObject item = Instantiate(insItem, position, insItem.transform.rotation) as GameObject;
		item.GetComponent<ArmourScript>().masterController = gameObject; 
		item.transform.GetChild(5).GetComponent<HealthDisplayScript>().mainCamera = mainCamera; 
		item.GetComponent<ArmourScript>().isPlayer = true;
		GameObject glowSphere = Instantiate (visualSpherePrefab, new Vector3(position.x, 3.5f, position.z), Quaternion.identity) as GameObject;
		glowSphere.transform.SetParent (item.transform);
		for (int i = 0; i < armourArray.Length; i++) {
			if (armourArray[i] == null) {
				armourArray[i] = item;
				break;
			}
		}
	}
	public GameObject addObjectToEnemyArmourArray (GameObject insItem, Vector3 position) {
		GameObject item = Instantiate(insItem, position, insItem.transform.rotation) as GameObject;
		item.GetComponent<ArmourScript>().masterController = gameObject; 
		item.transform.GetChild(5).GetComponent<HealthDisplayScript>().mainCamera = mainCamera;
		for (int i = 0; i < enemyArmourArray.Length; i++) {
			if (enemyArmourArray[i] == null) {
				enemyArmourArray[i] = item;
				break;
			}
		}
		return item;
	}
	public void addObjectToNaturalResourcesArray (GameObject insItem) {
		insItem.GetComponent<DefenceScript> ().isGoldReady = true;
		for (int a = 0; a < naturalResourcesArray.Length; a++) {
			if (naturalResourcesArray[a] == null) {
				naturalResourcesArray[a] = insItem;
				break;
			}
		}
	}
	void addObjectToDefenceArray (GameObject insItem, Vector3 position, float cost) {
		GameObject item = Instantiate(insItem, position, insItem.transform.rotation) as GameObject;
		item.GetComponent<DefenceScript>().masterController = gameObject; 
		item.GetComponent<DefenceScript> ().buildingCost = cost;
		item.transform.GetChild(2).GetComponent<HealthDisplayScript>().mainCamera = mainCamera; 
		if (insItem == goldMinePrefab) {
			for (int i = 0; i < naturalResourcesArray.Length; i++) {
				if (naturalResourcesArray [i] != null) {
					if (Vector3.Distance (naturalResourcesArray [i].transform.position, position) < 1f) {
						Destroy (naturalResourcesArray [i].gameObject);
						naturalResourcesArray [i] = null;
						break;
					}
				}
			}
			GameObject[] localDefenceArray = new GameObject[defenceArray.Length];
			for (int i = 0; i < defenceArray.Length - 1; i++) {
				if (defenceArray[i] != null) {
					localDefenceArray [i + 1] = defenceArray [i].gameObject;
				} 
			}
			localDefenceArray [0] = item;
			defenceArray = localDefenceArray;
		} else {
			for (int i = 0; i < defenceArray.Length; i++) {
				if (defenceArray[i] == null) {
					defenceArray[i] = item;
					break;
				}
			}
		}
	}
	void addObjectToMyDefenceArray (GameObject insItem, Vector3 position, float cost) {
		GameObject item = Instantiate(insItem, position, insItem.transform.rotation) as GameObject;
		item.GetComponent<DefenceScript>().masterController = gameObject; 
		item.GetComponent<DefenceScript>().isPlayer = true;
		item.GetComponent<DefenceScript> ().buildingCost = cost;
		item.transform.GetChild(2).GetComponent<HealthDisplayScript>().mainCamera = mainCamera; 
		GameObject glowSphere = Instantiate (visualSpherePrefab, new Vector3(position.x, 3.5f, position.z), Quaternion.identity) as GameObject;
		glowSphere.transform.SetParent (item.transform);
		for (int i = 0; i < myDefenceArray.Length; i++) {
			if (myDefenceArray[i] == null) {
				myDefenceArray[i] = item;
				break;
			}
		}
	}
	GameObject[] copyArrayToArray (GameObject[] origin, GameObject[] target) {
		for (int i = 0; i < target.Length; i++) {
			if (target[i] != null) {
				origin[i] = target[i].gameObject;
			}
		}
		return origin;
	}
	void refreshSelectedArmour () {
		for (int i = 0; i < selectedArmour.Length; i++) {
			if (selectedArmour[i] != null) {
				selectedArmour[i].GetComponent<ArmourScript>().selected = true;
			}
		}
	}
	public void clearSelectionFromArmour () {
		for (int i = 0; i < armourArray.Length; i++) {
			if (armourArray[i] != null) {
				armourArray[i].GetComponent<ArmourScript>().selected = false;
			}
		}
	}
	public void clearSelectedArmour () {
		for (int i = 0; i < selectedArmour.Length; i++) {
			if (selectedArmour[i] != null) {
				selectedArmour[i] = null;    
			}
		}
	}
	void clearArray (GameObject[] array) {
		for (int i = 0; i < array.Length; i++) {
			if (array[i] != null) {
				array[i] = null;
			}
		}
	}
	void resetArmourBuildTimers () {
		selectedFactory.GetComponent<DefenceScript>().playerDoubleTankBuildTimer = selectedFactory.GetComponent<DefenceScript>().playerDoubleTankBuildCooldown;
		selectedFactory.GetComponent<DefenceScript>().playerArmouredCarBuildTimer = selectedFactory.GetComponent<DefenceScript>().playerArmouredCarBuildCooldown;
		selectedFactory.GetComponent<DefenceScript>().playerTankBuildTimer = selectedFactory.GetComponent<DefenceScript>().playerTankBuildCooldown;
	}
	public void produceFactoryItem (string itemName) {
		if (factorySelected) {
			if (itemName == "tank") {
				if (determineArmourPurchase() && selectedFactory.GetComponent<DefenceScript>().playerTankBuildTimer <= 0) {
					addObjectToArmourArray(tankPrefab, selectedFactory.transform.position);	
					playerCoins -= findBuildingCostWithName(itemName);
					resetArmourBuildTimers ();
				}
			} else if (itemName == "doubleTank") {
				if (determineArmourPurchase() && selectedFactory.GetComponent<DefenceScript>().playerDoubleTankBuildTimer <= 0) {
					addObjectToArmourArray(doubleTankPrefab, selectedFactory.transform.position);	
					playerCoins -= findBuildingCostWithName(itemName);
					resetArmourBuildTimers ();
				}
			} else if (itemName == "armouredCar") {
				if (determineArmourPurchase() && selectedFactory.GetComponent<DefenceScript>().playerArmouredCarBuildTimer <= 0) {
					addObjectToArmourArray(armouredCarPrefab, selectedFactory.transform.position);	
					playerCoins -= findBuildingCostWithName(itemName);
					resetArmourBuildTimers ();
				}
			}
		}
	}
	bool determineArmourPurchase () {
		bool p = true; 
		if (findBuildingCostWithName("tank") > playerCoins) {
			p = false;
			return p;
		}
		for (int i = 0; i < armourArray.Length; i++) {
			if (armourArray[i] != null) {
				if (Vector3.Distance(armourArray[i].transform.position, selectedFactory.transform.position) < 1f) {
					p = false;
					break;
				}
			}
		}
		return p;
	}
	void generateRandomEnemyBuilding () {
		int rand = Random.Range(0, 6);
		if (rand == 0 || rand == 1 || rand == 2) {
			currentEnemyBuilding = "turret";
		} else if (rand == 3) {
			currentEnemyBuilding = "bigTurret";
		} else if (rand == 4 || rand == 5) {
			currentEnemyBuilding = "factory";
		}
	}
	void buildEnemyBuilding (GameObject building) {
		generateRandomEnemyBuilding ();
		Vector3 placePosition = Vector3.zero;
		bool canPlace = false;
		Vector3 position;
		for (int h = 0; h < defenceArray.Length; h++) {
			if (defenceArray[h] != null) {
				for (int j = 0; j < 4; j++) {
					if (j == 0) {
						position = new Vector3(defenceArray[h].gameObject.transform.position.x + Random.Range(4f, 7f), 0, defenceArray[h].gameObject.transform.position.z + Random.Range(-1f, 1f));
					} else if (j == 1) {
						position = new Vector3(defenceArray[h].gameObject.transform.position.x + Random.Range(-1f, 1f), 0, defenceArray[h].gameObject.transform.position.z + Random.Range(4f, 7f));
					} else if (j == 2) {
						position = new Vector3(defenceArray[h].gameObject.transform.position.x - Random.Range(4f, 7f), 0, defenceArray[h].gameObject.transform.position.z + Random.Range(-1f, 1f));
					} else {
						position = new Vector3(defenceArray[h].gameObject.transform.position.x + Random.Range(-1f, 1f), 0, defenceArray[h].gameObject.transform.position.z - Random.Range(4f, 7f));
					}
					bool p = true;
					for (int i = 0; i < defenceArray.Length; i++) {
						if (myDefenceArray[i] != null) { 
							if (Vector3.Distance(myDefenceArray[i].gameObject.transform.position, position) < 4f) {
								p = false;
								break;
							}
						}
						if (defenceArray[i] != null) { 
							if (Vector3.Distance(defenceArray[i].gameObject.transform.position, position) < 4f) {
								p = false;
								break;
							}
						}
						if (armourArray[i] != null) { 
							if (Vector3.Distance(armourArray[i].gameObject.transform.position, position) < 4f) {
								p = false;
								break;
							}
						}
						if (enemyArmourArray[i] != null) { 
							if (Vector3.Distance(enemyArmourArray[i].gameObject.transform.position, position) < 4f) {
								p = false;
								break;
							}
						}
					}
					foreach (GameObject i in naturalResourcesArray) {
						if (i != null) { 
							if (Vector3.Distance(i.transform.position, position) < 2f) {
								p = false;
								break;
							}
						}
					}
					foreach (GameObject i in naturalBarriersArray) {
						if (i != null) { 
							if (i.transform.GetChild(0).GetComponent<Collider>().bounds.Contains(position)) {
								p = false;
								break;
							}
						}
					}
					if (position.x > 10 || position.x < -96 || position.z > 40 || position.z < -96) {
						p = false;
					}
					if (!GetComponent<PathFindGridScript>().gridArray[(int)(position.x + GetComponent<PathFindGridScript>().gridSize.x / 2), (int)(position.z + GetComponent<PathFindGridScript>().gridSize.y / 2)].placable) {
						p = false;
					}
					if (p) {
						if (building != goldMinePrefab) {
							canPlace = true;
							placePosition = position;
							break;
						} else if (building == goldMinePrefab) {
							foreach (GameObject i in naturalResourcesArray) {
								if (i != null) {
									if (Vector3.Distance (i.transform.position, position) < 8) {
										canPlace = true;
										placePosition = i.transform.position;
										break;
									}
								}
							}
							if (!canPlace) {
								generateRandomEnemyBuilding ();
							}
						}
					}
				}
				if (canPlace) {
					break;
				}
			}
		}
		if (canPlace) {
			addObjectToDefenceArray(building, placePosition, findBuildingCostWithName(building.name));
		}
	}
	public float findBuildingCostWithName (string name) {
		foreach (Cost i in prefabCosts) {
			if (i.name == name) {
				return i.cost;
			}
		}
		return 0;
	}
	void resetBuildTimers () {
		playerTurretBuildTimer = playerTurretBuildCooldown;
		playerFactoryBuildTimer = playerFactoryBuildCooldown;
		playerGoldMineBuildTimer = playerGoldMineBuildCooldown;
		playerHeadquartersBuildTimer = playerHeadquartersBuildCooldown;
	}
	void buildBuilding (GameObject building, Vector3 location) {
		if (building == turretPrefab && playerTurretBuildTimer <= 0) {
			float c = findBuildingCostWithName("turret"); 
			if (playerCoins >= c) {
				resetBuildTimers ();
				playerCoins -= c; 
				addObjectToMyDefenceArray(turretPrefab, location, c);
			}
		} else if (building == factoryPrefab && playerFactoryBuildTimer <= 0) {
			float c = findBuildingCostWithName("factory"); 
			if (playerCoins >= c) {
				resetBuildTimers ();
				playerCoins -= c;
				addObjectToMyDefenceArray(factoryPrefab, location, c);
			}
		} else if (building == goldMinePrefab && playerGoldMineBuildTimer <= 0) {
			float c = findBuildingCostWithName("goldMine"); 
			if (playerCoins >= c) {
				for (int i = 0; i < naturalResourcesArray.Length; i++) {
					if (naturalResourcesArray[i] != null) {
						if (Vector3.Distance(naturalResourcesArray[i].transform.position, location) < 1f) {
							Destroy (naturalResourcesArray[i].gameObject);
							naturalResourcesArray [i] = null;
							break;
						}
					}
				}
				resetBuildTimers ();
				playerCoins -= c;
				addObjectToMyDefenceArray(goldMinePrefab, location, c);
			}
		} else if (building == commandCenterPrefab && playerHeadquartersBuildTimer <= 0) {
			float c = findBuildingCostWithName("headquarters"); 
			if (playerCoins >= c) {
				resetBuildTimers ();
				playerCoins -= c;
				addObjectToMyDefenceArray(commandCenterPrefab, location, c);
			}
		}
	}
	float oneSecDelay = 1;
	void Update () {
		if (oneSecDelay > 1) {
			oneSecDelay -= Time.deltaTime;
		} else {
			int totalBuildings = 0;
			int goldMines = 0; 
			int totalFactories = 0;
			int totalTanks = 0; 
			foreach (GameObject i in defenceArray) {
				if (i != null) {
					totalBuildings++;
					if (i.GetComponent<DefenceScript>().isGoldMine && !i.GetComponent<DefenceScript>().stopGoldProduction) {
						goldMines++;
					}
					if (i.GetComponent<DefenceScript>().isFactory) {
						totalFactories++;
					}
				}
			}
			foreach (GameObject i in enemyArmourArray) {
				if (i != null) {
					totalBuildings++;
					if (i.GetComponent<ArmourScript>() != null) {
						totalTanks++;
					}
				}
			}
			if (goldMines < totalBuildings / (goldMines * 1.6f)) {
				currentEnemyBuilding = "goldMine";
			}
			if (totalTanks < totalFactories * 2) {
				prioritizeTankBuilding = true;
			} else {
				prioritizeTankBuilding = false;
			}
			oneSecDelay = 1;
		}
		deselectUnitsButton.GetComponent<Button> ().image = deselectUnitsButton.GetComponent<Button> ().image;
		if (gameEnded) {
			endGameDelay -= Time.deltaTime;
			if (endGameDelay < 0) {
				Time.timeScale = 0; 
				if (playerWon) {
					endGameUI.transform.GetChild (2).gameObject.GetComponent<Text> ().text = "Victory!";
				} else {
					endGameUI.transform.GetChild (2).gameObject.GetComponent<Text> ().text = "Defeat";
				}
				endGameUI.transform.Translate (new Vector3(0, -4000, 0), Space.Self);
				endGameDelay = 100000;
				gameEnded = false;
			}
		}
		if (Input.GetMouseButtonDown(0)) {
			touchDownPosition = Input.mousePosition;
		}
		if (sellBuildings) {
			if (!touchContainsUI) {
				if (Input.GetMouseButtonUp(0)) {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
					if (Physics.Raycast (ray, out hit, 1000f)) {
						if (hit.collider.transform.parent != null) {
							if (hit.collider.transform.parent.GetComponent<DefenceScript>() != null) {
								if (!hit.collider.transform.parent.GetComponent<DefenceScript>().isNatural && hit.collider.transform.parent.GetComponent<DefenceScript>().isPlayer) {
									hit.collider.transform.parent.GetComponent<DefenceScript> ().health = 0;
									playerCoins += hit.collider.transform.parent.GetComponent<DefenceScript> ().buildingCost / 3f;
									sellBuildingsToggle ();
								}
							}
						}
					}
				}
			}
		}
		bool haveSelection = false;
		foreach (GameObject i in selectedArmour) {
			if (i != null) {
				haveSelection = true;
				break;
			}
		}
		if (haveSelection) {
			showStopMoveButton ();
		} else {
			dontShowStopMoveButton ();
		}

		if (haveSelection || isConstructing) {
			showDeselectUnitsButton ();
		} else {
			dontShowDeselectUnitsButton ();
		}
		if (dragUnits) {
			dragUnitsButton.GetComponent<Image> ().color = new Color32 (60, 60, 60, 150);
		} else {
			dragUnitsButton.GetComponent<Image> ().color = new Color32 (255, 255, 255, 150);
		}
		if (sellBuildings) {
			sellBuildingsButton.GetComponent<Image> ().color = new Color32 (60, 60, 60, 150);
		} else {
			sellBuildingsButton.GetComponent<Image> ().color = new Color32 (255, 255, 255, 150);
		}
		if (rect1.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect2.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect3.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect4.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect5.Contains (Input.mousePosition) && buyTankButton.GetComponent<Image>().enabled) {
			touchContainsUI = true;
		} else if (rect6.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect7.Contains (Input.mousePosition) && deselectUnitsButton.GetComponent<Image>().enabled) {
			touchContainsUI = true;
		} else if (rect8.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect10.Contains (Input.mousePosition)) {
			touchContainsUI = true;
		} else if (rect11.Contains (Input.mousePosition) && stopMoveButton.GetComponent<Image>().enabled) {
			touchContainsUI = true;
		} else if (rect12.Contains (Input.mousePosition) && buyDoubleTankButton.GetComponent<Image>().enabled) {
			touchContainsUI = true;
		} else {
			touchContainsUI = false;
		}
		if (Input.GetMouseButtonDown(0) && dragUnits && !touchContainsUI) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			if (Physics.Raycast(ray, out hit, 1000f)) {
				dragStartPos = hit.point;
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			touchUpPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp(0) && dragUnits && !touchContainsUI) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			if (Physics.Raycast(ray, out hit, 1000f)) {
				Vector3 endPos = hit.point;
				if (Vector3.Distance(hit.point, dragStartPos) > 3) {
					clearSelectedArmour ();
					clearSelectionFromArmour ();
					for (int i = 0; i < armourArray.Length; i++) {
						if (armourArray[i] != null) {
							if (hit.point.x > armourArray[i].transform.position.x && dragStartPos.x < armourArray[i].transform.position.x || hit.point.x < armourArray[i].transform.position.x && dragStartPos.x > armourArray[i].transform.position.x) {
								if (hit.point.z > armourArray[i].transform.position.z && dragStartPos.z < armourArray[i].transform.position.z || hit.point.z < armourArray[i].transform.position.z && dragStartPos.z > armourArray[i].transform.position.z) {
									selectedArmour [i] = armourArray [i];
									armourArray [i].GetComponent<ArmourScript>().selected = true;
								}
							}
						}
					}
				}
			}
			if (Vector3.Distance(touchDownPosition, Input.mousePosition) > 10) {
				dragUnitsToggle ();
			}
		}	
		float headquartersCount = 0;
		foreach (GameObject i in myDefenceArray) {
			if (i != null) {
				if (i.GetComponent<DefenceScript>().isHeadquarters) {
					headquartersCount++;
				}
			}
		}
		if (playerFactoryBuildTimer > 0) {
			if (buyFactoryButton.GetComponent<Button>().enabled) {
				buyFactoryButton.GetComponent<Button>().enabled = false;
			}
			playerFactoryBuildTimer -= Time.deltaTime * (headquartersCount + 1 / 2);
		}
		if (playerFactoryBuildTimer < 0) {
			if (!buyFactoryButton.GetComponent<Button>().enabled) {
				buyFactoryButton.GetComponent<Button>().enabled = true;
			}
			playerFactoryBuildTimer = 0;
		}
		if (playerTurretBuildTimer > 0) {
			if (buyTurretButton.GetComponent<Button>().enabled) {
				buyTurretButton.GetComponent<Button>().enabled = false;
			}
			playerTurretBuildTimer -= Time.deltaTime * (headquartersCount + 1 / 2);
		}
		if (playerTurretBuildTimer < 0) {
			if (!buyTurretButton.GetComponent<Button>().enabled) {
				buyTurretButton.GetComponent<Button>().enabled = true;
			}
			playerTurretBuildTimer = 0;
		}
		if (playerGoldMineBuildTimer > 0) {
			if (buyGoldMineButton.GetComponent<Button>().enabled) {
				buyGoldMineButton.GetComponent<Button>().enabled = false;
			}
			playerGoldMineBuildTimer -= Time.deltaTime * (headquartersCount + 1 / 2);
		}
		if (playerGoldMineBuildTimer < 0) {
			if (!buyGoldMineButton.GetComponent<Button>().enabled) {
				buyGoldMineButton.GetComponent<Button>().enabled = true;
			}
			playerGoldMineBuildTimer = 0;
		}
		if (playerHeadquartersBuildTimer > 0) {
			if (buyHeadquartersButton.GetComponent<Button>().enabled) {
				buyHeadquartersButton.GetComponent<Button>().enabled = false;
			}
			playerHeadquartersBuildTimer -= Time.deltaTime;
		}
		if (playerHeadquartersBuildTimer < 0) {
			if (!buyHeadquartersButton.GetComponent<Button>().enabled) {
				buyHeadquartersButton.GetComponent<Button>().enabled = true;
			}
			playerHeadquartersBuildTimer = 0;
		}
		buyTurretButton.transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(playerTurretBuildTimer / playerTurretBuildCooldown * loadBarOriginalSize.x, loadBarOriginalSize.y);
		buyGoldMineButton.transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(playerGoldMineBuildTimer / playerGoldMineBuildCooldown * loadBarOriginalSize.x, loadBarOriginalSize.y);
		buyFactoryButton.transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(playerFactoryBuildTimer / playerFactoryBuildCooldown * loadBarOriginalSize.x, loadBarOriginalSize.y);
		buyHeadquartersButton.transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(playerHeadquartersBuildTimer / playerHeadquartersBuildCooldown * loadBarOriginalSize.x, loadBarOriginalSize.y);
		if (selectedFactory != null) {
			buyTankButton.transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(selectedFactory.GetComponent<DefenceScript>().playerTankBuildTimer / selectedFactory.GetComponent<DefenceScript>().playerTankBuildCooldown * loadBarOriginalSize.x, loadBarOriginalSize.y);
			buyDoubleTankButton.transform.GetChild(1).GetComponent<Image>().rectTransform.sizeDelta = new Vector2(selectedFactory.GetComponent<DefenceScript>().playerDoubleTankBuildTimer / selectedFactory.GetComponent<DefenceScript>().playerDoubleTankBuildCooldown * loadBarOriginalSize.x, loadBarOriginalSize.y);
		}
		enemyAttackTime -= Time.deltaTime;
		if (enemyAttackTime <= 0) {
			int enemyArmourCount = 0;
			int playerBuildingCount = 0;
			foreach (GameObject i in enemyArmourArray) {
				if (i != null) {
					enemyArmourCount++;
				}
			}
			foreach (GameObject i in myDefenceArray) {
				if (i != null) {
					playerBuildingCount++;
				}
			}
			if (enemyArmourCount >= playerBuildingCount || enemyArmourCount >= 2) {
				print ("attackPlayer");
				enemyAttackTime = enemyAttackCooldown;
				foreach (GameObject i in enemyArmourArray) {  
					if (i != null) {
						if (!i.GetComponent<ArmourScript>().attackPlayer) {
							i.GetComponent<ArmourScript> ().attackPlayer = true;
							float minDistance = 100000f;
							Vector3 moveDestination = new Vector3(0, 0, 0);
							foreach (GameObject a in myDefenceArray) {
								if (a != null) { 
									if (Vector3.Distance(a.transform.position, i.transform.position) < minDistance) {
										minDistance = Vector3.Distance(a.transform.position, i.transform.position);
										moveDestination = a.transform.position;
										print ("moving out");
									}	
								}	
							}	
							i.GetComponent<ArmourScript> ().moveTank (moveDestination.x, moveDestination.z, true);
						}
					}
				}
			}
		}
		
		float c = originalPrefabCosts[3].cost;
		playerCoins += 6 * Time.deltaTime;
		playerCoinsDisplay.text = ((int)playerCoins).ToString();
		foreach (GameObject i in myDefenceArray) {
			if (i != null) {
				if (i.GetComponent<DefenceScript>().isGoldMine) {
					if (!i.GetComponent<DefenceScript>().stopGoldProduction) {
						c += originalPrefabCosts[3].cost / 4;
					} 
				}
			}
		}
		prefabCosts[3].cost = c;
		updateButtonsText();

		float c1 = originalPrefabCosts[3].cost * 3;
		enemyCoins += (3 + PlayerPrefs.GetInt("difficulty")) * Time.deltaTime;
		foreach (GameObject i in defenceArray) {
			if (i != null) {
				if (i.GetComponent<DefenceScript>().isGoldMine) {
					if (!i.GetComponent<DefenceScript>().stopGoldProduction) {
						c1 += originalPrefabCosts[3].cost;
					} 
				}
			}
		}
		enemyPrefabCosts[3].cost = c1;

		if (Input.GetKeyDown(KeyCode.U) && playerTurretBuildTimer <= 0) {
			constructItem(turretPrefab);
		}
		if (Input.GetKeyDown(KeyCode.H) && playerHeadquartersBuildTimer <= 0) {
			constructItem(commandCenterPrefab);
		}
		if (Input.GetKeyDown(KeyCode.F) && playerFactoryBuildTimer <= 0) {
			constructItem(factoryPrefab);
		}
		if (Input.GetKeyDown(KeyCode.T)) {
			produceFactoryItem("tank");
		}
		if (Input.GetKeyDown(KeyCode.D)) {
			produceFactoryItem("doubleTank");
		}
		if (Input.GetKeyDown(KeyCode.G) && playerGoldMineBuildTimer <= 0) {
			constructItem(goldMinePrefab);
		} 
		if (factorySelected) {
			buyTankButton.GetComponent<Button>().enabled = true;
			buyTankButton.GetComponent<Image>().enabled = true;
			buyTankButton.transform.GetChild(0).GetComponent<Text>().enabled = true;
			buyTankButton.transform.GetChild(1).GetComponent<Image>().enabled = true;

			buyDoubleTankButton.GetComponent<Button>().enabled = true;
			buyDoubleTankButton.GetComponent<Image>().enabled = true;
			buyDoubleTankButton.transform.GetChild(0).GetComponent<Text>().enabled = true;
			buyDoubleTankButton.transform.GetChild(1).GetComponent<Image>().enabled = true;
		} else {
			buyTankButton.GetComponent<Button>().enabled = false;
			buyTankButton.GetComponent<Image>().enabled = false;
			buyTankButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
			buyTankButton.transform.GetChild(1).GetComponent<Image>().enabled = false;

			buyDoubleTankButton.GetComponent<Button>().enabled = false;
			buyDoubleTankButton.GetComponent<Image>().enabled = false;
			buyDoubleTankButton.transform.GetChild(0).GetComponent<Text>().enabled = false;
			buyDoubleTankButton.transform.GetChild(1).GetComponent<Image>().enabled = false;
		}
		if (enemyCoins >= findBuildingCostWithName(currentEnemyBuilding) && (currentEnemyBuilding == "goldMine" || !prioritizeTankBuilding || enemyCoins > 600)) {
			enemyCoins -= findBuildingCostWithName (currentEnemyBuilding);
			if (currentEnemyBuilding == "turret") {
				buildEnemyBuilding (turretPrefab);
			} else if (currentEnemyBuilding == "bigTurret") {
				buildEnemyBuilding (bigTurretPrefab);
			} else if (currentEnemyBuilding == "goldMine") {
				buildEnemyBuilding (goldMinePrefab);
			} else if (currentEnemyBuilding == "factory") {
				buildEnemyBuilding (factoryPrefab);
			}
		}
		if (isConstructing) {
			justConstructed = true;
			constructionHover.GetComponent<Renderer>().enabled = true;
			RaycastHit hit;
		 	Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		 	if (Physics.Raycast(ray, out hit, 1000f)) {
				Vector3 finalPos = hit.point;
				constructionHover.transform.position = new Vector3(hit.point.x, -0.5f, hit.point.z); 
		 		bool canPlace = true;
				bool canPlace1 = false; 
				bool canPlace2 = false;
				if (hit.collider.transform.parent != null && hit.collider.gameObject.transform.parent.GetComponent<DefenceScript>() != null) {
					if (hit.collider.gameObject.transform.parent.GetComponent<DefenceScript>().isBarrier) {
						canPlace = false;
					}
				} else if (hit.collider.gameObject.GetComponent<DefenceScript>() != null && !hit.collider.gameObject.GetComponent<DefenceScript> ().isGoldMine) {
					for (int i = 0; i < naturalResourcesArray.Length; i++) {
						if (naturalResourcesArray[i] != null) {
							if (Vector3.Distance (naturalResourcesArray [i].gameObject.transform.position, new Vector3 (hit.point.x, 0, hit.point.z)) < 2f) {
								canPlace = false;
								break;
							}
						}
					}
				} else {
					for (int i = 0; i < myDefenceArray.Length; i++) {
						if (myDefenceArray [i] != null) { 
							if (constructionItemPrefab == goldMinePrefab || constructionItemPrefab == factoryPrefab) {
								if (Vector3.Distance (myDefenceArray [i].gameObject.transform.position, new Vector3 (hit.point.x, 0, hit.point.z)) < 2f) {
									canPlace = false;
									break;
								}
							} else if (Vector3.Distance (myDefenceArray [i].gameObject.transform.position, new Vector3 (hit.point.x, 0, hit.point.z)) < myDefenceArray [i].GetComponent<DefenceScript> ().sizeX / 3 + 2f + constructionItemPrefab.GetComponent<DefenceScript> ().sizeX / 2) {
								canPlace = false;
								break;
							}
						}
						if (defenceArray [i] != null) { 
							if (Vector3.Distance (defenceArray [i].gameObject.transform.position, new Vector3 (hit.point.x, 0, hit.point.z)) < defenceArray [i].GetComponent<DefenceScript> ().sizeX / 3 + 2f + constructionItemPrefab.GetComponent<DefenceScript> ().sizeX / 2) {
								canPlace = false;
								break;
							}
						}
						if (armourArray [i] != null) { 
							if (Vector3.Distance (armourArray [i].gameObject.transform.position, new Vector3 (hit.point.x, 0, hit.point.z)) < 2f) {
								canPlace = false;
								break;
							}
						}
						if (enemyArmourArray [i] != null) { 
							if (Vector3.Distance (enemyArmourArray [i].gameObject.transform.position, new Vector3 (hit.point.x, 0, hit.point.z)) < 2f) {
								canPlace = false;
								break;
							}
						}
					}

					if (constructionItemPrefab == goldMinePrefab) {
						for (int i = 0; i < naturalResourcesArray.Length; i++) {
							if (naturalResourcesArray[i] != null) {
								if (Vector3.Distance(naturalResourcesArray[i].transform.position, new Vector3(hit.point.x, -0.5f, hit.point.z)) < 1f && naturalResourcesArray[i].GetComponent<DefenceScript>().isGoldReady) {
									canPlace2 = true;
									finalPos = naturalResourcesArray [i].transform.position;
									break;
								}
							}
						}
					} else {
						canPlace2 = true;
					}
					for (int i = 0; i < myDefenceArray.Length; i++) {
						if (myDefenceArray[i] != null) { 
							if (Vector3.Distance(myDefenceArray[i].gameObject.transform.position, new Vector3(hit.point.x, 0, hit.point.z)) < 8) {
								canPlace1 = true;
								break;
							}
						}
					}
				}
				if (canPlace && canPlace1 && canPlace2) {
					constructionHover.GetComponent<Renderer>().material.color = new Color32(0, 255, 0, 255);
					if (Input.GetMouseButtonUp(0) && !touchContainsUI) {
						buildBuilding(constructionItemPrefab, new Vector3(finalPos.x, 0, finalPos.z));
						isConstructing = false;
					}
				} else {
					constructionHover.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 255);
				}	
		 	}		
		} else {
			constructionHover.GetComponent<Renderer>().enabled = false;
		}
		if (Input.GetMouseButtonDown(0) && !touchContainsUI) {
			factorySelected = false;
			RaycastHit hit2;
			Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			if (Physics.Raycast(ray2, out hit2, 1000f)) {
				if (hit2.collider.transform.parent != null) {
					if (hit2.collider.transform.parent.GetComponent<DefenceScript>() != null) {
						if (hit2.collider.transform.parent.GetComponent<DefenceScript>().isFactory && hit2.collider.transform.parent.GetComponent<DefenceScript>().isPlayer) {
							factorySelected = true;
							selectedFactory = hit2.collider.transform.parent.gameObject;
							clearSelectedArmour ();
							clearSelectionFromArmour ();
						}
						if (!hit2.collider.transform.parent.GetComponent<DefenceScript>().isPlayer && !hit2.collider.transform.parent.GetComponent<DefenceScript>().isNatural && !hit2.collider.transform.parent.GetComponent<DefenceScript>().isBarrier) {
							foreach (GameObject i in selectedArmour) {
								if (i != null) {
									i.GetComponent<ArmourScript> ().target = hit2.collider.gameObject.transform.parent.gameObject;
								}
							}        
						}
					}
				}
			}
		}
		
		if (Input.GetKey(KeyCode.LeftAlt)) {
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				clearArray(armourGroup1);
				armourGroup1 = copyArrayToArray(armourGroup1, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				clearArray(armourGroup2);
				armourGroup2 = copyArrayToArray(armourGroup2, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				clearArray(armourGroup3);
				armourGroup3 = copyArrayToArray(armourGroup3, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				clearArray(armourGroup4);
				armourGroup4 = copyArrayToArray(armourGroup4, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha5)) {
				clearArray(armourGroup5);
				armourGroup5 = copyArrayToArray(armourGroup5, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha6)) {
				clearArray(armourGroup6);
				armourGroup6 = copyArrayToArray(armourGroup6, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha7)) {
				clearArray(armourGroup7);
				armourGroup7 = copyArrayToArray(armourGroup7, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha8)) {
				clearArray(armourGroup8);
				armourGroup8 = copyArrayToArray(armourGroup8, selectedArmour);
			}
			if (Input.GetKeyDown(KeyCode.Alpha9)) {
				clearArray(armourGroup9);
				armourGroup9 = copyArrayToArray(armourGroup9, selectedArmour);
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup1);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup2);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha3)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup3);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha4)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup4);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha5)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup5);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha6)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup6);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha7)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup7);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha8)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup8);
				refreshSelectedArmour();
			}
			if (Input.GetKeyDown(KeyCode.Alpha9)) {
				clearSelectionFromArmour();
				clearArray(selectedArmour);
				selectedArmour = copyArrayToArray(selectedArmour, armourGroup9);
				refreshSelectedArmour();
			}
		}
		if (Input.GetKey(KeyCode.LeftApple)) {
			stackSelectedArmour = true;
		} else {
			stackSelectedArmour = false;
		}
		if (Input.GetMouseButtonUp(0) && !touchContainsUI) {
			RaycastHit hit;
		 	Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			if (Physics.Raycast (ray, out hit, 1000f)) {
				if (hit.collider.gameObject.GetComponent<ArmourComponentScript> () != null) {
					if (stackSelectedArmour) {
						for (int i = 0; i < selectedArmour.Length; i++) {
							if (hit.collider.gameObject.GetComponent<ArmourComponentScript> ().selectTank (false).GetComponent<ArmourScript> ().selected == false && selectedArmour [i] == null) {
								selectedArmour [i] = hit.collider.gameObject.GetComponent<ArmourComponentScript> ().selectTank (true);
								break;
							}
						}
					} else {
						clearSelectionFromArmour ();
						clearSelectedArmour ();
						selectedArmour [0] = hit.collider.gameObject.GetComponent<ArmourComponentScript> ().selectTank (true);
					}
				} else {
					RaycastHit hit1;
					Ray ray1 = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
					if (Physics.Raycast(ray1, out hit1, 1000f) && !dragUnits && !justConstructed && !isConstructing && Vector3.Distance(touchDownPosition, touchUpPosition) < 10) {
						for (int i = 0; i < selectedArmour.Length; i++) {
							if (selectedArmour[i] != null) {
								print ("move");
								selectedArmour[i].GetComponent<ArmourScript>().moveTank(hit1.point.x, hit1.point.z, true);
							}
						}
					}
				}
			}  
		}
		if (Input.GetKeyDown(KeyCode.A)) {
			selectAllUnits ();
		}
		justConstructed = false;
	}
	public void selectAllUnits () {
		for (int i = 0; i < armourArray.Length; i++) {
			if (armourArray[i] != null) {
				armourArray[i].GetComponent<ArmourScript>().selected = true;
				selectedArmour[i] = armourArray[i];
			}
		}
	}
}