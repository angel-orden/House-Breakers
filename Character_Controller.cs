using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character_Controller : MonoBehaviour
{
	public Camera cam;

	public NavMeshAgent agent;

	protected enum player_states {idle,moving};

	protected player_states state = player_states.idle;

	protected GameObject [] enemies;

	protected Game_Manager game_manager;


	public Light luz;

	public int life_points;

	public bool alive = true;

	public float vision;
	
	private SpriteRenderer cuerpo;

	private SpriteRenderer pies;
	
	protected Animator animator;

	public int damage_given;

	protected bool detected;


	protected virtual void Start()
	{
        //Asignamos el game manager y el array de enemigos, a la vez que desactivamos la flecha.
		game_manager = GameObject.Find("Game_Manager").GetComponent<Game_Manager>();

		//enemies = GameObject.FindGameObjectsWithTag("Enemy");

		cuerpo = transform.GetChild(0).GetComponent<SpriteRenderer>();

		pies = cuerpo.transform.GetChild(0).GetComponent<SpriteRenderer>();

		animator = GetComponent<Animator>();
	}

    //Creamos el movimiento, el cual se creará clicando en un punto del NavMesh Surface del mapa.
	public virtual void Move()
	{

	}

    //Creamos el método de rotación.
	public virtual void Rotate()
	{
		
	}

    //Creamos el método de combate, en el cual comprobamos si nuestro personaje ve al enemigo, en cuyo caso procederá a dispararle, quitándole dos puntos de vida.
	public virtual void Combat()
	{

		RaycastHit hit;

		for (int i = 0; i < enemies.Length; i++)
		{
			GameObject other = enemies [i];

			Vector3 targetDir = other.transform.position - transform.position;

        	float angle = Vector3.Angle(targetDir, transform.forward);

			//Debug.Log(angle);

			if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 1000) && angle < vision)
			{

				if(hit.transform.gameObject == other)
				{
					other.GetComponent<Character_Controller>().Life_Points(damage_given);
					Debug.Log("Enemy spoted, shooting...");
					detected = true;
				}
			}
		}
		
	}

    //Creamos el método de daño, en el cual según el daño que le pasemos, se le restará a la vida del personaje.
	public virtual void Life_Points (int damage)
	{
		life_points -= damage;
	}

	public void SetRendererActive(bool active)
	{
		cuerpo.enabled = active;

		pies.enabled = active;
	}

   
}
