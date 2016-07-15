using UnityEngine;
using System.Collections;

namespace Raycasting
{


    public static class Extensions
    {
        public static Vector2 getDirection(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Vector2.up;
                case Direction.Right: return Vector2.right;
                case Direction.Down: return Vector2.down;
                case Direction.Left: return Vector2.left;
                default: return Vector2.zero;

            }
        }
    }

    public enum Direction
    {
        Up, Right, Down, Left
    }


    public class Raycaster : MonoBehaviour
    {

        public Vector2 speed;
        public Vector2 acceleration;
        public bool grounded;
        public bool touchingLeftWall;
        public bool touchingRightWall;

        public Transform bottomLeftCorner;
        public Transform bottomRightCorner;
        public Transform topLeftCorner;


        public LayerMask layerMask;
        public int numRaycasts = 2;
        public float groundDrag = .9f;





        /// <summary>
        /// Snaps this to the given game object  given the direction
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="wall"></param>
        protected void snapToWall(Direction dir, GameObject wall)
        {
            float xPos = wall.transform.position.x;
            float yPos = wall.transform.position.y;



            float wallYOffset = wall.GetComponent<BoxCollider2D>().size.y / 2 * wall.transform.localScale.y;
            float wallXOffset = wall.GetComponent<BoxCollider2D>().size.x / 2 * wall.transform.localScale.x;

            float playerYOffset = gameObject.GetComponent<BoxCollider2D>().size.y / 2 * gameObject.transform.localScale.y;
            float playerXOffset = gameObject.GetComponent<BoxCollider2D>().size.x / 2 * gameObject.transform.localScale.x;


            float newXPos = transform.position.x;
            float newYPos = transform.position.y;

            switch (dir)
            {
                case Direction.Up:
                    newYPos = yPos - wallYOffset - playerYOffset;
                    speed.y = 0;
                    break;
                case Direction.Right:
                    newXPos = xPos - playerXOffset - wallXOffset;
                    speed.x = 0;
                    touchingRightWall = true;
                    break;
                case Direction.Down:
                    newYPos = yPos + wallYOffset + playerYOffset;
                    speed.y = 0;
                    grounded = true;
                    break;
                case Direction.Left:
                    newXPos = xPos + playerXOffset + wallXOffset;
                    speed.x = 0;
                    touchingLeftWall= true;
                    break;
                default:
                    break;
            }
            transform.position = new Vector3(newXPos, newYPos);

        }

        protected void Move()
        {
            if (speed.x > 0)
            {
                touchingLeftWall = false;
                Raycast(Direction.Right);
            }
            else if (speed.x < 0)
            {
                touchingRightWall = false;
                Raycast(Direction.Left);
            }

            if (speed.y > 0)
            {
                grounded = false;
                Raycast(Direction.Up);
            }
            else if (speed.y < 0)
            {
                Raycast(Direction.Down);
            }

            speed.x = speed.x + acceleration.x * Time.fixedDeltaTime;
            speed.y = speed.y + acceleration.y * Time.fixedDeltaTime + Globals.instance.gravity * Time.fixedDeltaTime;

            transform.position = new Vector3(transform.position.x + speed.x * Time.fixedDeltaTime, transform.position.y + speed.y * Time.fixedDeltaTime);

        }

        /// <summary>
        /// Checks all the ray casts, returns true if it snaps to the wall...
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        protected bool Raycast(Direction dir)
        {


            float offset = .01f;
            float distBetweenCorners;
            Vector2 raycastDirection = dir.getDirection();

            switch (dir)
            {
                case Direction.Up:
                case Direction.Down:
                    distBetweenCorners = topLeftCorner.position.y - offset - (bottomLeftCorner.position.y + offset);
                    break;
                case Direction.Right:
                case Direction.Left:
                    distBetweenCorners = bottomRightCorner.position.x - offset - (bottomLeftCorner.position.x + offset);
                    break;
                default:
                    Debug.LogError("Direction not passed!!");
                    distBetweenCorners = 1;
                    break;
            }

            Vector3 rayCastPos;


            for (int i = 0; i < numRaycasts; i++)
            {

                switch (dir)
                {
                    case Direction.Up:
                        rayCastPos = new Vector3(bottomLeftCorner.position.x + offset + i * distBetweenCorners / (numRaycasts - 1), topLeftCorner.position.y);
                        break;
                    case Direction.Down:
                        rayCastPos = new Vector3(bottomLeftCorner.position.x + offset + i * distBetweenCorners / (numRaycasts - 1), bottomLeftCorner.position.y);
                        break;
                    case Direction.Right:
                        rayCastPos = new Vector3(bottomRightCorner.position.x, bottomLeftCorner.position.y + offset + i * distBetweenCorners / (numRaycasts - 1));
                        break;
                    case Direction.Left:
                        rayCastPos = new Vector3(bottomLeftCorner.position.x, bottomLeftCorner.position.y + offset + i * distBetweenCorners / (numRaycasts - 1));
                        break;
                    default:
                        Debug.LogError("Direction not passed!!");
                        rayCastPos = Vector3.zero;
                        break;
                }

                RaycastHit2D hit = Physics2D.Raycast(rayCastPos, raycastDirection, 100, layerMask.value);

                if (hit.collider != null)
                {

                    float distance = 0;
                    switch (dir)
                    {
                        case Direction.Up:
                        case Direction.Down:
                            distance = Mathf.Abs(hit.point.y - rayCastPos.y);
                            Debug.DrawRay(rayCastPos, raycastDirection * distance, Color.green);
                            if (distance <= Mathf.Abs(speed.y * Time.fixedDeltaTime) + .01f)
                            {
                                snapToWall(dir, hit.collider.gameObject);
                                return true;
                            }
                            grounded = false;
                            break;
                        case Direction.Right:
                        case Direction.Left:
                            distance = Mathf.Abs(hit.point.x - rayCastPos.x);
                            Debug.DrawRay(rayCastPos, raycastDirection * distance, Color.green);
                            if (distance <= Mathf.Abs(speed.x * Time.fixedDeltaTime) + .01f)
                            {
                                snapToWall(dir, hit.collider.gameObject);
                                return true;
                            }
                            break;
                        default:
                            break;
                    }


                }


            }


            return false;
        }

        public void FixedUpdate()
        {
            Move();
        }




        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
