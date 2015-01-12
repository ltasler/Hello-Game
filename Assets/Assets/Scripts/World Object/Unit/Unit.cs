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

	protected enum State {idle, moving};

	// Use this for initialization
	protected override void  Start () {
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		unitState = State.idle;
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(unitState == State.moving)
			Moving();
	}

	public void StartMove(Vector3 destination) {
		Path p = seeker.StartPath(transform.position, destination);
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
			Debug.Log ("End Of Path Reached");
			unitState = State.idle;
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= movementSpeed * Time.deltaTime;
		characterController.SimpleMove (dir);
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
		}
	}
}
