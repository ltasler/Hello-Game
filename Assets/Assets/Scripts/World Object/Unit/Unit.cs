using UnityEngine;
using System.Collections;
using Pathfinding;
using RTS;

public class Unit : WorldObject {

	private Seeker seeker; //seeker component
	private CharacterController characterController; //charachter controller (SimpleMove(direction))
	private Path path; //unit's current path
	private int currentWaypoint; // current waypoint on which unit is

	private State unitState; //state of unit
	protected enum State {idle, moving, attack}; //possible states that unit can take

	public float movementSpeed; //speed of unit
	public float nextWaypointDistance; // a* something

	protected GameObject trainBuilding; //if unit is training it is training in that building
	protected bool toTrain; //if unit is training (desibles all funcunality if it is)

	public float attackDamage;
	public float attackSpeed;
	public float attackRange;

	//unit's radius of detecteing units and also fog of war?
	public float detectionRadius;

	/*Unit's target */
	private GameObject target;
	private Vector3 lastknownTargetPosition;

	/*1 when redy to strike less then 1 when not */
	private float readyToAttack;

	/* if unit ordered to perform attack move it will remember destination and went there */
	private Vector3 attackMoveDestination;
	private GraphNode node; /* node on which unit was last before randomly attacking */


	#region unity's methods
	// Use this for initialization
	protected override void  Start () {
		lastknownTargetPosition = GameManager.InvalidPosition;
		attackMoveDestination = GameManager.InvalidPosition;
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		unitState = State.idle;
		toTrain = false;
		base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		readyToAttack += Time.deltaTime/attackSpeed;
		if(unitState == State.idle) {
			AttackNearby();
		}
		else if(target) {
			AttackTarget();
		}
		else if (unitState == State.attack)
			unitState = State.idle;
	}

	void FixedUpdate() {
		if(unitState == State.moving) {
			Moving ();
		}
	}
	#endregion

	#region unit attack capabilities
	private void AttackTarget() {
		float TargetDistance = Vector3.Distance(target.transform.position, this.transform.position);
		if(TargetDistance < attackRange) {
			Strike();
		}
		else {
			MoveToTarget();
		}
	}

	private void Strike() {
		unitState = State.attack;
		if(readyToAttack >= 1) {
			WorldObject wo = target.GetComponent<WorldObject>();
			wo.Hit(attackDamage);
		}
	}

	/* Unit, moves to targed and attacks it */
	public void Attack(GameObject toAttack) {
		target = toAttack;
		lastknownTargetPosition = target.transform.position;
		seeker.StartPath(transform.position, toAttack.transform.position, OnPathComplete);
	}
	
	/*stops targeting a unit, called always when performing any other action then attacking */
	private void Disengage() {
		target = null;
		lastknownTargetPosition = GameManager.InvalidPosition;
	}

	#endregion

	#region unit's movement capabilities
	private void MoveToTarget() {
		if(target.transform.position != lastknownTargetPosition || lastknownTargetPosition == GameManager.InvalidPosition)
			seeker.StartPath(transform.position, target.transform.position, OnPathComplete);

	}

	public void StartMove(Vector3 destination) {
		Disengage();
		seeker.StartPath(transform.position, destination, OnPathComplete);
		if(toTrain)
			toTrain = false;
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

	#endregion

	/* unit starts training in building given in parameter
	 * @param	Gameobject building		building in which is about to train
	 */
	public void GoTrain(GameObject building) {
		Disengage();
		toTrain = true;
		trainBuilding = building;
		Vector3 destination = building.transform.position;
		seeker.StartPath(transform.position, destination, OnPathComplete);
	}


	public void AttackMove(Vector3 destination) {
		//ATTACK MOVE TODO
		attackMoveDestination = destination;
	}

	/* Method makes unit attack nerby units */

	private void AttackNearby() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
		foreach(Collider c in hitColliders) {
			if(c.gameObject.GetComponent<WorldObject>()) {
				WorldObject wo = c.gameObject.GetComponent<WorldObject>();
				if(wo.Owner != owner) {
					Attack (wo.gameObject);
					Debug.Log (wo.gameObject);
					return;
				}
			}
		}
	}
}





















