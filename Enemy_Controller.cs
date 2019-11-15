using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : Character_Controller
{
	[SerializeField]
	private int position = 0;

	public bool ciclic;

	private bool forward;

	public List <Transform> positions;
	// Use this for initialization
	protected override void  Start ()
	{
		base.Start();
        
		enemies = GameObject.FindGameObjectsWithTag("Agent");

		forward = true;

		SetRendererActive(false);
	}

	public override void Move()
	{
		animator.SetBool("Move_enemy", true);

		if (ciclic)
		{
			position ++;

			if (position >= positions.Count)
			{
				position = 0;
			}

			 
		}

		else 
		{
			if (forward)
			{
				position ++;

				if (position == positions.Count)
				{
					forward = false;
					position --;
				}
			}

			if (forward == false)
			{
				Debug.Log ("Volviendo");

				position --;

				if (position < 0)
				{
					forward = true;

					position += 2;
				}
			}
		}

		agent.SetDestination(positions[position].position);
	}

	public override void Combat()
	{
		animator.SetBool("Move_enemy", false);

		if (detected)
		{
			animator.SetTrigger("shoot_enemy");

			detected = false;
		}

		base.Combat();
	}
	public override void Life_Points(int damage)
	{
		base.Life_Points(damage);

		if (life_points <= 0)
		{
			//gameObject.GetComponent<Renderer>().enabled = false;

			animator.SetTrigger("morir_enemy");

			alive = false;
		}
	}

}
