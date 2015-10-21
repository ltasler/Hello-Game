using UnityEngine;
using System.Collections;
using RTS;

public class Building : WorldObject {

	protected Vector3 spawnPoint;

	protected float creationProgress;
	public GameObject unit; /*unit to be build here prefab */
	public float creationSpeed; /* how many seconds till new unit */

	public int populationSupport; /* how many units is able to support */

	protected Terrain terrain;


	public bool isBuilt;
	public int maxResources;
	protected int broughtResources;
	public int BroughtResources {get {return broughtResources;}}
	protected float buildProgress;

	// Use this for initialization
	protected override void Start () {

		broughtResources = 0;
		buildProgress = 0.0f;

		if(isBuilt) { /* to initialize builds present from begining */
			StartBuilding(owner);

		}
		base.Start ();
	}

	protected Bounds bounds;

	// Update is called once per frame
	protected override void Update () {
		if(hitPoints <= 0) {
			bounds = gameObject.GetComponent<Collider>().bounds;
			base.Update();
			AstarPath.active.UpdateGraphs(bounds, 0);
		}
		if(isBuilt) 
			CreateNewUnit();
	}

	protected virtual void CreateNewUnit() {
		if(owner.Population < owner.PopulationLimit || this is Barracks) {
			if(creationProgress >= 1) {
				Vector3 unitSize = unit.GetComponent<Renderer>().bounds.size;

				/* this could be possible performance issue.. have to check */
				Vector3 newSpawnPoint = spawnPoint;
				newSpawnPoint.y =terrain.SampleHeight(newSpawnPoint) + unitSize.y/2;
				Vector3 sphereCheckVector = newSpawnPoint;
				sphereCheckVector.y += 2;
				while(Physics.CheckSphere(sphereCheckVector, 1,9)) {
					Vector3 rand = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
					rand.Normalize();
					newSpawnPoint += rand ;
					sphereCheckVector += rand;
				}
				
				GameObject u = Instantiate(unit, newSpawnPoint, Quaternion.identity) as GameObject;
				GameManager.SetOwner(u, owner);
				owner.AddUnit(u);
				creationProgress -= 1;
			}
			else {
				creationProgress += Time.deltaTime/creationSpeed;
			}
		}		
	}

	public void StartBuilding(Player player) {
		owner = player;
		isBuilt = true;
		creationProgress = 0;
		spawnPoint = transform.position;
		while(this.GetComponent<Collider>().bounds.Contains (spawnPoint))
			spawnPoint += transform.forward;
		terrain = Terrain.activeTerrain;
		spawnPoint.y = terrain.SampleHeight(spawnPoint) + unit.GetComponent<Renderer>().bounds.size.y;
	}
}























