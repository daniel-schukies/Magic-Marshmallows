using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{

	public GameObject Enemy;
	public GameObject Intro;
	public GameObject pivot;
	public GameObject Player;
	public GameObject youLose;
    public GameObject youWin;
	private bool inIntroState = true;


	private Vector3 pPosition;
	private Quaternion pRotation;

	// Use this for initialization
	void Start () 
	{
		this.pPosition = this.pivot.transform.position;
		this.pRotation = this.pivot.transform.rotation;
		pivot.GetComponent<rotate> ().enabled = false;
		this.Enemy.SetActive (false);
		this.youLose.SetActive (false);
	}

	public bool isInIntroState(){
		return inIntroState;
	}


	public void setLose()
	{
		this.pivot.transform.position = this.pPosition;
		this.pivot.transform.rotation = this.pRotation;

		this.Enemy.SetActive (false);
		pivot.GetComponent<rotate> ().enabled = false;

		this.Intro.SetActive (true);

		this.Enemy.transform.position = new Vector3 (this.Enemy.transform.position.x, this.pivot.transform.position.y, this.Enemy.transform.position.z);
		this.Player.transform.position = new Vector3 (this.Player.transform.position.x, this.pivot.transform.position.y, this.Player.transform.position.z);



		this.inIntroState = true;
	}


	public void setLose(string tag)
	{
		if (tag == "Player") 
		{
			this.setLose();

			this.youLose.SetActive (true);
            this.youWin.SetActive(false);
		} 
		else 
		{
            this.youLose.SetActive(false);
            this.youWin.SetActive(true);

			this.setLose();


		}
	}


	public void startGame(){

		this.pivot.transform.position = this.pivot.transform.position + new Vector3 (0,10,0);
		this.Enemy.SetActive (true);
		this.Enemy.GetComponent<collidePlayer> ().setStartHeight ();
		this.Player.GetComponent<collidePlayer> ().setStartHeight ();

		this.Intro.SetActive (false);
		pivot.GetComponent<rotate> ().enabled = true;
		inIntroState = false;

		this.youLose.SetActive (false);
	}

	// Update is called once per frame
	void Update () 
	{
		if (this.inIntroState)
			return;

		if(this.Player.transform.position.y < this.pPosition.y)
		{
			this.setLose("Player");
		}

		if(this.Enemy.transform.position.y < this.pPosition.y)
		{
			this.setLose("Enemy");
		}
	}
}
