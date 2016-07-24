using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Hazard : MonoBehaviour
{

    public enum MovementType
    {
        Sin, Linear
    }
    public MovementType movementType;
    Vector3 originalPosition;
    public float xMoveSpeed = 0;
    public float yMoveSpeed = 0;
    public float xMove = 0;
    public float yMove = 0;

    public bool xDir = false;
    public bool yDir = false;

    LevelManager levelManager;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;
        levelManager = FindObjectOfType<LevelManager>();
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            levelManager.PlayerDeath();

        }
    }

    public void FixedUpdate()
    {
        Vector3 newPos;
        if(movementType == MovementType.Sin)
        {
            newPos = new Vector3(originalPosition.x , originalPosition.y );
            newPos.x += Mathf.Cos(Time.time * xMoveSpeed) * xMove;
            newPos.y += Mathf.Sin(Time.time * yMoveSpeed) * yMove;
        }else if(movementType == MovementType.Linear)
        {
            newPos = transform.position;

            if (xDir)
            {
                newPos.x += Time.deltaTime * xMoveSpeed;
            }
            else
            {
                newPos.x -= Time.deltaTime * xMoveSpeed;
            }

            if(transform.position.x > originalPosition.x + xMove)
            {
                xDir = false;
            }else if (transform.position.x < originalPosition.x - xMove)
            {
                xDir = true;
            }


            if (yDir)
            {
                newPos.y += Time.deltaTime * yMoveSpeed;
            }
            else
            {
                newPos.y -= Time.deltaTime * yMoveSpeed;
            }

            if (transform.position.y > originalPosition.y + yMove )
            {
                yDir = false;
            }else if (transform.position.y < originalPosition.y - yMove)
            {
                yDir = true;
            }
        }
        else
        {
            newPos = Vector3.zero;
            Debug.LogError("You didnt set the movement type");
        }

        transform.position = newPos;
    }
}
