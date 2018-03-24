using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using KKSpeech;

public class RecordingCanvas : MonoBehaviour {

	public static RecordingCanvas Instance;

	public Button startRecordingButton;
	public Text resultText;

	public string spokenWords;
	public string lastSpokenWords; 
	private bool waitforUpdate;
	public string partialString;

	private Animator dragonAnimator;
	public Text Text1;
	public Text Text2;
	public bool isIdle = false;

	public bool isReadyForrecording = true;
	public List<string> pickUpCmds,freeflyCmds,sadCmds,sleepCmds,consfusedCmds,awakeCmds,talkCmds,swimCmds,happyCmds,digCmds,flyrollCmds,deathCmds,angryCmds,eatCmds,stopCmds,attackCmds;
	public List<AnimationData> animationDatas;

	void Start() {
		dragonAnimator = positionManager.Instance.dragonAnimator;
		if (SpeechRecognizer.ExistsOnDevice()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			//listener.
			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
			startRecordingButton.enabled = false;
			SpeechRecognizer.RequestAccess();
		} else {
			resultText.text = "Sorry, but this device doesn't support speech recognition";
			startRecordingButton.enabled = false;
		}
	}

	void Awake() {
		Instance = this;
	}

	void Update() {
		if (isReadyForrecording == true  && isIdle == true ) {
			OnStartRecordingPressed ();
		}
	}

	IEnumerator waitTime() 
	{
		yield return new WaitForSeconds (1.5f);
		if (spokenWords == lastSpokenWords) 
		{	
			spokenWords = "";
			lastSpokenWords = "";
			SpeechRecognizer.StopIfRecording ();
		} 
		else 
		{
			lastSpokenWords = spokenWords;
			StartCoroutine (waitTime());
		}
	}

	IEnumerator CurrentAnimation(string results)
	{
		string[] split = results.Split(' ');
		if (results == "bye") {
			positionManager.Instance.takeOffCalled ();
			resultText.text = "...";
			positionManager.Instance.idleCallled = false;
			positionManager.Instance.voice = false;

		} else {
			foreach (var item in split) {
				if (pickUpCmds.Contains (item)) {
					positionManager.Instance.VoiceFetchCalled ();
				} else if (freeflyCmds.Contains (item)) {
					positionManager.Instance.VoiceFreeFlyCalled ();
				} else if (attackCmds.Contains (item)) {
					float random = Random.Range (0, 10);
					if (random < 2)
						yield return StartCoroutine (positionManager.Instance.VoiceCommend ("Attack tail"));
					else if (random >= 2 && random < 4)
						yield return StartCoroutine (positionManager.Instance.VoiceCommend ("Attack Stump"));
					else if (random >= 4 && random < 6)
						yield return StartCoroutine (positionManager.Instance.VoiceCommend ("Attack Paw R"));
					else if (random >= 6 && random < 8)
						yield return StartCoroutine (positionManager.Instance.VoiceCommend ("Attack Paw L"));
					else if (random >= 8 && random <= 10)
						yield return StartCoroutine (positionManager.Instance.VoiceCommend ("Attack head"));
				} else {
					foreach (var animationData in animationDatas) {
						if (animationData.words.Contains (item)) {
							yield return StartCoroutine (positionManager.Instance.VoiceCommend (animationData.animationName));
						}	
					}
				} 
			} 
			isReadyForrecording = true;
		}
	}

	public void OnFinalResult(string result) {
		spokenWords = result;
		string results = result.ToLower ();
		resultText.text = result;

		StartCoroutine (CurrentAnimation(results));
	}

	IEnumerator Temp(string result)
	{
		string[] words = result.Split (new string[] { " " }, System.StringSplitOptions.None);
		foreach (string animationname in words) {
				dragonAnimator.Play (animationname.ToLower());
				float animatTime = dragonAnimator.GetCurrentAnimatorStateInfo (0).length;
				yield return new WaitForSeconds (animatTime);
		}
		isReadyForrecording = true;
	}

	public void OnPartialResult(string result) {
		resultText.text = result;
		spokenWords = result;
		StartCoroutine (waitTime());
	}

	public void OnAvailabilityChange(bool available) {
		startRecordingButton.enabled = available;
		if (!available) {
			resultText.text = "Speech Recognition not available";
		} else {
			resultText.text = "Say something :-)";
		}
	}

	public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		switch (status) {
		case AuthorizationStatus.Authorized:
			startRecordingButton.enabled = true;
			break;
		default:
			startRecordingButton.enabled = false;
			resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
			break;
		}
	}

	public void OnEndOfSpeech() {
		startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	}

	public void OnError(string error) {
		OnStartRecordingPressed ();
	}

	public void OnStartRecordingPressed() 
	{
		isReadyForrecording = false;
		positionManager.Instance.voice = true;
		if (SpeechRecognizer.IsRecording()) {
			SpeechRecognizer.StopIfRecording();
			startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
		} else {
			SpeechRecognizer.StartRecording(true);
			startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
			resultText.text = "Say something :-)";
		}
	}
}



[System.Serializable]
public class AnimationData
{
	public List<string> words;
	public string animationName;
}