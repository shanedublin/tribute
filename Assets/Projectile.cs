using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

    public Vector2 speed;
    public Vector2 acceleration;
    public Vector2 drag;
    public float groundDrag = .9f;
    public float airDrag;


    public Transform bottomLeftCorner;
    public Transform bottomRightCorner;
    public Transform topLeftCorner;

    public bool grounded;


    public int numVeriticalRayCasts = 2;
    public int numHorizontalRayCasts = 2;

    public LayerMask layerMask;


    public float moveForce;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame


    public void FixedUpdate()
    {

        EntityInput();
        HorizontalMovement();
        VerticalMovement();
    }

    protected abstract void EntityInput();

    protected void HorizontalMovement()
    {

        float distBetweenCorners = topLeftCorner.position.y - bottomLeftCorner.position.y;

        Vector2 direciton;
        bool checkRightWall;
        if (speed.x > 0)
        {
            checkRightWall = true;
            direciton = Vector2.right;
        }
        else
        {
            checkRightWall = false;
            direciton = Vector2.left;

        }

        for (int i = 0; i < numHorizontalRayCasts; i++)
        {
            float offset = 0;
            if (i == 0)
                offset = .01f;
            else if (i == numHorizontalRayCasts - 1)
                offset = -.01f;

            Vector3 rayCastPos;
            if (checkRightWall)
                rayCastPos = new Vector3(bottomRightCorner.position.x, bottomLeftCorner.position.y + offset + i * distBetweenCorners / (numHorizontalRayCasts - 1));
            else
                rayCastPos = new Vector3(bottomLeftCorner.position.x, bottomLeftCorner.position.y + offset + i * distBetweenCorners / (numHorizontalRayCasts - 1));

            RaycastHit2D hit = Physics2D.Raycast(rayCastPos, direciton, 100, layerMask.value);

            if (hit.collider != null)
            {
                float distance = Mathf.Abs(hit.point.x - rayCastPos.x);
                Debug.DrawRay(rayCastPos, direciton * distance, Color.green);
                if (distance <= .02f && Mathf.Abs(speed.x) > 0)
                {
                    snapToWall(hit.collider.gameObject, checkRightWall);
                    break;
                }
            }
        }

        float x = transform.position.x;
        // Debug.Log(Mathf.Abs(acceleration.x) > 0);
        if (Mathf.Abs(acceleration.x) > 0 || Mathf.Abs(speed.x) > .03f)
        {
            speed.x += acceleration.x * Time.fixedDeltaTime;
            if(grounded)
                speed.x *= groundDrag;
            x = x + speed.x * Time.fixedDeltaTime ;
            transform.position = new Vector2(x, transform.position.y);

        }
        else
        {
            speed.x = 0;
        }

    }

    public void VerticalMovement()
    {
        float distBetweenCorners = bottomRightCorner.position.x - bottomLeftCorner.position.x;

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
        for (int i = 0; i < numVeriticalRayCasts; i++)
        {

            float offset = 0;
            if (i == 0)
                offset = .01f;
            else if (i == numVeriticalRayCasts - 1)
                offset = -.01f;

            Vector3 rayCastPos;
            if (goingDown)
                rayCastPos = new Vector3(bottomLeftCorner.position.x + offset + i * distBetweenCorners / (numVeriticalRayCasts - 1), bottomLeftCorner.position.y);
            else
                rayCastPos = new Vector3(bottomLeftCorner.position.x + offset + i * distBetweenCorners / (numVeriticalRayCasts - 1), topLeftCorner.position.y);

            RaycastHit2D hit = Physics2D.Raycast(rayCastPos, direction, 1000, layerMask.value);
            if (hit.collider != null)
            {
                float distance = Mathf.Abs(hit.point.y - rayCastPos.y);

                Debug.DrawRay(rayCastPos, direction * distance, Color.blue);

                if (distance <= .1f && Mathf.Abs(speed.y) > 0) // if the player is really close snap to the ground
                {
                    snapToGround(hit.collider.gameObject, goingDown);
                    break;
                }


            }
        }

        if (speed.y > 0)
        {
            grounded = false;
        }





        transform.position = new Vector3(transform.position.x, transform.position.y + speed.y * Time.fixedDeltaTime);
        speed.y = speed.y + Globals.instance.gravity * Time.fixedDeltaTime;
    }


    public void snapToGround(GameObject ground, bool groundBellow)
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
            // Debug.Log("" + groundedY);

        }


        speed.y = 0;
        transform.position = new Vector3(transform.position.x, groundedY);
    }

    public void snapToWall(GameObject wall, bool wallOnRight)
    {
        float wallXpos = wall.transform.position.x;
        float playerXOffset = gameObject.GetComponent<BoxCollider2D>().size.x / 2 * gameObject.transform.localScale.x;
        float wallXOffset = wall.GetComponent<BoxCollider2D>().size.x / 2 * wall.transform.localScale.x;


        float wallX;
        if (wallOnRight)
            wallX = wallXpos - playerXOffset - wallXOffset; // wall on right
        else
            wallX = wallXpos + playerXOffset + wallXOffset; // wall on left

        speed.x = 0;
        acceleration.x = 0;
        transform.position = new Vector3(wallX, transform.position.y);

    }
}
