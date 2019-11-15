using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	//public Slider loadBar;
	//public Image fadeImage;

	public void Play()
	{
		//SceneManager.LoadScene("Game");
		Debug.Log("cambiando de escena");
		SceneManager.LoadScene("Game");
	}

	public void Exit()
	{
		SceneManager.LoadScene("menu_principal");
	}

	/*IEnumerator LoadYourAsyncScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		asyncLoad.allowSceneActivation = false;

        while (loadBar.value < 0.9f)
        {

			loadBar.value = asyncLoad.progress < loadBar.value + 0.02f ? asyncLoad.progress : loadBar.value + 0.02f;

			Debug.Log("Progress: " + loadBar.value);

            yield return null;
        }

		float fadeIntensity = 0;

		while(fadeIntensity < 1)
		{
			fadeIntensity += 0.02f;

			Color col = fadeImage.color;
			col.a = fadeIntensity;

			fadeImage.color = col;

			yield return null;
		}

		Color col = fadeImage.color;
		col.a = fadeIntensity;

		fadeImage.color = col;

		asyncLoad.allowSceneActivation = true;
    }*/
}
