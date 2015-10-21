using UnityEngine;
using System.Collections;
using Pathfinding;
using RTS;

public class Unit : WorldObject {

	#region parameters

	protected Seeker seeker; //seeker component
	protected CharacterController characterController; //charachter controller (SimpleMove(direction))
	protected Path path; //unit's current path
	protected int currentWaypoint; // current waypoint on which unit is
	
	protected State unitState; //state of unit
	protected enum State {idle, moving, attack, attack_move, onTraining, choppin, buildin}; //possible states that unit can take
	
	public float movementSpeed; //speed of unit
	public float nextWaypointDistance; // a* something

	public float attackDamage;
	public float attackSpeed;
	public float attackRange;

	protected const float ATTACK_SPEED_MULTIPLYER = 100;
	
	//unit's radius of detecteing units and also fog of war?
	public float sight;
	
	/*Unit's target */
	protected GameObject target;
	
	/*1 when redy to strike less then 1 when not */
	protected float readyToAttack;

	#endregion

	#region Unity's functions
	protected override void Start (){
		base.Start ();

		currentWaypoint = 0;
		seeker = GetComponent<Seeker>();
		characterController = GetComponent<CharacterController>();
		unitState = State.idle;
	}

	protected override void Update() {
		base.Update();
		RefreshPathTimer();
		switch (unitState) {
		case State.onTraining:
			Training();
			break;
		case State.moving:
			Moving();
			break;
		case State.attack:
			Attacking();
			break;
		case State.idle:
			if(EnemyInSight()) {
				target = FindNearestEnemy();
				unitState = State.attack;
			}
			break;
		}
		if(!isReadyToAttack()) 
			readyToAttack += (attackSpeed * Time.deltaTime) / ATTACK_SPEED_MULTIPLYER;
	}

	#endregion

	#region update functions
	//unit is in training buillding or is on its way to there
	//TODO
	protected void Training() {
		if(path != null) {
			if(EndOfPath()) {
				unitState = State.idle;
				this.gameObject.transform.position = target.transform.position;
				Barracks b = target.GetComponent<Barracks>();
				b.StartTraining(this.gameObject);
			}
			else {
				Moving();
			}
		}
	}

	//unit moves on it's calucated path
	protected void Moving() {
		if(path == null)
			return;

		//unit has come to the end of path
		if(EndOfPath()) {
			unitState = State.idle;
			path = null;
			return;
		}

		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= movementSpeed * Time.deltaTime;
		Vector3 lookingDirection = transform.forward;
		Vector3 goingDirection = path.vectorPath[currentWaypoint].normalized;
		if(lookingDirection != goingDirection) {
			Vector3 lookAt = path.vectorPath[currentWaypoint];
			lookAt.y = transform.position.y;
			transform.LookAt(lookAt);
		}
		characterController.SimpleMove(dir);

		//onward to next waypoint may pesants! :D
		if(Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
		}
	}

	protected void Attacking() {
		if(target) {

			if(TargetInRange() && isReadyToAttack()) { //if target is in range and unit prepared to attack, let him hit the enemy unit
				WorldObject wo = target.GetComponent<WorldObject>();
				wo.Hit(attackDamage);
				readyToAttack = 0.0f;
				transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
			} 


			else if(!TargetInRange() && CanRefreshPathAndReset() && HasTargetMoved()){ //or enemy unit is not in attacking range of this, so it moves closer
				Vector3 dest = target.transform.position;
				seeker.StartPath(transform.position, dest, OnPathComplete);
			}

			if(!TargetInRange()) {
				if(path == null)
					Attack(target);
				else
					Moving();
			}
		} 

		//unit has no target.. therefor entering idle mode
		else {
			unitState = State.idle;
		}
	}
	#endregion

	#region call's functions
	public void GoTrain(GameObject hitObject) {
		target = hitObject;
		unitState = State.onTraining;
		Vector3 dest = hitObject.transform.position;
		dest.x += (hitObject.GetComponent<Renderer>().bounds.size.x/2) * hitObject.transform.forward.x;
		dest.y += (hitObject.GetComponent<Renderer>().bounds.size.y/2) * hitObject.transform.forward.y;
		dest.z += (hitObject.GetComponent<Renderer>().bounds.size.z/2) * hitObject.transform.forward.z;
		seeker.StartPath(transform.position, dest, OnPathComplete);
	}

	public void Attack(GameObject hitObject) {
		target = hitObject;
		unitState = State.attack;
		seeker.StartPath(transform.position, hitObject.transform.position, OnPathComplete);

	}

	public void StartMove(Vector3 dest) {
		target = null;
		if(Input.GetKey(KeyCode.LeftShift))
			unitState = State.attack_move;
		else 
			unitState = State.moving;
		seeker.StartPath (transform.position, dest, OnPathComplete);
	}

	#endregion

	#region override functions

	public override void Hit (float damage) {
		base.Hit (damage);
/*
		if(unitState == State.idle) {

			GameObject hitter = FindNearestEnemy();
			
			float radius = sight;
			while (hitter == null) {
				radius *= sight;
				hitter = FindNearestEnemy(radius);
			}

			if(hitter != null) 
				Attack(hitter);
		}*/
	}	

	#endregion

	#region aid functions
	protected bool TargetInRange() {
		float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
		if(distance <= attackRange)
			return true;

		 if (target.GetComponent<Building>() && distance <= attackRange + 5)
			return true;
		return false;
	}

	public void OnPathComplete(Path p) {
		if(!p.error) {
			if(p != path) {
				path = p;
				currentWaypoint = 0;
			}
		}
	}

	protected bool EnemyInSight() {
		GameObject o = FindNearestEnemy();
		if(o)
			return true;
		return false;
	}

	protected GameObject FindNearestEnemy() {
		return FindNearestEnemy(sight);
	}

	protected GameObject FindNearestEnemy(float radius) {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

		foreach(Collider c in hitColliders) {
			if(c.gameObject.GetComponent<Unit>() || c.gameObject.GetComponent<Building>()) {
				WorldObject wo = c.gameObject.GetComponent<WorldObject>();
				if(wo.Owner != Owner) {
					return c.gameObject;
				}
			}
		}

		return null;
	}

	protected bool isReadyToAttack() {
		if(readyToAttack >= 1)
			return true;
		return false;
	}

	protected bool EndOfPath() {
		if(currentWaypoint >= path.vectorPath.Count)
			return true;
		return false;
	}

	//limits path refreshing for seeking targets every 0.25s (4 times per second maximum if neccesary
	protected float refreshTimer = 0.0f;
	protected const float REFRESH_TIME_LIMITER = 0.25f;

	//updates every frame counter for path refreshing when units attack 
	protected void RefreshPathTimer() {
		if(!CanRefreshPath()) {
			refreshTimer += Time.deltaTime;
		}
	}

	protected bool CanRefreshPath() {
		if(refreshTimer >= REFRESH_TIME_LIMITER)
			return true;
		else
			return false;
	}

	protected bool CanRefreshPathAndReset() {
		bool canRefresh = CanRefreshPath();
		if(canRefresh) 
			refreshTimer = 0.0f;

		return canRefresh;
	}

	protected bool HasTargetMoved() {
		if(target == null || path == null)
			return false;
		Vector3 tv = target.transform.position;
		Vector3 v3 = path.vectorPath[path.vectorPath.Count-1];
		if(Vector3.Distance(tv, v3) == 1)
			return true;
		return false;
	}

	#endregion
}



























