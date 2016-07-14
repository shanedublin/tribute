using UnityEngine;
using System.Collections;
using System;

public class PlayerController : Entity
{

    // public Rigidbody2D body;

    public float floatHeight;
    public float liftForce;
    public float damping;

    public bool needToJump;
    public bool jumped;

    public Grapple grapple;


    float horizontalInput;

    public float jumpForce = 5f;




    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump") && grounded)
        {
            Debug.Log("Haters gonna hate");
            needToJump = true;
        }
        Grapple();
    }

  

    public void Grapple()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grapple.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = grapple.target - transform.position;
            grapple.target = transform.position;
            grapple.enabled = true;
            grapple.shoot = true;
            direction = direction.normalized;
            //Debug.Log(direction);
            grapple.speed = direction * grapple.moveForce + speed /3;
            grapple.acceleration = Vector2.zero;
        }
        else
        {
            //grapple.enabled = false;
        }
    }

    protected override void EntityInput()
    {
        acceleration.x = horizontalInput * moveForce;


        if (needToJump && grounded)
        {
            speed.y = jumpForce;
            needToJump = false;
        }


       

    }
}
