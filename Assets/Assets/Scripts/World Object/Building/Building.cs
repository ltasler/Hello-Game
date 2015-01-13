using UnityEngine;
using System.Collections;

public class Building : WorldObject {

	protected Vector3 spawnPoint;

	protected float creationProgress;
	public GameObject unit;
	public float creationSpeed; /* how many seconds till new unit */

	public int populationSupport; /* how many units is able to support */

	private Terrain terrain;

	// Use this for initialization
	protected override void Start () {
		creationProgress = 0;
		spawnPoint = transform.position;
		while(this.collider.bounds.Contains (spawnPoint))
			spawnPoint += transform.forward;
		terrain = Terrain.activeTerrain;
		spawnPoint.y = terrain.SampleHeight(spawnPoint) + unit.renderer.bounds.size.y; 
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(owner.Population < owner.PopulationLimit) 
			CreateNewUnit();
	}
	
	protected void CreateNewUnit() {
		if(creationProgress >= 1) {
			/* this could be possible performance issue.. have to check */
			Vector3 newSpawnPoint = spawnPoint;
			/*while(Physics.CheckSphere(gameObject.transform.position, 1)) {
				Vector3 rand = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
				rand.Normalize();
				newSpawnPoint += rand * 5;
			}*/
			
			GameObject u = Instantiate(unit, newSpawnPoint, Quaternion.identity) as GameObject;
			owner.AddUnit(u);
			creationProgress -= 1;
		}
		else {
			creationProgress += Time.deltaTime/creationSpeed;
		}
		
	}

}
