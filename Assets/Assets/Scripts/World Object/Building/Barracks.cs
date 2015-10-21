using UnityEngine;
using System.Collections.Generic;

public class Barracks : Building {

	/*is building active ? */
	private bool isTraining;

	/* unit queue for units */
	private Queue<GameObject> unitsToTrain;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		spawnPoint += transform.forward; //TODO twas a real quick spawn fix :)
		isTraining = false;
		unitsToTrain = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(isTraining)
			base.Update();
	}

	public void StartTraining(GameObject unit) {
		isTraining = true;
		unitsToTrain.Enqueue(unit); 
		owner.DeselectUnit(unit);
	} 

	protected override void CreateNewUnit () {
		if(unitsToTrain.Count == 0) {
			isTraining = false;
			creationProgress = 0;
			return;
		}
		if(creationProgress >= 1) {

			//unit that came in barracks to train should be destroyed
			GameObject newUnit = unitsToTrain.Dequeue();
			owner.DeselectUnit(newUnit);
			owner.RemoveUnit(newUnit);
			Destroy(newUnit);
		}
		base.CreateNewUnit ();
	}
}
