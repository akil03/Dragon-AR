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
	public Transform targetholdingpoint;
	public bool Free = true;
	public Transform dragoninitialPosition;

	GameObject target;
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
			print (hit);
			if (Free == true) {
				if (hit == false) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out Hit)) {
						print (Hit.transform.gameObject.name);
						target = Hit.transform.gameObject;
						if (Hit.transform.gameObject.tag == "Player" || Hit.transform.gameObject.tag == "UI") {
							return;
						} else {
							hitPosition = Hit.point;
							positionManager.Instance.IdleToFetch ();
							positionManager.Instance.Fetchcalled ();
						}
					}
				}
			}
		}
	}

	public void onCompleteCalled() {
		dragon.transform.LookAt (Camera.main.transform);
		Hit.transform.SetParent (targetholdingpoint);
		Hit.transform.localPosition = new Vector3 (0,0,0);
		FetchBackMovement ();
		hit = false;
	}



	public void FetchMovement() {
		dragon.transform.LookAt (Hit.transform);
		//initialPosition.y = initialPosition.y +0.1f;
		dragon.transform.DOMove (Hit.point, 2f, false).OnComplete (() =>onCompleteCalled() );
	}

	public void FetchBackMovement() {
		//initialPosition.y = initialPosition.y -0.1f;
		dragon.transform.DOMove (dragoninitialPosition.position, 2f, false).OnComplete(() => onRetreveFinished());
	}

	public void onRetreveFinished() {
		//Hit.transform.position = new Vector3 (Hit.transform.position.x, Hit.transform.position.y-0.08f, Hit.transform.position.z - 0.12f);

		Hit.transform.SetParent (Camera.main.transform);
		Hit.transform.localPosition = new Vector3 (0,-0.462f,1.05f);
		Hit.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		positionManager.Instance.FetchComplete ();

	}

	public void ThrowHit() {
		Invoke("Targetdestroy",0.5f);
	}

	public void EatHit() {
		Invoke("Targetdestroy",4f);
	}

	void Targetdestroy() {
		target.transform.gameObject.SetActive(false);
	}


}
