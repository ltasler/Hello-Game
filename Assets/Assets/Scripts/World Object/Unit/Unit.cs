using UnityEngine;
using System.Collections;
using Pathfinding;

public class Unit : WorldObject {

	private Seeker seeker;
	private CharacterController characterController;
	private Path path;
	private int currentWaypoint;
	private State unitState;

	public float movementSpeed;
	public float nextWaypointDistance;

	protected GameObject trainBuilding;
	protected bool toTrain;

	protected enum State {idle, moving};

	// Use this for initialization
	protected override void  Start () {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		unitState = State.idle;
		toTrain = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
	
	}

	void FixedUpdate() {
		if(unitState == State.moving) {
			Moving ();
		}
	}

	public void StartMove(Vector3 destination) {
		Path p = seeker.StartPath(transform.position, destination, OnPathComplete);
		if(!p.error) {
			unitState = State.moving;
			path = p;
			currentWaypoint = 0;
		}
	}

	public void OnPathComplete(Path p) {
		if(!p.error) {
			path = p;
			currentWaypoint = 0;
			unitState = State.moving;
		}
	}

	private void Moving() {
		if (path == null) {
			//We have no path to move after yet
			return;   
		}

		//ends movement.. destination reached
		if (currentWaypoint >= path.vectorPath.Count) {
			unitState = State.idle;
			/*when unit reaches its destination and is supposed to train, it hides inside building and waits till training is complete */
			if(toTrain && path.vectorPath.Count != 0) {
				this.gameObject.transform.position = trainBuilding.transform.position;
				Barracks b = trainBuilding.GetComponent<Barracks>();
				b.StartTraining(this.gameObject);
			}
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= movementSpeed * Time.fixedDeltaTime;
		//if(dir != movementDirection) {
		//	movementDirection = dir;
			characterController.SimpleMove(dir);
		//}
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
		}
	}

	/* unit starts training in building given in parameter
	 * @param	Gameobject building		building in which is about to train
	 */
	public void GoTrain(GameObject building) {
		toTrain = true;
		trainBuilding = building;
		StartMove (building.transform.position);
	}
}





















