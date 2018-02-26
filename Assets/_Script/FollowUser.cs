using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG;

public class FollowUser : MonoBehaviour {

	//public Transform target;
	private Vector3 lastPos;
	//public Transform dragonFollow;
	//public Text test1;
	public static float height;
	Vector3 newposition;
	public bool moveComplete = true;
	public Ease eastType;

	public Animator dragonAnimator;
	public Transform target;

	public string flyAnimation;
	public string[] idleAnimation;
	public string flyIdle;
	public string flyLand;


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
				
				if (Vector3.Distance (transform.position, objectHit) > 0.15f) {
					moveComplete = false;
					transform.LookAt (objectHit);
					dragonAnimator.Play (flyAnimation);
					transform.DOMove (objectHit, 1f, false).SetEase (eastType).OnComplete (() => onCompleteFunction ());
				}
			}
			//	transform.position = objectHit;
		}
	}

	void Update() {
//		ProperRay ();
//		test1.text = Vector3.Distance (transform.position, target.position).ToString ();


		if ((Vector3.Distance (transform.position, target.position)) > 0.25f) {
			
			float distance = Vector3.Distance (transform.position, target.position);
			transform.LookAt (target);
			dragonAnimator.Play (flyAnimation);
			transform.DOMove (target.position, (distance * 1.5f), false).SetEase (eastType).OnComplete (() => onCompleteFunction ());


		} 
	}

	void onCompleteFunction() {
		lastPos = transform.position;
		moveComplete = true;
		//transform.LookAt (Camera.main.transform);
		dragonAnimator.Play (idleAnimation[Random.Range(0,idleAnimation.Length)]);


	}
}
