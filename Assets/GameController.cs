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
    private bool ready;

    private Vector3 startPosition;



	private Vector3 pPosition;
	private Quaternion pRotation;

	// Use this for initialization
	void Start () 
	{
        this.ready = false;
		this.pPosition = this.pivot.transform.position;
		this.pRotation = this.pivot.transform.rotation;
		pivot.GetComponent<rotate> ().enabled = false;
		this.Enemy.SetActive (false);
		this.youLose.SetActive (false);

        for (int i = 0; i < this.GetComponents<AudioSource>().Length; i++)
        {
            this.GetComponents<AudioSource>()[i].Stop();
        }


        this.GetComponents<AudioSource>()[Random.Range(0, this.GetComponents<AudioSource>().Length)].Play();
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


        this.Enemy.GetComponent<collidePlayer>().spawnPoint.transform.position = this.Enemy.transform.position;
        this.Player.GetComponent<collidePlayer>().spawnPoint.transform.position = this.Player.transform.position;



		this.inIntroState = true;
        this.ready = false;

        int PlayIndex = 0;

        for (int i = 0; i < this.GetComponents<AudioSource>().Length; i++)
        {
            if(this.GetComponents<AudioSource>()[i].isPlaying)
            {
                PlayIndex = i;
            }
            this.GetComponents<AudioSource>()[i].Stop();
        }




        int NextPlay = Random.Range(0, this.GetComponents<AudioSource>().Length);
        if (this.GetComponents<AudioSource>().Length > 1)
        {
            while ( NextPlay != PlayIndex)
            {
                NextPlay = Random.Range(0, this.GetComponents<AudioSource>().Length);
            }

            this.GetComponents<AudioSource>()[NextPlay].Play();
        }

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
        this.ready = true;

        this.startPosition = this.pivot.transform.position;

        this.Enemy.SetActive(true);
        this.Enemy.GetComponent<collidePlayer>().setStartHeight();
        this.Player.GetComponent<collidePlayer>().setStartHeight();

        this.Intro.SetActive(false);
        pivot.GetComponent<rotate>().enabled = true;
        inIntroState = false;

        this.youLose.SetActive(false);
        this.youWin.SetActive(false);

	}

	// Update is called once per frame
	void Update () 
	{
        if (inIntroState)
            return;
        /*
        if (ready && inIntroState)
        {
            this.pivot.transform.position = Vector3.Lerp(this.pivot.transform.position, startPosition + new Vector3(0, 5, 0), 10 * Time.deltaTime);

            if (this.pivot.transform.position.y >= (startPosition + new Vector3(0, 5, 0)).y)
            {

                this.Enemy.SetActive(true);
                this.Enemy.GetComponent<collidePlayer>().setStartHeight();
                this.Player.GetComponent<collidePlayer>().setStartHeight();

                this.Intro.SetActive(false);
                pivot.GetComponent<rotate>().enabled = true;
                inIntroState = false;

                this.youLose.SetActive(false);
                this.youWin.SetActive(false);
            }
        }
            */


        if (this.Player.transform.position.y < -11.14)
		{
			this.setLose("Player");
		}

        if (this.Enemy.transform.position.y < -11.14)
		{
			this.setLose("Enemy");
		}
	}
}
