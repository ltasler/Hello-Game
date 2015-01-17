using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	//player's name
	public string username;
	//check if human
	public bool human;

	//player's color
	public Color playerColor;
	public Color PlayerColor {get {return playerColor;}}

	/* handles everything a play has selected */
	private List<GameObject> selectedObjects;
	public GameObject[] SelectedObjects {get {return selectedObjects.ToArray();}}

	/* Handles all unit under player command as well as calculates unit limit and current population */
	public List<Unit> ownedUnits;
	public Unit[] OwnedUnits { get {return ownedUnits.ToArray();}}
	public int Population { get {return ownedUnits.Count;}}

	/* Handles all buildings under play command and calculates population limit */
	public List<Building> ownedBuildings;
	public int PopulationLimit {get {int p=0; foreach (Building b in ownedBuildings) if(b is Housing) p+=b.populationSupport; return p;}}

	void Start() {
		selectedObjects = new List<GameObject>();
	}

	void Awake() {

	}

	void Update() {

	}

	public void Select(Unit unit) {
		GameObject gameObject = unit.gameObject;
		selectedObjects.Add(gameObject);
	}

	public void Deselect() {
		selectedObjects.Clear();
	}

	public void DeselectUnit(GameObject unit) {
		selectedObjects.Remove(unit);
	}

	public void AddUnit(GameObject o) {
		Unit u = o.GetComponent<Unit>();
		ownedUnits.Add(u);
	}

	public void RemoveUnit(GameObject o) {
		Unit u = o.GetComponent<Unit>();
		ownedUnits.Remove(u);
	}

	public void AddBuilding(GameObject o) {
		Building b = o.GetComponent<Building>();
		ownedBuildings.Add(b);
	}

	public void RemoveBuilding(GameObject o) {
		Building b = o.GetComponent<Building>();
		ownedBuildings.Remove(b);
	} 
}










