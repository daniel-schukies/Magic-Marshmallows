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

    private int HandIndex = -1;


    private bool HandActive;

    public int GestureAcceleration = 1100;
    public GameObject SpawnPoint;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        this.elementSwitchDelay = 0f;

        HandIndex = -1;

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
            if (frame.Hands[i].PalmVelocity.Magnitude > GestureAcceleration)
            {
                ActiveGesture = true;
                
                HandIndex = i;

                IsHandActive = true;
            }

            if(HandIndex == i)
            {
                CurrentHand = this.GetComponent<LeapServiceProvider>().CurrentFrame.Hands[HandIndex];
            }
            
        }

        // ------------ Check Hand ------------
        if(frame.Hands.Count <= 0)
        {
            this.CurrentHand = null;
        }

        if(CurrentHand != null)
        {
        
        }
        

        
        // ------------ Check SpawnPoint ------------
        if(this.currentElement != null)
        {
            if(this.SpawnPoint == null || frame.Hands.Count <= 0 || CurrentHand.IsLeft)
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
        if (currentElement != null )
        {
            currentElement.transform.position = GetIndexFinger(CurrentHand).TipPosition.ToVector3();
            currentElement.transform.localPosition = new Vector3(currentElement.transform.localPosition.x, currentElement.transform.localPosition.y, currentElement.transform.localPosition.z);
            Debug.Log("Spawn Point: " + SpawnPoint.transform.position );
            Debug.Log("Current Element: " + currentElement.transform.position );
        }
        else
        {
            Debug.Log("Current element null");
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
                    this.spawnElement( Random.Range(0, 2) );
                }
                else
                {
                    Destroy(this.currentElement);
                    elementIndex = elementIndex == 0 ? 1 : 0;
                    this.spawnElement(elementIndex);

                    Debug.Log("Switch Element");
                }

                this.elementSwitchDelay = 1f;
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
                this.shootElement(shootDirection);//this.SpawnPoint.transform.forward
                this.loadingShoot = false;
            }
        }

        if (fireDelay < 0.0f && currentElement != null)
        {
           //this.shootElement(shootDirection);
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
            this.currentElement = (GameObject)Instantiate(this.Elements[elementIndex], GetIndexFinger(this.CurrentHand).TipPosition.ToVector3(), Quaternion.identity);
            this.currentElement.transform.localScale = this.currentElement.transform.localScale * 0.015f;
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
    }
}
