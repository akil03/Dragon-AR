﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using UnityEngine.UI;

public class positionManager : MonoBehaviour {

	public float distancebetween;
	public float distance;
	public float movementThreshold;
	public GameObject recordButton;
	public Text animationTiming;

	public GameObject[] dragonList;
	public GameObject landingSurface;
	public GameObject buttoncontainer;
	public GameObject talkButton;
	public GameObject uiLanding;
	public GameObject selectionScreen;
	public GameObject dragonButton;

	public string[] idleAnimations;
	public static positionManager Instance;
	public string[] hitAnimations;

	public Transform focusCube;
	public Transform dragoninitialPosition;
	public Transform DragonLookat;
	public Transform focuspoint;

	public bool sleep =  false;
	public bool Free = true;
	public bool idleCallled = false;
	public bool flyCalled = true;
	public bool FreeFly = false;
	public bool isCameraMoving;
	public bool voice = false;
	public bool UI;

	public Vector3 prevPosition;
	public Vector3 lastPos;
	public Vector3 newPos;

	public List<CheckVisible> checkVisibleScripts;

	public int index;

	public GameObject dragon;
	public Animator dragonAnimator;

	private float targetDistance = 1000;
	public GameObject targetObject;

	void Start () {
		dragon.SetActive (true);
		lastPos = focusCube.position;
		InvokeRepeating ("CheckCameraMove", 0, 0.5f);
		//ac = dragonAnimator.runtimeAnimatorController;

	}
		
