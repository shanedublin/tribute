using UnityEngine;
using System.Collections;

public class Victory : MonoBehaviour {


    LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            levelManager.LevelVictory();

        }
    }
}
