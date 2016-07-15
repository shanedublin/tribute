using UnityEngine;
using System.Collections;
using System;

public class PlayerController : Entity
{

    // public Rigidbody2D body;

    

    public bool wallBoots = true;
    public bool teleBall = true;
    public bool doubleJumpBoots = true;
    


    public float floatHeight;
    public float liftForce;
    public float damping;

    public bool needToJump;
    public bool doubleJump;
    public bool jumped;

    public Grapple grapple;


    float horizontalInput;

    public float jumpForce = 5f;

    float jumpRoot = Mathf.Sqrt(2);

    public void Start()
    {
        jumpRoot = Mathf.Sqrt(jumpForce / 2);

    }


    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Haters gonna hate");
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
            if (grounded)
            {                
                speed.y = jumpForce;
                touchingLeftWall = false;
                touchingRightWall = false;
            }
            else if (wallBoots)
            {            
                if (touchingLeftWall)
                {
                    speed.x = jumpForce  ;
                    speed.y = jumpForce / 1.9f ;
                    touchingLeftWall = false;
                }
                else if (touchingRightWall)
                {
                    speed.x = -jumpForce;
                    speed.y = jumpForce / 1.9f ;
                    touchingRightWall = false;

                }
            }else if (doubleJumpBoots)
            {
                if (doubleJump)
                {
                    speed.y = jumpForce / 1.5f;
                    doubleJump = false;
                }
            }
            needToJump = false;
        }



    }


}
