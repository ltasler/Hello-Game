using UnityEngine;
using System.Collections;

public abstract class WorldObject : MonoBehaviour {
	
	public int hitPoints;
	public int max_hit_points;
	public Player owner;
	public Player Owner {get {return owner;} set {owner = value;}}
	private Color playerColor;

	// Use this for initialization
	protected virtual void Start () {
		SetColor();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(hitPoints <= 0) {
			Destroy (this.gameObject);
		}
	}

	protected void SetColor() {
		if(owner) {
		playerColor = owner.PlayerColor;
		this.renderer.material.color = playerColor;
		}
	}

	public void Hit(float damage) {
		hitPoints -= (int)damage;
	}
}
