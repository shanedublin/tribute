using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{



    public int deathCount = 0;
    public bool levelComplete;
    public int boogers = 0;
    public int totalBoogers = 0;

    public Text timerText;
    public Text deathText;
    public Text victoryText;
    public Text timer2Text;
    public Text booger2Text;
    public Text boogerText;
    public GameObject panel;
    public Text msg;
    float levelTime = 0;

    public PlayerController player;
    List<GameObject> disabledGameObjects = new List<GameObject>();

    public void PlayerDeath()
    {
        if(!levelComplete)        // if the level is complete dont add to the death
            deathCount++;
        
        deathText.text = deathCount + "";
        player.transform.position = Globals.instance.spawnPoint.position;
        player.Death();
        resetLevel();

    }

    public void addGameObject(GameObject go)
    {
        if (!disabledGameObjects.Contains(go))
        {
            disabledGameObjects.Add(go);
        }
    }

    private void resetLevel()
    {
        disabledGameObjects.ForEach(delegate (GameObject go)
        {

            go.SetActive(true);
        });
        boogers = 0;
        getBooger(0);
    }

    public void getBooger(int numBoogers)
    {
        boogers += numBoogers;
        boogerText.text = boogers + "/" + totalBoogers;

    }


    public void LevelVictory()
    {
        float savedTime = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "time");


        if (savedTime == 0 || savedTime > levelTime)
        {
            timer2Text.text = "Time: " + timerText.text + " New Record!";
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "time", levelTime);
        }
        else
            timer2Text.text = string.Format("Time: {0:F2} sec", savedTime);
        int savedBooger = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "booger");
        if(savedBooger < boogers)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "booger", boogers);
        }
        booger2Text.text = "Boogers: " + boogers + "/" + totalBoogers;

        levelComplete = true;
        timer2Text.gameObject.SetActive(true);
        victoryText.gameObject.SetActive(true);
        booger2Text.gameObject.SetActive(true);
        panel.SetActive(true);
        msg.gameObject.SetActive(true);

    }

    public void Update()
    {
        if (levelComplete)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Need to load menu screen");
                SceneManager.LoadScene("main");
            }
            return;
        }
        else
        {
        levelTime += Time.deltaTime;
        timerText.text = string.Format("{0:F2} sec",levelTime );

        }
    }

    
}
