using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG;


public class fetchscriptTest : MonoBehaviour {

	public static fetchscriptTest Instance;

	private GameObject dragon;
	private GameObject target;

	public Vector3 initialPosition;
	public Vector3 hitPosition;

	public bool hit = false;
	public bool Free = true;
	private bool Fetch = false;

	public Transform targetholdingpoint;
	public Transform dragoninitialPosition;

	private RaycastHit Hit;

	void Start() {
		dragon = positionManager.Instance.dragon;
		Vector3 dragonLandingPosition = new Vector3 (dragon.transform.position.x, dragon.transform.position.y - 0.2f, dragon.transform.position.z);
		dragoninitialPosition.position = dragonLandingPosition;
	}

	void Awake() {
		Instance = this;
	}
		void Update()
	{
		if (Input.GetMouseButton (0)) {
			if (Free == true&& Fetch == false) {
				if (hit == false) {
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out Hit)) {
						Fetch = true;
						if (Hit.transform.gameObject.tag == "Player" || Hit.transform.gameObject.tag == "UI") {
							return;
						} else {
							target = Hit.transform.gameObject;
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
		dragon.transform.DOMove (Hit.point, 2f, false).OnComplete (() =>onCompleteCalled() );
	}

	public void FetchBackMovement() {
		print (dragoninitialPosition.position);
		dragon.transform.DOMove (dragoninitialPosition.position, 2f, false).OnComplete(() => onRetreveFinished());
	}

	public void onRetreveFinished() {
		dragon.transform.position = dragoninitialPosition.position;
		Free = false;
		Fetch = false;
		Hit.transform.SetParent (Camera.main.transform);
		Hit.transform.localPosition = new Vector3 (0,-0.462f,1.05f);
		Hit.transform.localScale = new Vector3 (0.5f,0.5f,0.5f);
		positionManager.Instance.FetchComplete ();
	}

	public void ThrowHit() {
		print (target.transform.gameObject.name);
		Invoke("Targetdestroy",0.5f);
	}

	public void Eat() {
		Invoke("Targetdestroy",4f);
	}

	void Targetdestroy() {
		target.transform.gameObject.SetActive(false);
	}
}
