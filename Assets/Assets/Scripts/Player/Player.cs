using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	//player's name
	public string username;
	//check if human
	public bool human;

	private List<GameObject> selectedObjects;
	public GameObject[] SelectedObjects {get {return selectedObjects.ToArray();}}

	public List<Unit> ownedUnits;
	public Unit[] OwnedUnits { get {return ownedUnits.ToArray();}}
	public int Population { get {return ownedUnits.Count;}}

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
		Debug.Log(gameObject);
	}

	public void Deselect() {
		selectedObjects.Clear();
	}
}










