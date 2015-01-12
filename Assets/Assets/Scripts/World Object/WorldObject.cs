using UnityEngine;
using System.Collections;

public abstract class WorldObject : MonoBehaviour {
	
	public int hitPoints;
	public int max_hit_points;
	public Player owner;

	// Use this for initialization
	protected abstract void Start ();
	
	// Update is called once per frame
	protected abstract void Update ();
}
