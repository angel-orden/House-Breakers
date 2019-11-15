using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{


    //Declaramos los estados por los que pasará cada turno, y hacemos que inicie en el primer estado de move.
	private enum States {Move,Rotate,Combat,End_Turn}
	States state = States.Move;

    //Creamos los arrays de jugadores y enemigos.
	[SerializeField]
	private Player_Controller [] players;

	private Enemy_Controller [] enemies;
    
    //Hacemos un texto para indicarnos la vida de los agentes vivos, a la par que los textos de victoria y derrota.
	[SerializeField]
	private Text agents_life_points;

	[SerializeField]
	private Text Game_over;

	[SerializeField]
	private Text Victory_Royale;
    
    //Creamos las variables de enemigos y jugadores que se encuentran vivos en escena.
	public int players_alive = 0;

	public int enemies_alive = 0;

	public int num_player = 0;

	[SerializeField]
	private Button resume;

	[SerializeField]
	private Button exit;

	[SerializeField]
	private Image panel;

	public bool Paused = false;

// Use this for initialization
	void Start ()
	{
        //Creamos el array agents para que recoja todos los objetos de la imagen con el tag de "Agent", y cogemos sus scripts para metérselos al array de players.
		GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");

		players = new Player_Controller [agents.Length];

		for (int i = 0; i < agents.Length; i++)
		{
			players [i] = agents[i].GetComponent<Player_Controller>();
		}

        //Creamos el array agents para que recoja todos los objetos de la imagen con el tag de "Enemy", y cogemos sus scripts para metérselos al array de enemies.
        GameObject[] terrorists = GameObject.FindGameObjectsWithTag("Enemy");

		enemies = new Enemy_Controller [terrorists.Length];

		for (int i = 0; i < terrorists.Length; i++)
		{
			enemies [i] = terrorists[i].GetComponent<Enemy_Controller>();
		}

        //Hacemos que desde el inicio se active el método de vida de los agentes, y seteamos los textos de derrota y victoria como falsos.
		Update_Life_Points();

        Game_over.gameObject.SetActive(false);

        Victory_Royale.gameObject.SetActive(false);

		StartCoroutine(Move());

		resume.gameObject.SetActive(false);

		exit.gameObject.SetActive(false);

		panel.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.GetButtonDown("Cancel"))
		{
			Time.timeScale = 0;

			resume.gameObject.SetActive(true);

			exit.gameObject.SetActive(true);

			panel.gameObject.SetActive(true);

			Paused = true;
		}
	}

	public void Continue ()
	{
		Time.timeScale = 1;

		resume.gameObject.SetActive(false);

		exit.gameObject.SetActive(false);

		panel.gameObject.SetActive(false);

		StartCoroutine ( Pause());
	}

	public void Exit()
	{
		SceneManager.LoadScene("menu_principal");
	}


	IEnumerator Pause()
	{
		yield return new WaitForSeconds (0.1f);
		Paused = false;
	}
	IEnumerator Move()
	{
		while(state == States.Move)
		{
			if(Input.GetMouseButtonUp(0) && Paused == false)
			{
					
				if (players[num_player].alive)
				{
					//Debug.Log("Moving player..." + num_player);
					players [num_player].Move();
				}
					
			
			
				if (num_player == players.Length - 1)
				{
					players_alive = 0;

					enemies_alive = 0;

					for (int j = 0; j < enemies.Length; j++)
					{
						if(enemies[j].alive)
						{
							enemies[j].Move();
						}
					}

					//Next_State();
					state++;
					yield return new WaitForSeconds(0.1f);
					StartCoroutine(Rotate());
				}
				else
				{
					Debug.Log("Moviendo siguiente jugador..." + num_player);
					num_player ++;
				}
			}
			yield return null;
		}

		//
	}

	IEnumerator Rotate()
	{
		num_player = 0;

		while(state == States.Rotate)
		{
			if (Paused == false)
			{
				if(Input.GetMouseButton(0))
				{
					if (players[num_player].alive)
					{
						Debug.Log("Rotating player..." + num_player);
						players[num_player].Rotate();
					}	
				}
					
				if (Input.GetMouseButtonUp(0))
				{
					Debug.Log("Cambiando de agente...");
					num_player ++;
					
					if (num_player == players.Length)
					{
						num_player = 0;

						state ++;
						yield return new WaitForSeconds(0.1f);
						StartCoroutine(Combat());
					}
				}	
			}
		
			yield return null;
		}
	}

	IEnumerator Combat()
	{
		while(state == States.Combat)
		{
			for (int i = 0; i < players.Length; i++)
			{
				if (players[i].alive)
				{
					players[i].Combat();
				}
				
			}

			for (int i = 0; i < enemies.Length; i++)
			{
				if (enemies[i].alive)
				{
					enemies[i].Combat();
				}
				
			}

			//Next_State();

			state ++;
			
			Update_Life_Points();
			yield return new WaitForSeconds(0.1f);
			StartCoroutine(End_Turn());
			yield return null;
		}
	}

	IEnumerator End_Turn()
	{
		while(state == States.End_Turn)
		{
			for (int i = 0; i < players.Length; i ++)
			{
				if (players[i].alive)
				{
					players_alive++;
				}
			}

			for (int i = 0; i < enemies.Length; i ++)
			{
				if (enemies[i].alive)
				{
					enemies_alive++;
				}
			}

			if (players_alive == 0)
			{
				Time.timeScale = 0;

				Game_over.gameObject.SetActive(true);

				Debug.Log("Has perdido");
			}
			
			if (enemies_alive == 0)
			{
				Time.timeScale = 0;

				Victory_Royale.gameObject.SetActive(true);

				Debug.Log("Has ganado");
			}

			else
			{
				state = States.Move;
				yield return new WaitForSeconds(0.1f);
				StartCoroutine(Move());

				num_player = 0;
			}

			yield return null;
		}
	}

    //Hacemos un método para cambiar de estado al siguiente.
	public void Next_State()
	{
		state ++;
		Debug.Log("Cambiando estado " + state);
	}

    //Creamos un método que comprobará cuántos agentes hay y cuánta vida poseen, plasmándolo en el texto de players_life_points.
	public void Update_Life_Points()
	{
		string life_points = "";

		for (int i = 0; i < players.Length; i++)
		{
			Debug.Log("Comprobando vida" + players.Length);
			life_points += "Agent " + i + " => " + players[i].life_points + "\n";
		}

		agents_life_points.text = life_points;

	}
	
}
