using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {

	//how much wood a tree still contains
	public int resourceCount;

	//how long does worker has to "hit" it till he collects a resource
	public float resistance;

	private float workNeeded = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int ChopTree(float workerPower) {
		if(workNeeded == 1) {
			workNeeded = 0.0f;
			return 1;
		}
		else {
			workNeeded += (Time.deltaTime * workerPower) / resistance;
			return 0;
		}
	}
}
