using UnityEngine;
using System.Collections;

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
	
	// Use this for initialization
	void Start () {
		player = transform.root.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player && player.human) {
			moveCamera();
		}
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

}













