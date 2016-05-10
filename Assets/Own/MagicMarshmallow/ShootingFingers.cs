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
    private Hand handModel;
    private int elementIndex = 0;
    bool attackChanged;

    private float fireDelay;
    private float startFireDelay = 5;
    private float elementSwitchDelay;

    private bool loadingShoot;

    private Leap.Controller controller;

    private Hand CurrentHand;
    private bool IsHandActive;


    private bool HandActive;


    public GameObject SpawnPoint;

    // Use this for initialization
    void Start()
    {
        Debug.Log("start");
        //Elements = new GameObject[1];
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        this.elementSwitchDelay = 0f;

        controller = new Controller();
        loadingShoot = false;
        fireDelay = this.startFireDelay;
        this.attackChanged = true;


        CurrentHand = null;
        IsHandActive = false;

        // Gesture 
        /*
        controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);


        controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);



        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        */
        //this.handModel = GetComponent<HandModel> ();
    }

    // Update is called once per frame
    void Update()
    {
        bool ActiveGesture = false;

        // Gesture
        Frame frame = controller.Frame();

        if (frame.Hands.Count > 0)
        {
            var hands = frame.Hands;
            Hand firstHand = hands[0];
            this.handModel = firstHand;
        }
        else
        {
            return;
        }


        for (int i = 0; i < frame.Hands.Count; i++ )
        {
            if(frame.Hands[0].PalmVelocity.Magnitude > 1000)
            {
                ActiveGesture = true;

                if(!IsHandActive)
                {
                    CurrentHand = frame.Hands[0];
                    IsHandActive = true;
                    
                }
                Debug.Log("Hand Nr." + i + "PalmVelocity: " + frame.Hands[0].PalmVelocity.Magnitude);
            }
        }

        if(frame.Hands.Count <= 0)
        {
            this.CurrentHand = null;
        }

            //Hand leapHand = handModel.GetLeapHand ();
            //if(leapHand != null) Debug.Log ("Position: " + handModel.EffectPosition.position);
        /*
            if (!this.isActiveAndEnabled)
            {
                //Destroy(currentSmallBall);
                //return;
                //this.fireDelay = 3;

                if (this.currentElement == null)
                {
                    this.spawnElement(0);
                }
            }
            else
            {
               // this.fireDelay = 900f;
            }
        */
        this.fireDelay = fireDelay - Time.deltaTime;
        this.elementSwitchDelay = this.elementSwitchDelay - Time.deltaTime;

        // handModel.Fingers.;

        //Vector3 shootDirection = handModel.fingers[1].GetBoneDirection(3);
        if(CurrentHand != null)
        {
            Vector3 shootDirection = GetIndexFinger(CurrentHand).Direction.ToVector3();
        }
        

        //	Vector3 fingerOrigin = handModel.fingers [1].GetRay ().origin;// handModel.GetPalmPosition ();	//handModel.fingers [1].GetRay ().origin;

        //	Debug.DrawRay (fingerOrigin, shootDirection);


        if (currentElement != null)
        {
            currentElement.transform.position = GetIndexFinger(CurrentHand).TipPosition.ToVector3();
        }


        // if(frame.Hands[0].PalmVelocity.Magnitude > 0)
        {
            //Debug.Log("Log: " + frame.Hands[0].PalmVelocity.Magnitude);
            // Debug.Log("Log: " + frame.Hands[1].PalmVelocity.Magnitude);
        }


        if (ActiveGesture && ( this.CurrentHand != null ) )
        {

            Debug.Log("enter Gesture");
            if( gameController.isInIntroState() )
            {
                gameController.startGame();
                IsHandActive = false;
               return;
            }



            if (this.elementSwitchDelay <= 0f)
            {
                if (this.currentElement != null && !this.loadingShoot)
                {
                    this.GetComponent<AudioSource>().Play();
                    this.loadingShoot = true;
                    //this.shootElement(shootDirection);

                    Debug.Log("Play Audio Shoot");
                }

                Debug.Log("if (this.elementSwitchDelay <= 0f)");
                if (this.currentElement == null)
                {
                    Debug.Log("if (this.currentElement == null)");
                    this.spawnElement(0);
                }
                else
                {

                    Destroy(this.currentElement);
                    elementIndex = elementIndex == 0 ? 1 : 0;
                    this.spawnElement(elementIndex);

                    Debug.Log("Switch Element");

                    Debug.Log("elementSwitchDelay" + this.elementSwitchDelay);
                }





                this.elementSwitchDelay = 5f;
            }
            else
            {
                Debug.Log("Switch blocked");
            }

        }


        if (!this.GetComponent<AudioSource>().isPlaying && this.loadingShoot)
        {
            //this.shootElement(shootDirection);
            this.loadingShoot = false;
        }

        if (fireDelay < 0.0f && currentElement != null)
        {
            //this.shootElement(shootDirection);
           // Debug.Log("shoot");

            
        }

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
        /*
        if (!this.GetComponent<AudioSource> ().isPlaying && this.loadingShoot) 
        {
            //this.shootElement(shootDirection);
            this.loadingShoot = false;
        }

        if (fireDelay < 0.0f && currentElement != null) 
        {
            //this.shootElement(shootDirection);
        }
        */
    }

    private void spawnElement(int elementIndex)
    {
        Debug.Log("spawnElement(int elementIndex)");
        if (this.Elements.Length <= 0)
        {
            Debug.Log("element index: " + elementIndex + "Length: " + this.Elements.Length);
        }
        else
        {


            this.currentElement = (GameObject)Instantiate(this.Elements[elementIndex], GetIndexFinger(CurrentHand).Bone(Bone.BoneType.TYPE_DISTAL).NextJoint.ToVector3(), Quaternion.identity);

            // this.currentElement = (GameObject)Instantiate (this.Elements[elementIndex], handModel.fingers[0].GetTipPosition(), Quaternion.identity);
           // this.currentElement.transform.localScale = this.currentElement.transform.localScale * 0.01f;
            this.currentElement.GetComponent<SphereCollider>().enabled = false;
            this.attackChanged = false;


            Debug.Log("Spawn");
        }
    }

    private Finger GetIndexFinger(Hand hand)
    {
        for (int f = 0; f < CurrentHand.Fingers.Count; f++)
        {
            Finger leapFinger = CurrentHand.Fingers[f];
            if (leapFinger.Type == Finger.FingerType.TYPE_INDEX)
            {
                return leapFinger;
            }
        }

        return null;
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
        ballBig.GetComponent<collide>().setActive();
        fireDelay = this.startFireDelay;
    }

    private void OnDestroy()
    {
        Destroy(this.currentElement);
        //print("Script was destroyed");
    }
}
