using UnityEngine;
using System.Collections;

public class timeDistanceScale : MonoBehaviour {

	private bool isActive;
	private Vector3 startPosition;

	private float startScale;
	private float startTime;


	public float maxScale;
	public float scaleTime;

	// Use this for initialization
	void Start () 
	{
		this.isActive = false;
		this.startTime = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.isActive && startTime < scaleTime) 
		{
			startTime += Time.deltaTime;
			float scale = Mathf.Lerp(startScale, maxScale,  startTime / scaleTime );
			this.transform.localScale = new Vector3(scale, scale, scale);
			//Debug.Log(scale);
		}
	}

	public void setActive()
	{
		this.startPosition = this.transform.position;
		this.startScale = this.transform.localScale.x;
		this.isActive = true;
	}
}
