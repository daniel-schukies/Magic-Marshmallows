using UnityEngine;
using System.Collections;
using Leap.Unity;
using Leap;


public class ShootingFingers : MonoBehaviour 
{
	private GameController gameController;

	public GameObject[] Elements;
	public GameObject aBall;
	public GameObject smallBall;
	public int force;
	//private GameObject ballBig;
	private GameObject currentElement;
	private HandModel handModel;
	private int elementIndex =0;
	bool attackChanged;

	private float fireDelay;
	private float startFireDelay = 5;
	private float elementSwitchDelay;

	private bool loadingShoot = false;

	private Leap.Controller controller;

	// Use this for initialization
	void Start () 
	{
		//Elements = new GameObject[1];
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		this.elementSwitchDelay = 1f;

        controller = new Controller();

		fireDelay = this.startFireDelay;
		this.attackChanged = true;

		// Gesture 
        /*
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);


		controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);



		controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        */
		this.handModel = GetComponent<HandModel> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Gesture
		Frame frame = controller.Frame ();


		Hand leapHand = handModel.GetLeapHand ();
		//if(leapHand != null) Debug.Log ("Position: " + handModel.EffectPosition.position);

		if (leapHand == null) 
		{
			//Destroy(currentSmallBall);
			//return;
			//this.fireDelay = 3;

			if (this.currentElement == null)
				this.spawnElement (0);
		} else 
		{
			this.fireDelay = 900f;
		}

		this.fireDelay = fireDelay - Time.deltaTime;
		this.elementSwitchDelay = this.elementSwitchDelay - Time.deltaTime;


		Vector3 shootDirection = handModel.fingers [1].GetBoneDirection (3);

		//Vector3 shootDirection = handModel.GetPalmNormal (); 	//Vector3 shootDirection = handModel.fingers [1].GetRay ().direction;

		Vector3 fingerOrigin = handModel.fingers [1].GetRay ().origin;// handModel.GetPalmPosition ();	//handModel.fingers [1].GetRay ().origin;

		Debug.DrawRay (fingerOrigin, shootDirection);

        /*TODO
		if (currentElement != null) {
			currentElement.transform.position = handModel.EffectPosition.position;
		}
        */

        /*CHECK
		if (frame.Gestures ().Count > 0)
        {
            Debug.Log ("Detected " + frame.Gestures ().Count);
        }
			
		//Gesture
		foreach (Gesture gesture in frame.Gestures()) 
		{
			Debug.Log ("Gesture");
			

			// Spawn Element
			if (gesture.Type == Gesture.GestureType.TYPE_SWIPE) 
			{
				Debug.Log ("TYPE_SWIPE Gesture");

				if( gameController.isInIntroState() ){
					gameController.startGame();
					return;
				}

				if (this.elementSwitchDelay <= 0f) 
				{
					if (this.currentElement == null) 
					{
						this.spawnElement (0);
					} 
					else 
					{

						Destroy (this.currentElement);
						elementIndex = elementIndex == 0 ? 1 : 0;
						this.spawnElement (elementIndex);

						Debug.Log ("Switch Element");

						Debug.Log ("elementSwitchDelay" + this.elementSwitchDelay);
					}


					if(this.currentElement != null && !this.loadingShoot)
					{
						this.GetComponent<AudioSource>().Play();
						this.loadingShoot = true;
						//this.shootElement(shootDirection);
					}

					if (gesture.State.Equals (Gesture.GestureState.STATE_STOP)) 
					{
						Debug.Log ("TYPE_SWIPE Gesture STOP ------------>");
						//this.elementSwitchDelay = 0f;

					}

					this.elementSwitchDelay = 1f;
				} 
				else 
				{
					Debug.Log ("Switch blocked");
				}
			}

			if(gesture.Type == Gesture.GestureType.TYPE_KEY_TAP || gesture.Type == Gesture.GestureType.TYPESCREENTAP)
			{

			}
		}
        */

		if (!this.GetComponent<AudioSource> ().isPlaying && this.loadingShoot) 
		{
			this.shootElement(shootDirection);
			this.loadingShoot = false;
		}

		if (fireDelay < 0.0f && currentElement != null) 
		{
			this.shootElement(shootDirection);
		}

	}

	private void spawnElement(int elementIndex)
	{
		//TODOthis.currentElement = (GameObject)Instantiate (this.Elements[elementIndex], handModel.EffectPosition.position, Quaternion.identity);
		this.currentElement.transform.localScale = this.currentElement.transform.localScale * 0.01f;
		this.currentElement.GetComponent<SphereCollider>().enabled = false;
		this.attackChanged = false;


		Debug.Log("Spawn");

	}


	private void shootElement(Vector3 shootDirection)
	{
		GameObject ballBig = currentElement;

		ballBig.tag = "Attack";
		
		
		currentElement = null;

		ballBig.GetComponent<timeDistanceScale>().setActive();

		//shootDirection = new Vector3(shootDirection.x, shootDirection.y, shootDirection.z);

		
		// Add a rigidbody in order to add forces to it later and get a reference to it
		//Rigidbody rigid = theBall.AddComponent<Rigidbody>();	
		Rigidbody rigid = ballBig.GetComponent<Rigidbody>();
		
		// convert the local direction "forward" of this gameobject to world space
		Vector3 worldDirection = shootDirection;
		
		// add force in world space
		rigid.AddForce(worldDirection * force);
		ballBig.GetComponent<collide> ().setActive ();
		fireDelay = this.startFireDelay;
	}

	private void OnDestroy() 
	{
		Destroy (this.currentElement);
		//print("Script was destroyed");
	}
}
