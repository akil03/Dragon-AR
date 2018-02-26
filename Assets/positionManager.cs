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

	public bool invokeCalled = false;
	public bool flyCalled = true;

	void Start () {
		lastPos = focusCube.position;
	}

	void Awake() {
		Instance = this;
	}



	void Update () {
		if(sleep == false) {
		newPos = focusCube.position;
		distance = Vector3.Distance (lastPos, newPos);
		if (distance < 0.1) {
			if (invokeCalled == false) {
				invokeCalled = true;
				Invoke ("LandingCalled", 5f);
				lastPos = focusCube.position;
			}
		} else {
			invokeCalled = false;
			dragonAnimator.applyRootMotion = false;
			dragonAnimator.Play ("JumpUp");
			CancelInvoke ("LandingCalled");
			CancelInvoke ("IdleAnimationcalled");
			if(flyCalled == false) {
				flyCalled = true;
				Invoke ("ReverseTurn180",0.1f);
			 	invokeCalled = false;

			}
			lastPos = focusCube.position;
		}
		}
		
	}

	void LandingCalled() {
		flyCalled = false;
		Vector3 idlePosition = new Vector3 (dragon.transform.localPosition.x, dragon.transform.localPosition.y - 0.2f, dragon.transform.localPosition.z);
		dragon.transform.DOLocalMove (idlePosition, 0.18f, false).OnComplete(() => Turn180());
		dragon.transform.localEulerAngles = new Vector3 (0, 0, 0);
		dragonAnimator.applyRootMotion = false;
		dragonAnimator.Play ("Fly Land");
	}


	void IdleAnimationcalled() {
		sleep = false;
		invokeCalled = true;
		dragonAnimator.applyRootMotion = false;
		string CurrentAnimation = idleAnimations [Random.Range (0, idleAnimations.Length)];

		dragonAnimator.Play (CurrentAnimation);
		Invoke ("SadAnimationCalled",10f); 
		//print(dragonAnimator.GetCurrentAnimatorStateInfo (0).length);

	}

	void TackOffCalled() {
		dragonAnimator.applyRootMotion = false;
		Vector3 flyPosition = new Vector3 (dragon.transform.localPosition.x, dragon.transform.localPosition.y + 0.2f, dragon.transform.localPosition.z);
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
		//if (sleep == false) {
			invokeCalled = false;
			CancelInvoke ("LandingCalled");
		//}
	}

	public void IdleToFetch() {
		if (sleep == false) {
			if (flyCalled == false) {
				dragonAnimator.applyRootMotion = false;
				ReverseTurn180 ();
				Invoke ("FetchScriptCallback", 1.3f);
			} else {
				FetchScriptCallback ();
			}
		}
	}

	public void FetchComplete() {
		dragonAnimator.applyRootMotion = true;
		dragonAnimator.Play ("JumpUp");
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
		dragonAnimator.Play ("Sleep Enter");
	}

	public void AwakeAnimationCalled() {
		
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("TackOffCalled");
		dragonAnimator.Play ("Sleep Exit");
		invokeCalled = true;
		flyCalled = false;
		Invoke ("IdleAnimationcalled",2.05f);


	}

	void Sleepfalse() {
		sleep = false;
	}
}
