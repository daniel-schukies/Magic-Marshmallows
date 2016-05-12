/** 
 * 
 * Copyright © 2016 by Daniel Schukies 
 * 
 * **/



using UnityEngine;
using System.Collections;

public class AIEnemyShoot : MonoBehaviour 
{
	public GameObject player;
	// Use this for initialization
	public GameObject[] Elements;
	public GameObject leftHandFinger;
	public int force;

	private GameObject currentElement;
	private int elementIndex =0;
	private bool switchElement;
	   
	private float fireDelay;

	public float startFireDelay = 3;

	private float elementSwitchDelay;

	public float attackCollideMinDistance;

	private Animator animator;
	private int attackHash = Animator.StringToHash("attackTrigger");
	

	// Use this for initialization
	void Start () 
	{
		animator = this.GetComponent<Animator> ();

		this.switchElement = true;
		this.elementSwitchDelay = 1f;
		
		fireDelay = this.startFireDelay;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.fireDelay = fireDelay - Time.deltaTime;
		this.elementSwitchDelay = this.elementSwitchDelay - Time.deltaTime;

		if (this.currentElement != null) 
		{
			this.currentElement.transform.position = leftHandFinger.transform.position;
		}

		if (this.elementSwitchDelay <= 0f && this.switchElement) 
		{
			if (this.currentElement == null) 
			{
				elementIndex = elementIndex == 0 ? 1 : 0;
				this.spawnElement (elementIndex);
			} 
			else 
			{
				Destroy (this.currentElement);
				elementIndex = elementIndex == 0 ? 1 : 0;
				this.spawnElement (elementIndex);
			}

			this.switchElement = false;
			//this.elementSwitchDelay = 1f;
		}

		Vector3 shootDirection = this.player.transform.position - this.leftHandFinger.transform.position;
		Debug.DrawRay (this.leftHandFinger.transform.position, shootDirection);

		if (this.fireDelay <= 0f && this.currentElement != null) 
		{
			this.animator.SetTrigger(this.attackHash);
		}
	}

	private void shootElement(Vector3 shootDirection)
	{
		GameObject ballBig = currentElement;
		
		
		currentElement = null;
		
		ballBig.GetComponent<timeDistanceScale>().setActive();
				
		// Add a rigidbody in order to add forces to it later and get a reference to it
		//Rigidbody rigid = theBall.AddComponent<Rigidbody>();	
		Rigidbody rigid = ballBig.GetComponent<Rigidbody>();
		
		// convert the local direction "forward" of this gameobject to world space
		Vector3 worldDirection = shootDirection;
		
		// add force in world space
		rigid.AddForce(worldDirection.normalized * force);
		
		fireDelay = this.startFireDelay;
		ballBig.GetComponent<collide> ().minDistance = this.attackCollideMinDistance;
		ballBig.GetComponent<collide> ().setActive ();
	}



	private void spawnElement(int elementIndex)
	{
		this.currentElement = (GameObject)Instantiate (this.Elements[elementIndex], leftHandFinger.transform.position, Quaternion.identity);

		this.currentElement.tag = "EnemyFire";
		this.currentElement.transform.localScale = this.currentElement.transform.localScale * 0.1f;
		this.currentElement.GetComponent<SphereCollider>().enabled = false;
	}

	private void AnimationEventShoot()
	{
		Vector3 shootDirection = this.player.transform.position - this.leftHandFinger.transform.position;
		this.shootElement(shootDirection);
		this.switchElement = true;
	}
}
