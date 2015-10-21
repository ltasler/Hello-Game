using UnityEngine;
using System.Collections;

public class BasicUnit : Unit {

	//worker's power(ability to chop down trees and build buildings
	public float workerPower;

	private int carrying;

	public int workingRange;

	private Tree nearestTree;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		nearestTree = null;
		carrying = 0;

	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	/*	if(unitState == State.choppin)
			KeepChopin();
		else if(unitState == State.buildin)
			KeepOnBuildin();
*/
	}

	/*#region start methods

	public void StartGathering(GameObject tree) {
		nearestTree = FindNearestTree();
		unitState = State.choppin;
		seeker.StartPath(transform.position, tree.transform.position, OnPathComplete);
	}

	public void StartBuilding (GameObject building) {
		target = building;
		unitState = State.buildin;
		seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
	}

	#endregion

	//TODO
	private void KeepChopin() {
		Vector3 targetPos = target.transform.position;
		Vector3 ownPosition = transform.position;
		Vector3 distance = Vector3.Distance(targetPos, ownPosition);
		if(distance <= CHOPPIN_DISTANCE) {

		}
	}

	private void KeepOnBuildin() {
		Building b = target.GetComponent<Building>();
		if(b.IsBuild) {
			unitState = State.idle;
			target = null;
		}

		else if (b.buildProgress < ((100*resourceBrought) / maxResources)) {

		}
		if(IsInworkingRange(target)) {

			b.HitHammer(workerPower);
		}
		else {
			Moving();
		}
	}

	#region aid
	
		private bool IsInworkingRange(GameObject obj) {
			Vector3 objPos = obj.transform.position;
			Vector3 pos = transform.position;
			Vector3 distance = Vector3.Distance(objPos, pos);
			if(distance < workingRange)
				return true;
			else
				return false;
		}

	private Tree FindNearestTree() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100);
		
		foreach(Collider c in hitColliders) {
			if(c.gameObject.GetComponent<Tree>()) {
				Tree tr = c.gameObject.GetComponent<Tree>();
				return c.gameObject;

				}*/
		/*	}
		return null;
	}*/

}





















