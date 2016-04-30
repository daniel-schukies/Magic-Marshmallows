using UnityEngine;
using System.Collections;

public class checkCollide : MonoBehaviour 
{
	public GameObject gameController;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log ("Collide Boden!" + collisionInfo.gameObject.name + "---" + collisionInfo.gameObject.tag);

		if (collisionInfo.gameObject.tag == "Player") {
			GameController controller = this.gameController.GetComponent<GameController> ();

			controller.setLose ("Player");
			
		} 
		else if ((collisionInfo.collider.tag == "Enemy")) 
		{
			GameController controller = this.gameController.GetComponent<GameController> ();
			
			controller.setLose ("Enemy");
		}
	}
}
