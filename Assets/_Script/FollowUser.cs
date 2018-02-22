using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG;

public class FollowUser : MonoBehaviour {

	public Transform target;
	private Vector3 lastPos;
	public Transform dragonFollow;
	public Text test1;
	public static float height;
	Vector3 newposition;
	bool moveComplete = true;
	public Ease eastType;

	void Start () {
	}
	void RetardedRayCalled () {
		RaycastHit hit;
		Transform cam = Camera.main.transform;
		Ray ray = new Ray(cam.position, cam.forward);
		if (Physics.Raycast(ray, out hit)) {
			Vector3 objectHit = hit.point;
			transform.LookAt(objectHit);
//			if ((Vector3.Distance(transform.position,objectHit)) > 0.1f) {
//				transform.Translate (Vector3.forward * (2*Time.deltaTime), Space.Self);
//			}
		}
    }

	void ProperRay(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/3, Screen.height/4, 0));
		if (Physics.Raycast(ray, out hit)) {
			Vector3 objectHit = hit.point;
			if (hit.transform.gameObject != gameObject)
			if (moveComplete == true) {
				moveComplete = false;
				transform.DOMove (objectHit, 1f, false).SetEase(eastType).OnComplete (() => moveComplete = true);
			}
			//	transform.position = objectHit;
		}
	}

	void Update() {
		ProperRay ();
//		test1.text = Vector3.Distance (transform.position, target.position).ToString ();
//		if ((Vector3.Distance(transform.position,target.position)) > 0.2f) {
//			//transform.LookAt (dragonFollow);
//			transform.position = Vector3.Lerp (transform.position, new Vector3(dragonFollow.position.x,height,dragonFollow.position.z), 2 * Time.deltaTime);
//			//transform.Translate (Vector3.forward * (2*Time.deltaTime), Space.Self);
//		}
	}
}
