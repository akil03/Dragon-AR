using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class positionManager : MonoBehaviour {

	public GameObject dragon;
	public Vector3 lastPos;
	public Vector3 newPos;
	public float distance;
	public Transform focusCube;
	public int idleTimer = 5;
	public Animator dragonAnimator;
	public string[] idleAnimations;
	public static positionManager Instance;
	public bool sleep =  false;
	public Transform focuspoint;
	public GameObject landingSurface;
	public string[] hitAnimations;
	public GameObject buttoncontainer;
	public GameObject talkButton;
	public Transform dragoninitialPosition;
	public Transform DragonLookat;

	public bool Free = true;
	public bool idleCallled = false;
	public bool flyCalled = true;
	public float distancebetween;
	public bool FreeFly = false;

	public bool isCameraMoving;
	public float movementThreshold;
	public Vector3 prevPosition;
	void Start () {
		lastPos = focusCube.position;
		InvokeRepeating ("CheckCameraMove", 0, 0.5f);
	}

	void Awake() {
		Instance = this;
	}

	void CheckCameraMove(){
		float distance = Vector3.Distance (prevPosition, focusCube.position);
		if (distance > movementThreshold)
			isCameraMoving = true;
		else
			isCameraMoving = false;

		prevPosition = focusCube.position;
	}

	void Update () {
		if (sleep == false) {
			if(FreeFly == false) {
				if (Free == true) {
					newPos = focusCube.position;
					distance = Vector3.Distance (lastPos, newPos);
					if (distance < 0.4) {
						if (idleCallled == false) {
							idleCallled = true;
							CancelInvoke ("SetFree");
							CancelInvoke ("LandingCalled");
							CancelInvoke ("IdleAnimationcalled");
							CancelInvoke ("SadAnimationCalled");
							Invoke ("LandingCalled", 5f);
							lastPos = focusCube.position;
						} 
					} else {
						dragon.transform.SetParent (Camera.main.transform);
						idleCallled = false;
						dragonAnimator.applyRootMotion = false;
						CancelInvoke ("SetFree");
						CancelInvoke ("LandingCalled");
						CancelInvoke ("IdleAnimationcalled");
						CancelInvoke ("SadAnimationCalled");
						if (flyCalled == false) {
							flyCalled = true;
							Invoke ("ReverseTurn180", 0.1f);
							idleCallled = false;
						}
						lastPos = focusCube.position;
					}
				}
			}
		}
		if (FreeFly == true) {
			float distancebetween = Vector3.Distance (Camera.main.transform.position, dragon.transform.position);
			if (distancebetween > 4f) {
				dragon.transform.LookAt (Camera.main.transform);
			}
		}
	}

	void OnDragonMovement() {
		dragon.transform.LookAt (DragonLookat.transform);
	}

	void LandingCalled() {
		flyCalled = false;
		Vector3 idlePosition = new Vector3 (dragon.transform.localPosition.x, dragon.transform.localPosition.y - 0.2f, dragon.transform.localPosition.z);
		dragon.transform.DOLocalMove (idlePosition, 0.18f, false).OnComplete(() => Turn180());
		landingSurface.transform.DOLocalMove (new Vector3 (0, -0.8f, 1.15f), 0.5f, false);
		//landingSurface.SetActive (true);
		dragon.transform.localEulerAngles = new Vector3 (0, 0, 0);
		dragonAnimator.applyRootMotion = false;
		dragonAnimator.Play ("Fly Land");
	}


	void IdleAnimationcalled() {
		sleep = false;
		idleCallled = true;
		dragonAnimator.applyRootMotion = false;
		string CurrentAnimation = idleAnimations [Random.Range (0, idleAnimations.Length)];

		dragonAnimator.Play (CurrentAnimation);
		int Random1 = Random.Range (0, 10);
		if (Random1 > 5) {
			
			Invoke ("SetFree",10f); 
		} else {
			Invoke ("SadAnimationCalled", 10f);
		}
	}

	void TackOffCalled() {
		dragonAnimator.applyRootMotion = false;
		Vector3 flyPosition = new Vector3 (dragon.transform.localPosition.x, dragon.transform.localPosition.y + 0.2f, dragon.transform.localPosition.z);
		landingSurface.transform.DOLocalMove (new Vector3 (0, -2.8f, 1.15f), 0.5f, false);
		dragon.transform.DOLocalMove (flyPosition, 0.14f, false);
		dragonAnimator.Play ("Fly Take off");
		dragon.transform.localEulerAngles = new Vector3 (-20, 0, 0);
		Invoke ("FlyCalled", 0.14f);
	}

	void FlyCalled() {
		
		dragonAnimator.Play ("Fly Foward");
	}

	void Turn180() {
		dragonAnimator.applyRootMotion = true;
		dragonAnimator.Play ("Turn 180 L");
		Invoke ("IdleAnimationcalled",1.02f);
	}

	void ReverseTurn180() {
		dragonAnimator.applyRootMotion = true;
		dragonAnimator.Play ("Turn 180 L");
		Invoke ("TackOffCalled",1.02f);
	}


	public void Fetchcalled() {
			idleCallled = false;
			CancelInvoke ("LandingCalled");
	}

	public void IdleToFetch() {
		if (sleep == false) {
			if (flyCalled == false) {
				fetchscriptTest.Instance.Free = false;
				dragonAnimator.applyRootMotion = false;
				ReverseTurn180 ();
				CancelInvoke ("LandingCalled");
				CancelInvoke ("IdleAnimationcalled");
				CancelInvoke ("SleepAnimationCalled");
				Invoke ("FetchScriptCallback", 1.3f);
			} else {
				FetchScriptCallback ();
			}
		}
	}

	public void FetchComplete() {
		Free = false;
		buttoncontainer.SetActive (true);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		//landingSurface.SetActive (true);
		landingSurface.transform.DOLocalMove (new Vector3 (0, -0.8f, 1.15f), 0.5f, false);
		CancelInvoke ("SleepAnimationCalled");
		Invoke ("IdleAnimationcalled", 10f);
		idleCallled = true;
		flyCalled = false;
		dragon.transform.LookAt (focuspoint);
		dragon.transform.SetParent (Camera.main.transform);
		FreeFly = false;
		dragonAnimator.Play ("Happy 1");

	}

	void FetchScriptCallback(){
		
		fetchscriptTest.Instance.FetchMovement ();
	}

	void SadAnimationCalled() {
		dragonAnimator.Play ("Sad");
		Invoke ("SleepAnimationCalled",8.08f);
	}

	void SleepAnimationCalled() {
		
		sleep = true;
		//dragonAnimator.applyRootMotion = true;
		dragonAnimator.Play ("Sleep Enter");
	}

	public void AwakeAnimationCalled() {
		if (sleep == true) {
			CancelInvoke ("LandingCalled");
			CancelInvoke ("IdleAnimationcalled");
			CancelInvoke ("TackOffCalled");
			dragonAnimator.Play ("Sleep Exit");
			idleCallled = true;
			flyCalled = false;
			Invoke ("IdleAnimationcalled", 2.05f);
		}
	}
	

	void Sleepfalse() {
		sleep = false;

	}

	public void EatClicked() {
		buttoncontainer.SetActive (false);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("TackOffCalled");
		flyCalled = false;
		Invoke ("SadAnimationCalled",10f); 
		dragonAnimator.Play ("Eat_Drink");
		fetchscriptTest.Instance.EatHit ();
		fetchscriptTest.Instance.Free = true;
		Free = true;
	}

	public void HateClicked() {
		buttoncontainer.SetActive (false);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("TackOffCalled");
		Invoke ("SadAnimationCalled",5f); 
		//idleCallled = false;
		flyCalled = false;
		Free = true;
		dragonAnimator.Play (hitAnimations[Random.Range(0,hitAnimations.Length)]);
		fetchscriptTest.Instance.Free = true;
		fetchscriptTest.Instance.ThrowHit ();
	}

	public void TalkCalled() {
		talkButton.SetActive (false);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("TackOffCalled");
		dragonAnimator.Play ("Talk");

	}

	public void SetFree() {
		dragon.transform.SetParent (null);
		FreeFly = true;

		dragonAnimator.applyRootMotion = true;
		landingSurface.transform.DOLocalMove (new Vector3 (0, -2.8f, 1.15f), 0.5f, false);
		dragonAnimator.Play ("Fly Take off 0");
	}
}
 