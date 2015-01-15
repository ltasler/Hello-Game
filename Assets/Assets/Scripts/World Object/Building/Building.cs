using UnityEngine;
using System.Collections;

public class Building : WorldObject {

	protected Vector3 spawnPoint;

	protected float creationProgress;
	public GameObject unit; /*unit to be build here prefab */
	public float creationSpeed; /* how many seconds till new unit */

	public int populationSupport; /* how many units is able to support */

	protected Terrain terrain;

	public bool isBuilt;

	// Use this for initialization
	protected override void Start () {
		if(isBuilt) { /* to initialize builds present from begining */
			StartBuilding(owner);
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(isBuilt) 
			CreateNewUnit();
	}
	
	protected virtual void CreateNewUnit() {
		if(owner.Population < owner.PopulationLimit || this is Barracks) {
			if(creationProgress >= 1) {
				Vector3 unitSize = unit.renderer.bounds.size;
				/* this could be possible performance issue.. have to check */
				Vector3 newSpawnPoint = spawnPoint;
				newSpawnPoint.y =terrain.SampleHeight(newSpawnPoint) + unitSize.y/2;
				while(Physics.CheckSphere(newSpawnPoint, unitSize.z/2)) {
					Vector3 rand = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
					rand.Normalize();
					newSpawnPoint += rand ;
				}
				
				GameObject u = Instantiate(unit, newSpawnPoint, Quaternion.identity) as GameObject;
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
		while(this.collider.bounds.Contains (spawnPoint))
			spawnPoint += transform.forward;
		terrain = Terrain.activeTerrain;
		spawnPoint.y = terrain.SampleHeight(spawnPoint) + unit.renderer.bounds.size.y;
	}
}
