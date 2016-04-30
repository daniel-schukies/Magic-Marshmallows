using UnityEngine;
using System.Collections;

public class introSoundCtrl : MonoBehaviour {

	public AudioClip hitA;
	public AudioClip hitB;

	public AudioClip hitC;
	public AudioClip hitD;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hitA_intro(){
		playClip( hitA);
		playClip (hitD);

	}


	public void hitB_intro(){
		playClip( hitB);
		playClip (hitC);
	}

	private void playClip(AudioClip clip){
		GameObject tmpSound = new GameObject ();
		tmpSound.transform.position = this.transform.position;
		AudioSource tmpSource = tmpSound.AddComponent<AudioSource> ();
		tmpSource.clip = clip;
		tmpSource.Play ();
		Destroy (tmpSound, clip.length+2);
	}

}
