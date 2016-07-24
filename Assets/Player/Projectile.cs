using UnityEngine;
using System.Collections;
using Raycasting;

public abstract class Projectile : Raycaster
{


    public Vector2 drag;
    
    public float airDrag;

    public int numVeriticalRayCasts = 2;
    public int numHorizontalRayCasts = 2;

    public float moveForce;

    protected abstract void EntityInput();

    new public void FixedUpdate()
    {
        if (grounded)
        {
            speed.x = speed.x * groundDrag;
            if (Mathf.Abs(speed.x) <= 0.1f)
                speed.x = 0;
        }
        base.FixedUpdate();
    }
}
