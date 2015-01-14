using UnityEngine;
using System.Collections;

namespace RTS {

	public class GameManager {

		public static Vector3 InvalidPosition {get {return new Vector3(-9999, -9999, -9999);}}

		public static Vector3 MouseToWorldPosition() {
			Vector3 mousePosition = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit))
				return hit.point;
			return InvalidPosition;
		}
	}
}
