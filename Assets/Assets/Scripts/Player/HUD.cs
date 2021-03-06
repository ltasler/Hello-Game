﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	private Player player;
	private GameObject panel;
	private Text text;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<Player>();
		panel = GameObject.Find(gameObject.name + "/Panel");
		text = panel.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		if(player && player.human) {
			Draw();
		}
	}

	void Draw() {
		if(player.SelectedObjects.Length > 0 && player.SelectedObjects[0]) {
			string selectionName = player.SelectedObjects[0].name;
			text.text = selectionName;
		}
		else {
			text.text = "hello :)";
		}
	}

	public void ButtonClick() {

	}
}
