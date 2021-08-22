using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid {
	public GameObject originateFrom;
	public bool permanantUnplacable;
	public Vector2 position;
	public Vector2 arrayIndex;
	public Grid previousGrid;
	public bool placable; 
	public bool placed;
	public bool isStart;
	public bool isFinish;
	public bool closed;
	public GameObject cannotPlaceReason;
	public Grid (Vector2 p1, bool p3) {
		position = p1;
		placable = p3;
	}
}
public class PathFindGridScript : MonoBehaviour {
	public float refreshGridSpeed;
	float refreshTimer;
	public Grid[,] gridArray;
	float gridRadius = 0.7f;                                                  
	public Vector2 gridSize;

	void Start () {
		
		refreshTimer = refreshGridSpeed;
		gridArray = new Grid[(int)gridSize.x, (int)gridSize.y];
		for (int i = 0; i < gridSize.x; i++) {
			for (int j = 0; j < gridSize.y; j++) {
				gridArray [i, j] = new Grid (new Vector2(i - gridSize.x / 2, j - gridSize.y / 2), true);
				gridArray [i, j].arrayIndex = new Vector2 (i, j);
				if (Physics.CheckSphere(new Vector3(gridArray [i, j].position.x, 0f, gridArray [i, j].position.y), gridRadius, 1)) {
					gridArray [i, j].placable = false;
					GameObject orgItem = Physics.OverlapSphere(new Vector3(gridArray [i, j].position.x, 0f, gridArray [i, j].position.y), gridRadius, 1)[0].gameObject;
					if (orgItem.transform.parent != null) {
						orgItem = orgItem.transform.parent.gameObject;
					}
					gridArray [i, j].originateFrom = orgItem;
				}
				transform.position = new Vector3 (gridArray [i, j].position.x, 10, gridArray [i, j].position.y);
				RaycastHit hit;
				Vector3 down = transform.TransformDirection (Vector3.down);
				if (Physics.Raycast (transform.position, down, out hit, 100)) {
					if (hit.collider.name == "Ocean") {
						gridArray [i, j].placable = false;
						gridArray [i, j].permanantUnplacable = true;
					}
				}
				transform.position = Vector3.zero;
			}
		}
	}  
	public void clearGridArray (Grid[] grid) {
		for (int j = 0; j < grid.Length; j++) {
			if (grid [j] != null) {
				grid [j].placed = false;
				grid [j].isStart = false;
				grid [j].isFinish = false;
				grid [j].closed = false;
				grid [j].previousGrid = null;
			} else {
				break;
			}
		} 
	}
	void Update () {
		refreshTimer -= Time.deltaTime;
		if (refreshTimer <= 0) {
			refreshTimer = refreshGridSpeed;
			for (int i = 0; i < gridSize.x; i++) {
				for (int j = 0; j < gridSize.y; j++) {
					if (Physics.CheckSphere (new Vector3 (gridArray [i, j].position.x, 0f, gridArray [i, j].position.y), gridRadius, 1)) {
						gridArray [i, j].placable = false;
						GameObject orgItem = Physics.OverlapSphere(new Vector3(gridArray [i, j].position.x, 0f, gridArray [i, j].position.y), gridRadius, 1)[0].gameObject;
						if (orgItem.transform.parent != null) {
							orgItem = orgItem.transform.parent.gameObject;
						}
						gridArray [i, j].originateFrom = orgItem;
					} else {
						if (!gridArray [i, j].permanantUnplacable) {
							gridArray [i, j].placable = true;
						}
					}
				} 
			} 
		}
	}
	void OnDrawGizmos () {
		Gizmos.DrawWireCube (new Vector3(0, 2, 0), new Vector3(160, 1, 160));
		if (gridArray != null) {
			for (int i = 0; i < gridSize.x; i++) {
				for (int j = 0; j < gridSize.y; j++) {
					if (gridArray[i, j].placable) {
						Gizmos.DrawWireCube (new Vector3(gridArray[i, j].position.x, 1, gridArray[i, j].position.y), new Vector3(1, 1, 1));
					}
				} 
			}
		}
	}
}
