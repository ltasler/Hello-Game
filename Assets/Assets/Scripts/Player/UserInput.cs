using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour {

	//who's imput is
	private Player player;

	//camera movment variables
	private const int SCROLL_AREA = 25;
	private const int SCROLL_SPEED = 40;
	
	private const int ZOOM_SPEED = 35;
	private const int ZOOM_MIN = 30;
	private const int ZOOM_MAX = 120;
	
	private const int PAN_SPEED = 40;
	private const int PAN_ANGLE_MIN = 30;
	private const int PAN_ANGLE_MAX = 70;

	//selection box start coordinate, end coordinate (on screen) and texture
	private Vector2 startBoxPos, endBoxPos;
	public Texture2D selectionBoxTexture;

	private BuildManager bm;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		startBoxPos = Vector2.zero;
		endBoxPos =  Vector2.zero;
		bm = GetComponent<BuildManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player && player.human) {
			moveCamera();
			MouseActivity();
		}
	}

	void OnGUI() {
		if(startBoxPos != Vector2.zero && endBoxPos != Vector2.zero)
			GUI.DrawTexture(new Rect(startBoxPos.x, Screen.height - startBoxPos.y, endBoxPos.x - startBoxPos.x, -1 * ((Screen.height - startBoxPos.y) - (Screen.height - endBoxPos.y))), selectionBoxTexture);
	}

	private void moveCamera() {
		//init camera trans
		Vector3 translation = Vector3.zero;

		//Zoom in or out
		float zoomDelta = Input.GetAxis ("Mouse ScrollWheel") * ZOOM_SPEED* Time.deltaTime;
		if(zoomDelta != 0) {
			translation -= Vector3.up * zoomDelta * ZOOM_SPEED;
		}

		//pan camera if zooming in close or vice versa
		float pan = Camera.main.transform.eulerAngles.x -zoomDelta * PAN_SPEED;
		pan = Mathf.Clamp(pan, PAN_ANGLE_MIN, PAN_ANGLE_MAX);
		if(zoomDelta < 0 || Camera.main.transform.position.y < (ZOOM_MAX/2))
			Camera.main.transform.eulerAngles = new Vector3(pan, 0, 0);

		//move camera with arrow keys
		translation += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		//move camera if mouse pointer reaches screen borders
		if(Input.mousePosition.x < SCROLL_AREA)
			translation += Vector3.left * SCROLL_SPEED * Time.deltaTime;
		if(Input.mousePosition.x >= Screen.width - SCROLL_AREA)
			translation += Vector3.right * SCROLL_SPEED * Time.deltaTime;
		if(Input.mousePosition.y < SCROLL_AREA)
			translation += Vector3.back * SCROLL_SPEED * Time.deltaTime;
		if(Input.mousePosition.y >= Screen.height -SCROLL_AREA)
			translation += Vector3.forward * SCROLL_SPEED * Time.deltaTime;

		//keep camera within bounds
		Vector3 desiredPosition = Camera.main.transform.position + translation;
		if(desiredPosition.y < ZOOM_MIN || desiredPosition.y > ZOOM_MAX)
			translation.y = 0;

		//moves camera
		Camera.main.transform.position += translation;
	}

	private void MouseActivity() {
		if(bm.PlayerBuilding) {
			if(Input.GetMouseButtonDown(0))
				bm.PlaceBuilding();
			if(Input.GetMouseButtonDown(1))
				bm.CancelBuilding();
		}
			else {
			if(Input.GetMouseButton(0))
				LeftMouseClick();
			if(Input.GetMouseButtonDown(1))
				RightMouseClick();
			if(Input.GetMouseButtonUp(0)) {
				LeftMouseUp();
			}
		}
	}

	private void LeftMouseUp() {
		RaycastHit hit1, hit2;
		Physics.Raycast(Camera.main.ScreenPointToRay(startBoxPos), out hit1);
		Physics.Raycast(Camera.main.ScreenPointToRay(endBoxPos), out hit2);
		
		Vector3 v1 = hit1.point;
		Vector3 v2 = hit2.point;

		if(player.Population > 0) {
			Unit[] allUnits = player.OwnedUnits;
			if(allUnits.Length == 0) return;
			//deselects all selected units
			player.Deselect();

			Rect selectionBox = new Rect(Mathf.Min(v1.x, v2.x),
			                             Mathf.Min (v1.z, v2.z), 
			                             Mathf.Abs (v1.x - v2.x), 
			                             Mathf.Abs(v1.z - v2.z));
		
			foreach(Unit unit in allUnits) {
				Vector3 pos = unit.transform.position;
				Vector2 pos2d = new Vector2(pos.x, pos.z);
				if(selectionBox.Contains(pos2d)) {
					player.Select(unit);
				}
			}
		}
		
		startBoxPos = Vector2.zero;
		endBoxPos = Vector2.zero;
	}

	private void LeftMouseClick() {
		if (Input.GetMouseButtonDown(0)) {
			startBoxPos = Input.mousePosition;
		}
		endBoxPos = Input.mousePosition;		
	}

	private void RightMouseClick() {
		RaycastHit hit;
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
		Vector3 destination = hit.point;
		GameObject[] selectedObjects = player.SelectedObjects;
		if (selectedObjects.Length > 0) {
			foreach(GameObject go in selectedObjects) {
				if(go.GetComponent<Unit>()) {
					Unit u = go.GetComponent<Unit>();
					u.StartMove(destination);
				}
			}
		}
	}
}













