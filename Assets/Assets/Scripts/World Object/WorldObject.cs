using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class WorldObject : MonoBehaviour {
	
	public int hitPoints;
	public int max_hit_points;
	public Player owner;
	public Player Owner {get {return owner;} set {owner = value;}}
	private Color playerColor;

	public Slider healthBar;
	private GameObject healthBarobj;


	// Use this for initialization
	protected virtual void Start () {
		
		playerColor = owner.PlayerColor;
		SetColor(this.gameObject);


	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(hitPoints <= 0) {
			Destroy (this.gameObject);
		}
	}

	protected virtual void OnGUI() {
		SetHealthBar ();
	}
	protected void SetColor(GameObject toColor) {
		if(owner) {

			if(toColor.GetComponent<Renderer>())
			toColor.GetComponent<Renderer>().material.color = playerColor;

			//paints all children of object
			for(int i = 0; i < toColor.transform.childCount; i++) {
				SetColor(toColor.transform.GetChild(i).gameObject);
			}
		}
	}

	public virtual void Hit(float damage) {
		if(owner)
			owner.DeselectUnit(this.gameObject);
		hitPoints -= (int)damage;
	}

	private void SetHealthBar () {
		Vector3 pos = transform.position;
	}
}























