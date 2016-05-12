/** 
 * 
 * Copyright © 2016 by Daniel Schukies 
 * 
 * **/


using UnityEngine;
using System.Collections;

public class collidePlayer : MonoBehaviour 
{
	private Player player;
	private float startHeight;
	private Vector3 position;

	public float fallSpeed;
    public float PlayerLifeHeight = 1.0f;
    public float ReceiveDamage = 0.01f;

    private float DodgeX = 0;
    private float DodgeZ = 0;
    private float DodgeRange = 150.0f;
    private Vector3 startPostion;

	public AudioClip[] hitSounds;
	public string dmgColliderTag;

    public float startfireDelay = 10f;
    private float fireDelay;

    public GameObject spawnPoint;
    public bool Dodge = false;
    public bool reg = false;
    public bool blockRotation = false;
    public bool delayCorrection = false;
    

    private Vector3 StartSpawnPosition;
	private Animator animator;
	private int hitHash = Animator.StringToHash("HitTrigger");
	GameController gameController;

	void OnCollisionEnter(Collision collisionInfo)
	{
		if(collisionInfo.collider.tag == dmgColliderTag)
		{
			AudioSource source = GetComponent<AudioSource>();
			source.clip = hitSounds[Random.Range(0,hitSounds.Length)];
			source.Play();

            this.player.decrease(ReceiveDamage);

			this.animator.SetTrigger(hitHash);
		}
	}

	// Use this for initialization
	void Start () 
	{
        hitHash = Animator.StringToHash("HitTrigger");
        fireDelay = this.startfireDelay;
        this.DodgeX = 0;
        this.DodgeX = 0;
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		animator = GetComponent<Animator>();
        this.player = new Player(PlayerLifeHeight);
		setStartHeight ();
        StartSpawnPosition = this.spawnPoint.transform.position;
        this.startPostion = this.transform.localPosition;
	}

    public void resetSpawnPosition()
    {
        this.spawnPoint.transform.position = StartSpawnPosition;
    }

	public void setStartHeight()
    {
		this.startHeight = this.gameObject.transform.position.y;
		this.player.reset ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        this.fireDelay = fireDelay - Time.deltaTime;

        if (this.fireDelay <= 0)
        {
            if (Dodge)
            {
                if (this.DodgeX != 0)
                {
                    DodgeX = 0;
                }
                else
                {
                    DodgeX = RandomFloat(2, DodgeRange);
                }

                if (this.DodgeZ != 0.0f)
                {
                    DodgeZ = 0.0f;
                }
                else
                {
                    DodgeZ = RandomFloat(2, DodgeRange);
                }
            }
            this.delayCorrection = !this.delayCorrection;

            if (!delayCorrection)
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

            this.fireDelay = startfireDelay;
        }

		if (gameController.isInIntroState ()) return;

        this.position = this.gameObject.transform.position;

        if(reg)
        {
           float newHeight = Mathf.Lerp(this.spawnPoint.transform.position.y, this.startHeight * this.player.life, fallSpeed * Time.deltaTime);
           this.spawnPoint.transform.position = new Vector3(this.spawnPoint.transform.position.x, newHeight, this.spawnPoint.transform.position.z);
        }
        else
        {
            float newHeight = Mathf.Lerp(this.transform.position.y, this.startHeight * this.player.life, fallSpeed * Time.deltaTime);
            this.gameObject.transform.position = new Vector3(position.x, newHeight, position.z);
        }

		
        // Dodge never used
        if (Dodge)
        {
            float x = Mathf.Lerp(this.transform.localPosition.x, startPostion.x + DodgeX, fallSpeed * Time.deltaTime);
            float z = Mathf.Lerp(this.transform.localPosition.z, startPostion.z + DodgeZ, fallSpeed * Time.deltaTime);
            Vector3 localposition = this.transform.localPosition;
            this.transform.localPosition = new Vector3(x, localposition.y, z);

            Debug.Log("From: " + startPostion.z + " To: " + this.gameObject.transform.localPosition.z + DodgeZ);
            Debug.Log("Z ---------: " + z);
            Debug.Log("DodgeZ" + DodgeZ);
        }
        else
        {
           // this.spawnPoint.transform.position = new Vector3(this.spawnPoint.transform.position.x, newHeight, this.spawnPoint.transform.position.z);
        }

		if(this.fireDelay <= 0)
        {
           // float newHeight = Mathf.Lerp(this.gameObject.transform.position.y, this.startHeight * this.player.life, fallSpeed * Time.deltaTime);
           // Vector3 newPosition = Vector3.Lerp(this.transform.position, spawnPoint.transform.position, Time.deltaTime);
        }

        if(reg)
        {
            Vector3 newPosition = Vector3.Lerp(this.transform.position, spawnPoint.transform.position, 0.5f * Time.deltaTime);

            Quaternion newRoation = Quaternion.Lerp(this.transform.rotation, this.spawnPoint.transform.rotation,  Time.deltaTime);

            Debug.Log("Position: " + this.transform.position + " Cube Position " + spawnPoint.transform.position);

            if (!this.delayCorrection)
            {
                if (blockRotation)
                {
                    newRoation = Quaternion.Lerp(this.transform.rotation, this.spawnPoint.transform.rotation, 100 * Time.deltaTime);
                    this.transform.rotation = this.spawnPoint.transform.rotation;
                }
                else
                {
                    this.transform.rotation = newRoation;
                }

                this.transform.position = newPosition;
            }
        }
	}

    public static float RandomFloat(float minInclusive, float maxInclusive)
    {
        int randomInteger = Random.Range(0, 1024);
        // Convert to a value between 0f and 1f:
        float randomFloat = (float)randomInteger / (float)int.MaxValue;
        float range = maxInclusive - minInclusive;
        return minInclusive + randomFloat * range;
    }
}