	void Awake() {
		UI = false;
		Instance = this;
		dragon = dragonList[0];
		dragonAnimator = dragon.GetComponent<Animator>();
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
		if (UI == false) {
			uiLanding.SetActive (false);
			if (sleep == false && voice == false) {
				if (FreeFly == false) {
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
	}

	void OnDragonMovement() {
		dragon.transform.LookAt (DragonLookat.transform);
	}

	void LandingCalled() {
		dragonButton.SetActive (false);
		RecordingCanvas.Instance.isIdle = true;
		flyCalled = false;
		Vector3 idlePosition = new Vector3 (dragon.transform.localPosition.x, dragon.transform.localPosition.y - 0.2f, dragon.transform.localPosition.z);
		dragon.transform.DOLocalMove (idlePosition, 0.18f, false).OnComplete(() => Turn180());
		landingSurface.transform.DOLocalMove (new Vector3 (0, -0.835f, 1.2f), 0.5f, false);
		dragon.transform.localEulerAngles = new Vector3 (0, 0, 0);
		dragonAnimator.applyRootMotion = false;
		dragonAnimator.Play ("Fly land");
	}
		
	void IdleAnimationcalled() {
		sleep = false;
		idleCallled = true;
		dragonAnimator.applyRootMotion = false;
		string CurrentAnimation = idleAnimations [Random.Range (0, idleAnimations.Length)];
		dragonAnimator.Play (CurrentAnimation);
		int Random1 = Random.Range (0, 10);
		dragonButton.SetActive (false);
		if (Random1 > 5) {
			Invoke ("SetFree",10f);
		} else {
			Invoke ("SadAnimationCalled", 10f);
		}
	}

	public void takeOffCalled() {
		RecordingCanvas.Instance.isIdle = false;
		CancelInvoke ("SleepAnimationCalled");
		CancelInvoke ("SetFree");
		dragonAnimator.applyRootMotion = false;
		Vector3 flyPosition = new Vector3 (dragon.transform.localPosition.x, dragon.transform.localPosition.y + 0.2f, dragon.transform.localPosition.z);
		landingSurface.transform.DOLocalMove (new Vector3 (0, -2.8f, 1.15f), 0.5f, false);
		dragon.transform.DOLocalMove (flyPosition, 0.14f, false);
		dragonAnimator.Play ("Fly take off");
		dragon.transform.localEulerAngles = new Vector3 (-20, 0, 0);
		Invoke ("FlyCalled", 0.14f);
	}

	void FlyCalled() {
		idleCallled = false;
		dragonButton.SetActive (true);
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
		Invoke ("takeOffCalled",1.02f);
	}
		
	public void Fetchcalled() {
			idleCallled = false;
			CancelInvoke ("LandingCalled");
	}

	public void IdleToFetch() {
		if (sleep == false) {
			recordButton.SetActive (false);
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
		voice = false;
		buttoncontainer.SetActive (true);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("SetFree");
		CancelInvoke ("IdleAnimationcalled");
		landingSurface.transform.DOLocalMove (new Vector3 (0, -0.835f, 1.2f), 0.5f, false);
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
		dragonAnimator.Play ("Sleep enter");
	}

	public void AwakeAnimationCalled() {
		if (sleep == true) {
			CancelInvoke ("LandingCalled");
			CancelInvoke ("IdleAnimationcalled");
			CancelInvoke ("takeOffCalled");
			dragonAnimator.Play ("Sleep exit");
			idleCallled = true;
			flyCalled = false;
			Invoke ("IdleAnimationcalled", 2.05f);
		}
	}

	void Sleepfalse() {
		sleep = false;
	}

	public void EatClicked() {
		talkButton.SetActive (true);
		buttoncontainer.SetActive (false);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("SadAnimationCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("takeOffCalled");
		flyCalled = false;
		Invoke ("SadAnimationCalled",10f);
		dragonAnimator.Play ("Eat");
		fetchscriptTest.Instance.Eat ();
		fetchscriptTest.Instance.Free = true;
		Invoke ("FreeBool",4f);
	}

	public void HateClicked() {
		talkButton.SetActive (true);
		buttoncontainer.SetActive (false);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("takeOffCalled");
		Invoke ("SadAnimationCalled",5f);
		flyCalled = false;
		dragonAnimator.Play (hitAnimations[Random.Range(0,hitAnimations.Length)]);
		fetchscriptTest.Instance.Free = true;
		fetchscriptTest.Instance.ThrowHit ();
		Invoke ("FreeBool",1f);
	}

	public void TalkCalled() {
		talkButton.SetActive (false);
		CancelInvoke ("LandingCalled");
		CancelInvoke ("IdleAnimationcalled");
		CancelInvoke ("takeOffCalled");
		dragonAnimator.Play ("Talk");
	}

	public void SetFree() {
		RecordingCanvas.Instance.isIdle = false;
		dragon.transform.SetParent (null);
		FreeFly = true;
		dragonAnimator.applyRootMotion = true;
		landingSurface.transform.DOLocalMove (new Vector3 (0, -2.8f, 1.15f), 0.5f, false);
		dragonAnimator.Play ("Fly Take off 0");
	}

	public void FreeBool() {
		//recordButton.SetActive (true);
		Free = true;
	}

	public void VoiceFreeFlyCalled () {
		//voice = true;
		CancelInvoke ();
		SetFree ();
	}

	public void VoiceAnimationCalled(string AnimationName) {
		CancelInvoke ();
		dragonAnimator.Play (AnimationName);
		float animatTime = dragonAnimator.GetCurrentAnimatorStateInfo(0).length;
		StartCoroutine (IsreadyforRecording (animatTime));
	}

	IEnumerator IsreadyforRecording(float animationTime) {
		animationTiming.text = animationTime.ToString ();
		yield return new WaitForSeconds (animationTime);

		RecordingCanvas.Instance.isReadyForrecording = true;
	}

	public void VoiceFetchCalled() {
		voice = true;
		CancelInvoke ();
		foreach (CheckVisible target in checkVisibleScripts) {
			if (target.Visible == true) {
				float distancebetween = Vector3.Distance (dragon.transform.position, target.gameObject.transform.position);
				if (distancebetween < targetDistance) {
					targetDistance = distancebetween;
					targetObject = target.gameObject;
				}
			}
		}
		fetchscriptTest.Instance.VoiceFetchCalled (targetObject);

	}

	public void VoiceStopCalled() {
		CancelInvoke ();
		dragonAnimator.Play ("Idle");
	}

	public IEnumerator VoiceCommend(string cmds) {
		CancelInvoke ();
		dragonAnimator.Play (cmds);
		float animatTime = dragonAnimator.GetCurrentAnimatorStateInfo (0).length;
		yield return new WaitForSeconds (animatTime);
		RecordingCanvas.Instance.isReadyForrecording = true;
	}

	public void selectDragon() {
		UI = false;
		dragonButton.SetActive (true);
		selectionScreen.SetActive (false);
		dragonAnimator = dragon.GetComponent<Animator>();
	}

	public void seclectionScreenSlelected() {
		dragonButton.SetActive (false);
		CancelInvoke ();
		UI = true;
		selectionScreen.SetActive (true);
	}
}  