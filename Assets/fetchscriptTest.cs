using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG;


public class fetchscriptTest : MonoBehaviour {


	public bool hit = false;
	public GameObject dragon;
	public Vector3 initialPosition;
	public Vector3 hitPosition;
	public static fetchscriptTest Instance;

	RaycastHit Hit;
	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
		Instance = this;
	}
	// Update is called once per frame

		void Update()
	{
		if (Input.GetMouseButton (0)) {
			if (hit == false) {
				hit = true;

				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out Hit )) {
					initialPosition = dragon.transform.position;
					hitPosition = Hit.point;
					positionManager.Instance.IdleToFetch ();
					positionManager.Instance.Fetchcalled ();
				}
			}
		}
	}

	public void onCompleteCalled() {
		positionManager.Instance.Fetchcalled ();
		dragon.transform.LookAt (Camera.main.transform);
		Hit.transform.SetParent (dragon.transform);
		FetchBackMovement ();
		hit = false;
	}


	public void FetchMovement() {
		dragon.transform.LookAt (Hit.transform);
		dragon.transform.DOMove (Hit.point, 2f, false).OnComplete (() =>onCompleteCalled() );
	}

	public void FetchBackMovement() {
		//initialPosition.y +=  0.2f;
		dragon.transform.DOMove (initialPosition, 2f, false).OnComplete(() => onRetreveFinished());
	}

	public void onRetreveFinished() {
		Hit.transform.localPosition = new Vector3 (Hit.transform.localPosition.x, Hit.transform.localPosition.y - 1f, Hit.transform.localPosition.z);
		Hit.transform.SetParent (null);
		positionManager.Instance.FetchComplete ();
	}
}
