using UnityEngine;
using System.Collections;

public class PlayerController : Entity
{

    // public Rigidbody2D body;

    public float floatHeight;
    public float liftForce;
    public float damping;

    public bool needToJump;
    public bool jumped;
    public bool grounded;

    public Rigidbody2D body;

    float horizontalInput;
    public LayerMask layerMask;
    public float jumpForce = 5f;

    public float moveForce;

    public Transform bottomLeftCorner;
    public Transform bottomRightCorner;
    public Transform topLeftCorner;

    public int numVeriticalRayCasts = 2;
    public int numHorizontalRayCasts = 2;


    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        // set some deafault values;
        if (numVeriticalRayCasts < 2)
            numVeriticalRayCasts = 2;
        if (numHorizontalRayCasts < 2)
            numHorizontalRayCasts = 2;
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        

        if (Input.GetButtonDown("Jump") && grounded)
        {
            needToJump = true;
        }
    }

    public void FixedUpdate()
    {

        VerticalMovement();
        HorizontalMovement();


    }

    private void HorizontalMovement()
    {
        acceleration.x = horizontalInput;

        float distBetweenCorners = topLeftCorner.position.y - bottomLeftCorner.position.y;

        Vector2 direciton;
        bool checkRightWall;
        if (acceleration.x > 0)
        {
            checkRightWall = true;
            direciton = Vector2.right;
        }
        else 
        {
            checkRightWall = false;
            direciton = Vector2.left;

        }

        for(int i  = 0; i < numHorizontalRayCasts; i++)
        {
            float offset = 0;
            if (i == 0)
                offset = .01f;
            else if (i == numHorizontalRayCasts - 1)
                offset = -.01f;

            Vector3 rayCastPos;
            if(checkRightWall) 
                rayCastPos = new Vector3(bottomRightCorner.position.x, bottomLeftCorner.position.y + offset +  i * distBetweenCorners / (numHorizontalRayCasts - 1));
            else
                rayCastPos = new Vector3(bottomLeftCorner.position.x, bottomLeftCorner.position.y + offset + i * distBetweenCorners / (numHorizontalRayCasts - 1));

            RaycastHit2D hit = Physics2D.Raycast(rayCastPos, direciton, 100, layerMask.value );

            if(hit.collider != null)
            {
                float distance = Mathf.Abs(hit.point.x - rayCastPos.x);
                Debug.DrawRay(rayCastPos, direciton * distance, Color.green);
                if(distance <= .1f && Mathf.Abs(speed.x) > 0)
                {
                    snapToWall(hit.collider.gameObject, checkRightWall);
                    break;
                }
            }
        }

        float x = body.position.x;
       // Debug.Log(Mathf.Abs(acceleration.x) > 0);
        if (Mathf.Abs(acceleration.x) > 0 || Mathf.Abs(speed.x) > .1f)
        {
            speed.x += acceleration.x * Time.fixedDeltaTime;
            speed.x *= groundDrag;                
            x = x + speed.x * Time.fixedDeltaTime * moveForce;
            body.position = new Vector2(x, body.position.y);
            
        }
        else
        {
            speed.x = 0;
        }




    }

    private void VerticalMovement()
    {

        float distBetweenCorners =   bottomRightCorner.position.x - bottomLeftCorner.position.x;

        Vector2 direction;
        bool goingDown;

        if (speed.y > 0)
        {
            goingDown = false;
            direction = Vector2.up;
        }
        else
        {
            goingDown = true;
            direction = Vector2.down;
        }

        // raycast a bunch...
        for(int i = 0; i < numVeriticalRayCasts; i ++)
        {

            float offset = 0;
            if (i == 0)
                offset = .01f;
            else if (i == numVeriticalRayCasts - 1)
                offset = -.01f;

            Vector3 rayCastPos;
            if(goingDown)
                rayCastPos = new Vector3(bottomLeftCorner.position.x + offset + i * distBetweenCorners / (numVeriticalRayCasts -1),  bottomLeftCorner.position.y);
            else
                rayCastPos = new Vector3(bottomLeftCorner.position.x + offset + i * distBetweenCorners / (numVeriticalRayCasts - 1), topLeftCorner.position.y);

            RaycastHit2D hit = Physics2D.Raycast(rayCastPos , direction, 1000, layerMask.value);
            if (hit.collider != null)
            {
                float distance = Mathf.Abs(hit.point.y - rayCastPos.y);
                
                Debug.DrawRay(rayCastPos, direction * distance, Color.blue);

                if (distance <= .1f && Mathf.Abs(speed.y) > 0) // if the player is really close snap to the ground
                {
                    snapToGround( hit.collider.gameObject, goingDown);
                    break;
                }


            }
        }
        

        if (needToJump && grounded)
        {
            speed.y = jumpForce;
            needToJump = false;
        }
        

        if(speed.y > 0)
        {
            grounded = false;
        }

        speed.y = speed.y + Globals.instance.gravity * Time.fixedDeltaTime;

        transform.position = new Vector3(transform.position.x, transform.position.y + speed.y * Time.fixedDeltaTime);
      
        
    }

    public void snapToGround( GameObject ground, bool groundBellow)
    {

        float groundYpos = ground.transform.position.y;
        float playerYOffset = gameObject.GetComponent<BoxCollider2D>().size.y / 2 * gameObject.transform.localScale.y;
        float groundYOffset = ground.GetComponent<BoxCollider2D>().size.y / 2 * ground.transform.localScale.y;

        float groundedY;
        if (groundBellow)
        {
            groundedY = groundYpos + playerYOffset + groundYOffset;
            grounded = true;
        }
        else
        {
            groundedY = groundYpos - playerYOffset - groundYOffset;
            Debug.Log("" + groundedY);

        }


        speed.y = 0;
        transform.position = new Vector3(transform.position.x, groundedY);
    }
    
    public void snapToWall(GameObject wall, bool wallOnRight)
    {
        float wallXpos = wall.transform.position.x;
        float playerXOffset = gameObject.GetComponent<BoxCollider2D>().size.x / 2 * gameObject.transform.localScale.x;
        float wallXOffset = wall.GetComponent<BoxCollider2D>().size.x / 2 * wall.transform.localScale.x;

        
        float wallX ;
        if (wallOnRight)
            wallX = wallXpos - playerXOffset - wallXOffset; // wall on right
        else        
            wallX = wallXpos + playerXOffset + wallXOffset; // wall on left

        speed.x = 0;
        acceleration.x = 0;
        transform.position = new Vector3(wallX, transform.position.y);

    }

}
