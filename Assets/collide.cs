using UnityEngine;
using System.Collections;

public class collide : MonoBehaviour 
{
	private Vector3 startPosition;
	public float minDistance;
	private bool active = false;

	void OnCollisionEnter(Collision collisionInfo)
	{
		Debug.Log("Enemy Collision");
		Destroy (this.gameObject, 0.5f);
	}
	

	// Use this for initialization
	void Start () 
	{
		this.GetComponent<SphereCollider> ().enabled = false;
		this.startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (! active)
			return;
		float distance = Mathf.Abs((this.startPosition - this.transform.position).magnitude);

		if (distance >= minDistance) 
		{
			this.GetComponent<SphereCollider> ().enabled = true;
			//Debug.Log ("Distance: " + distance);

		}


	}

	public void setActive(){
		active = true;

	}
}
