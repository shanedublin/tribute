using UnityEngine;
using System.Collections;
using Raycasting;

public abstract class Entity : Raycaster
{

    
    public Vector2 drag;
    public float airDrag;






    public int numVeriticalRayCasts = 2;
    public int numHorizontalRayCasts = 2;



    public float moveForce;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
 

    new public void FixedUpdate()
    {
        
        EntityInput();
        
        speed.x *= groundDrag;
        if (Mathf.Abs(speed.x) <= .01f)
            speed.x = 0;
        base.FixedUpdate();
       
    }

    protected abstract void EntityInput();


}
