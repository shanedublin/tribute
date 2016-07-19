using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    public enum Power{
        WallBoots, TeleBall, DoubleJump, Booger
    }
    public Power power;
    // Use this for initialization

    LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if(Power.Booger == power)
        {
            levelManager.totalBoogers++;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            switch (power)
            {
                case Power.WallBoots:
                    controller.wallBoots = true;
                    break;
                case Power.TeleBall:
                    controller.teleBall = true;
                    break;
                case Power.DoubleJump:
                    controller.doubleJumpBoots = true;
                    break;
                case Power.Booger:
                    levelManager.getBooger(1);
                    break;
                default:
                    break;
            }
            levelManager.addGameObject(gameObject);
            gameObject.SetActive(false);
        }

    }
}
