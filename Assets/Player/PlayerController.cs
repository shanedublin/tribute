using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public class PlayerController : Entity
{

    public bool wallBoots = false;
    public bool teleBall = false;
    public bool doubleJumpBoots = false;
    public float floatHeight;
    public float liftForce;
    public ParticleSystem ps;

    internal void Death()
    {
        wallBoots = false;
        teleBall = false;
        doubleJumpBoots = false;
    }

    public float damping;

    public bool needToJump;
    public bool doubleJump;

    public Grapple grapple;


    float horizontalInput;

    public float jumpForce = 5f;



    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump"))
        {
           // Debug.Log("Haters gonna hate");
            needToJump = true;
        }
        Grapple();
        SwapWithGrapple();
    }

    private void SwapWithGrapple()
    {
        if (!teleBall)  // if you dont have the ball upgrade return
            return;
        if (Input.GetMouseButtonDown(1))
        {

            Vector3 grapplePos = grapple.transform.position;
            grapple.transform.position = transform.position;
            transform.position = grapplePos;
        }

    }

    public void Grapple()
    {
        if (!teleBall) // if you dont have the ball upgrade return
            return;

        if (Input.GetMouseButtonDown(0))
        {

            grapple.target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = grapple.target - transform.position;
            grapple.target = transform.position;
            grapple.gameObject.SetActive(true);
            grapple.shoot = true;
            direction = direction.normalized;
            //Debug.Log(direction);
            grapple.speed = direction * grapple.moveForce + speed / 3;
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
        
        if (grounded)
            doubleJump = true;

        if (needToJump)
        {
            Jump();            
            needToJump = false;
        }



    }
    protected void Jump()
    {
        if (grounded)
        {
            speed.y = jumpForce;
            touchingLeftWall = false;
            touchingRightWall = false;
            ps.Emit(10);
            ps.transform.localEulerAngles = new Vector3(90, 90, 90);
            return;
        }
        if (wallBoots)
        {
            if (touchingLeftWall)
            {
                speed.x = jumpForce;
                speed.y = jumpForce / 1.9f;

                touchingLeftWall = false;
                ps.Emit(10);
                ps.transform.localEulerAngles = new Vector3(180, 90, 90);
                return;
            }
            else if (touchingRightWall)
            {
                speed.x = -jumpForce;
                speed.y = jumpForce / 1.9f;
                touchingRightWall = false;
                ps.Emit(10);
                ps.transform.localEulerAngles = new Vector3(0, 90, 90);
                return;

            }
        }
        if (doubleJumpBoots)
        {
            if (doubleJump)
            {
                speed.y = jumpForce / 1.5f;
                doubleJump = false;
                ps.Emit(20);
                ps.transform.localEulerAngles = new Vector3(90, 90, 90);
                return;
            }
        }

    }


}
