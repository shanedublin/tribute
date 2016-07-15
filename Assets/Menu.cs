using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {

	
    public void startLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
