using UnityEngine;
using System.Collections;

public class collidePlayer : MonoBehaviour 
{
	private Player player;
	private float startHeight;
	private Vector3 position;
	public float fallSpeed;

	public AudioClip[] hitSounds;

	public string dmgColliderTag;

	private Animator animator;
	private int hitHash = Animator.StringToHash("HitTrigger");
	GameController gameController;
	void OnCollisionEnter(Collision collisionInfo)
	{
		if(collisionInfo.collider.tag == dmgColliderTag)
		{
			Debug.Log("Enemy Collision 2");

			AudioSource source = GetComponent<AudioSource>();
			source.clip = hitSounds[Random.Range(0,hitSounds.Length-1)];
			source.Play();

			this.player.decrease(0.01f);

			this.animator.SetTrigger(hitHash);

			//Destroy (this.gameObject, 0.5f);

			/*this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, 
			                                                 this.gameObject.transform.position.y-10,
			                                                 this.gameObject.transform.position.z);*/
			
		}
	}

	// Use this for initialization
	void Start () 
	{
		gameController = GameObject.Find ("GameController").GetComponent<GameController>();
		animator = GetComponent<Animator>();
		this.player = new Player ();
		setStartHeight ();
	}

	public void setStartHeight(){
		this.startHeight = this.gameObject.transform.position.y;
		this.player.reset ();
	}
	
	// Update is called once per frame
	void Update () 
	{


		if (gameController.isInIntroState ())
			return;

		this.position = this.gameObject.transform.position;

		float newHeight = Mathf.Lerp (this.gameObject.transform.position.y, this.startHeight * this.player.life, fallSpeed * Time.deltaTime);
		this.gameObject.transform.position = new Vector3 (position.x, newHeight, position.z);
	}
}
