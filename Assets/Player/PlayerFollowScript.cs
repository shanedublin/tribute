using UnityEngine;
using System.Collections;

public class PlayerFollowScript : MonoBehaviour
{

    public Transform player;

    public float xDelta = 2;
    public float yDelta = 2;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LateUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.z = -10;

        if(player.position.x - transform.position.x  > xDelta)
        {
            newPos.x += xDelta- (player.position.x  - transform.position.x) ;
        }


        Vector3 pos = Vector3.Lerp(transform.position, player.position, Time.deltaTime);

        // The following if statements check to see if the player is inside the box
        if(Mathf.Abs(player.position.x - transform.position.x) < xDelta)
        {
            pos.x = transform.position.x;
        }

        if(Mathf.Abs(player.position.y - transform.position.y ) < yDelta){
            pos.y = transform.position.y;
        }


        pos.z = -10;
        transform.position = pos;
//        transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime);
        //transform.position = newPos;

    }
}
