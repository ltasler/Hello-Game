using UnityEngine;
using System.Collections;
using RTS;

public class BuildManager : MonoBehaviour {

	private Player player;

	//array of all buildings that are possible to build
	public GameObject[] buildings;

	private GameObject toBuild;

	private bool playerBuilding;
	public bool PlayerBuilding {get {return playerBuilding;}}

	private Terrain terrain;

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		playerBuilding = false;
		terrain = Terrain.activeTerrain;
	}
	
	// Update is called once per frame
	void Update () {
		if(player && player.human && playerBuilding) {
			Vector3 mouseWorldPosition = GameManager.MouseToWorldPosition();
			mouseWorldPosition.y = terrain.SampleHeight(mouseWorldPosition);
			toBuild.transform.position = mouseWorldPosition;
		}
	}

	/* Player enters building mode, building model appers for player */
	public void startBuilding(string buildingName) {
		foreach(GameObject go in buildings) {
			if(buildingName.Equals(go.name)) {
				toBuild = Instantiate(go) as GameObject;
				GameManager.SetOwner(toBuild, player);
				playerBuilding = true;
				toBuild.transform.position = GameManager.MouseToWorldPosition();
				toBuild.GetComponent<Collider>().enabled = false;
			}
		}
	}
	
	/* gets out of building mode */
	private void StopBuilding() {
		toBuild = null;
		playerBuilding = false;
	}

	/* gets out of building mode, building is not build*/
	public void CancelBuilding() {
		Destroy (toBuild);
		StopBuilding();
	}

	/* Building is placed on clicked location */
	public void PlaceBuilding() {
		if(playerBuilding) {
			toBuild.GetComponent<Collider>().enabled = true;
			AstarPath.active.UpdateGraphs(toBuild.GetComponent<Collider>().bounds); /*refresh graphs so units wont go trough buildings */
			toBuild.GetComponent<Building>().StartBuilding(player);
			player.AddBuilding(toBuild);
			StopBuilding();
		}
	}
}




















