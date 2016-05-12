/** 
 * 
 * Copyright © 2016 by Daniel Schukies 
 * 
 * **/



using UnityEngine;
using System.Collections;

public class Player 
{
	private float _life;
    private float startLife;

	private bool isAlive; 

	public Player()
	{
        
		this.life = 1.0f;
        this.startLife = life;
	}

    public Player(float life)
    {
        this.startLife = life;
        this.life = life;
    }

	public float life
	{
		get { return _life; }
		set { _life = value; }
	}

	public void reset()
	{
		this.life = this.startLife;
	}

	public void increase(float addLife)
	{
		this.life += addLife; 

		if (this.life > 1f) {
			this.life = 1f;
		} else if (this.life > 0f) {
			this.isAlive = true;
		}
	}

	public void decrease(float removeLife)
	{
		this.life -= removeLife;

		if (this.life <= 0) {
			this.isAlive = false;
		}
	}




}
