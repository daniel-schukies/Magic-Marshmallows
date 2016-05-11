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

    public int GestureAcceleration = 1100;
    public GameObject SpawnPoint;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        this.elementSwitchDelay = 0f;

        controller = new Controller();
        loadingShoot = false;
        fireDelay = this.startFireDelay;
        this.attackChanged = true;

        CurrentHand = null;
        IsHandActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool ActiveGesture = false;

        // Gesture
        //
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

        // ------------ Check Gesture ------------
        for (int i = 0; i < frame.Hands.Count; i++ )
        {
            if (frame.Hands[0].PalmVelocity.Magnitude > GestureAcceleration)
            {
                ActiveGesture = true;

                // if(!IsHandActive)
                CurrentHand = this.GetComponent<LeapServiceProvider>().CurrentFrame.Hands[0];
                IsHandActive = true;
            }
        }

        // ------------ Check Hand ------------
        if(frame.Hands.Count <= 0)
        {
            this.CurrentHand = null;
        }


        // ------------ Check SpawnPoint ------------
        if(this.currentElement != null)
        {
            if(this.SpawnPoint == null)
            {
               this.currentElement.SetActive(false);
            }
            else
            {
                this.currentElement.SetActive(true);
            }
        }
  


        // ------------ Delay ------------
        this.fireDelay = fireDelay - Time.deltaTime;
        this.elementSwitchDelay = this.elementSwitchDelay - Time.deltaTime;




        // ------------ Move Element  ------------
        if (currentElement != null && SpawnPoint != null)
        {
            currentElement.transform.position = SpawnPoint.transform.position;
            Debug.Log("Spawn Point: " + SpawnPoint.transform.position );
            Debug.Log("Current Element: " + currentElement.transform.position );
        }




        if (ActiveGesture && ( this.CurrentHand != null ) && SpawnPoint != null )
        {
            // ------------ Intro ------------
            Debug.Log("enter Gesture");
            if( gameController.isInIntroState() )
            {
                gameController.startGame();
                IsHandActive = false;
               return;
            }

            // ------------ Shoot Delay ------------
            if (this.currentElement != null && !this.loadingShoot)
            {
                this.GetComponent<AudioSource>().Play();
                this.loadingShoot = true;

                Debug.Log("Play Audio Shoot");
            }
            // ------------ END Shoot Delay ------------


            // ------------ Switch Element ------------
            if (this.elementSwitchDelay <= 0f)
            {


                if (this.currentElement == null)
                {
                    this.spawnElement(0);
                }
                else
                {

                    Destroy(this.currentElement);
                    elementIndex = elementIndex == 0 ? 1 : 0;
                    this.spawnElement(elementIndex);

                    Debug.Log("Switch Element");
                }

                this.elementSwitchDelay = 5f;
            }
            else
            {
                Debug.Log("Switch blocked");
            }
            // ------------ END Switch Element ------------

        }

        // ------------ Shoot ------------
        if (!this.GetComponent<AudioSource>().isPlaying && this.loadingShoot)
        {
            if (CurrentHand != null)
            {
                Vector3 shootDirection = GetIndexFinger(CurrentHand).Direction.ToVector3();

                Debug.Log("SHOOT");
                this.shootElement(this.SpawnPoint.transform.forward);
                this.loadingShoot = false;
            }
        }

        if (fireDelay < 0.0f && currentElement != null)
        {
           //this.shootElement(shootDirection);
           // Debug.Log("shoot");
        }
    }

    private void spawnElement(int elementIndex)
    {
        Debug.Log("spawnElement function");
        if (this.Elements.Length <= 0)
        {
            Debug.Log("element index: " + elementIndex + "Length: " + this.Elements.Length);
        }
        else
        {

            this.Elements[elementIndex].transform.localScale = this.Elements[elementIndex].transform.localScale * 0.01f;
            this.currentElement = (GameObject)Instantiate(this.Elements[elementIndex], SpawnPoint.transform.position, Quaternion.identity);
            this.Elements[elementIndex].transform.localScale = this.Elements[elementIndex].transform.localScale * 100f;
            // this.currentElement = (GameObject)Instantiate (this.Elements[elementIndex], handModel.fingers[0].GetTipPosition(), Quaternion.identity);
            //this.currentElement.transform.localScale = this.currentElement.transform.localScale * 0.01f;
            this.currentElement.GetComponent<SphereCollider>().enabled = false;
            this.attackChanged = false;


            Debug.Log("Spawn ------------------------------------");
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
