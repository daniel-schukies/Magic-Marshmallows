/** 
 * 
 * Copyright © 2016 by Daniel Schukies 
 * 
 * **/


using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public float rotationSpeed;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0,rotationSpeed * Time.deltaTime,0));
	}
}
