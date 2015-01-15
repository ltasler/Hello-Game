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

		/*returns hitted world object as gameObject */
		public static GameObject HitObject() {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				GameObject hitObject = hit.collider.gameObject;
				if(hitObject) {
					if(hitObject.GetComponent<WorldObject>()) {
						return hitObject;
					}
					else if(hitObject.GetComponentInParent<WorldObject>()) {
						WorldObject w = hitObject.GetComponentInParent<WorldObject>();
						return w.gameObject;
					} 
				}
			}
			return null;
		}
	}
}
