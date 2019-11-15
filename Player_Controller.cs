using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Controller : Character_Controller
{
    //Creamos la cámara, el agente que se moverá, los estados del turno, el array de enemigos, la flecha de dirección, la luz, los puntos de vida, el bool de alive y referenciamos el game_manager.
	/*public Camera cam;

	public NavMeshAgent agent;

	private enum player_states {idle,moving};

	player_states state = player_states.idle;

	private GameObject [] enemies;

	private Game_Manager game_manager;



	public Light luz;

	public int life_points;

	public bool alive = true;*/
	
	public GameObject arrow;


	protected override void  Start()
	{
        //Asignamos el game manager y el array de enemigos, a la vez que desactivamos la flecha.
		base.Start();

		arrow.SetActive(false);

		enemies = GameObject.FindGameObjectsWithTag("Enemy");

	}
	
    //Este método lo utilizaremos para que nuestro personaje se mueva por el mapa, pasando al estado de idle una vez termina el movimiento, para eso nos saltamos 0.1 segundos del desplazamiento, evitando así cualquier fallo.
	private IEnumerator State_Move()
	{
		animator.SetBool("caminar_player",true);

		while (state == player_states.moving)
		{
			yield return new WaitForSeconds(0.1f);
		
				foreach (Enemy_Controller enemy in Enemy_detection())
				{
					//enemy.gameObject.GetComponent<Renderer>().enabled = true;

					enemy.SetRendererActive(true);
				}

			//Debug.Log("Moving...");
				if (agent.remainingDistance < 0.1f)
				{
					state = player_states.idle;

					//game_manager.Next_State();
					
					arrow.SetActive(true);
				}			
		}
		
		animator.SetBool("caminar_player", false);
	}

    //Creamos el movimiento, el cual se creará clicando en un punto del NavMesh Surface del mapa.
	public override void Move()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				//Debug.Log("Ray hit: " + hit.transform.gameObject.name);
				agent.SetDestination(hit.point);

				state = player_states.moving;

				StartCoroutine(State_Move());
			}



			
	}

    //Creamos el método de rotación.
	public override void Rotate()
	{
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				//Debug.Log("Ray hit: " + hit.transform.gameObject.name);
				transform.LookAt(hit.point, Vector3.up);
				transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			}

	}

    //Creamos el método de combate, en el cual comprobamos si nuestro personaje ve al enemigo, en cuyo caso procederá a dispararle, quitándole dos puntos de vida.
	public override void Combat()
	{
		arrow.SetActive(false);

		if (detected)
		{
			animator.SetTrigger("disparar_player");

			detected = false;
		}

		base.Combat();
	}

    //Creamos el método de daño, en el cual según el daño que le pasemos, se le restará a la vida del personaje.
	

    //Creamos una lista para detectar a cuantos enemigos vemos.
	public List<Enemy_Controller> Enemy_detection()
	{
		List<Enemy_Controller> enemies_spotted = new List<Enemy_Controller>();

		RaycastHit hit;

		for (int i = 0; i < enemies.Length; i++)
		{
			GameObject other = enemies [i];

			Vector3 targetDir = other.transform.position - transform.position;

        	float angle = Vector3.Angle(targetDir, transform.forward);

			//Debug.Log(angle);

			if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit, 1000) && angle < luz.spotAngle && other.GetComponent<Enemy_Controller>().alive == true)
			{

				if(hit.transform.gameObject == other)
				{
					/*other.GetComponent<Enemy_Controller>().Life_Points(2);
					Debug.Log("Enemy spoted, shooting...");*/
					enemies_spotted.Add(other.GetComponent<Enemy_Controller>());

					detected = true;
				}
			}
		}

		return enemies_spotted;
	}

	public override void Life_Points(int damage)
	{
		base.Life_Points(damage);

		if (life_points <= 0)
		{
			//gameObject.GetComponent<Renderer>().enabled = false;

			alive = false;

			animator.SetTrigger("morir_player");
		}
	}

}
