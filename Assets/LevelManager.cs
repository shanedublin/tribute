using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{



    public int deathCount = 0;
    public bool levelComplete;
    public int boogers = 0;
    public int totalBoogers = 0;
    public Transform spawnPoint;
    public Text timerText;
    public Text deathText;
    public Text victoryText;
    public Text timer2Text;
    public Text booger2Text;
    public Text boogerText;
    public GameObject panel;
    public Text msg;
    float levelTime = 0;

    public string levelName;

    public PlayerController player;
    List<GameObject> disabledGameObjects = new List<GameObject>();

    public void Awake()
    {
        if(levelName == null || levelName.Length < 1)
        {
            throw new Exception("Level Name Not Set");
        }
        player.transform.position = spawnPoint.position;
    }

    public void PlayerDeath()
    {
        if (!levelComplete)        // if the level is complete dont add to the death
            deathCount++;

        InfoBlurbManager.instance.CreateInfoBlurb(player.transform.position, "Dead!", Color.red);
        deathText.text = deathCount + "";
        player.transform.position = spawnPoint.position;
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
        resetBoogers();
    }

    public void getBooger(int numBoogers)
    {
        boogers += numBoogers;
        boogerText.text = boogers + "/" + totalBoogers;
        InfoBlurbManager.instance.CreateInfoBlurb(player.transform.position, "+" + numBoogers, Color.white);

    }
    public void resetBoogers()
    {
        boogers = 0;
        boogerText.text = boogers + "/" + totalBoogers;

    }


    public void LevelVictory()
    {
        float savedTime = PlayerPrefs.GetFloat(levelName + "time");


        if (savedTime == 0 || savedTime > levelTime)
        {
            timer2Text.text = "Time: " + timerText.text + " New Record!";
            PlayerPrefs.SetFloat(levelName + "time", levelTime);
        }
        else
        {
            timer2Text.text = string.Format("Best Time: {0:F2} sec", savedTime);
        }
        int savedBooger = PlayerPrefs.GetInt(levelName + "booger");
        if (savedBooger > boogers)
        {
            PlayerPrefs.SetInt(levelName + "booger", boogers);
            booger2Text.text = "Most Boogers: " + savedBooger + "/" + totalBoogers;
        }
        else
        {
            booger2Text.text = "Boogers: " + boogers + "/" + totalBoogers + " New Record! ";

        }

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
                
                SceneManager.LoadScene(0);
            }
            return;
        }
        else
        {
            levelTime += Time.deltaTime;
            timerText.text = string.Format("{0:F2} sec", levelTime);

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            //SceneManager.LoadScene(0);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }


}
