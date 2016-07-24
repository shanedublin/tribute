using UnityEngine;
using System.Collections;
using System;

public class Grapple : Projectile
{
    private Vector3 _target;
    public bool shoot = false;
    public Vector3 target
    {
        set
        {
            _target = new Vector3(value.x, value.y, 0);
        }
        get
        {
            return _target;
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    new public void  FixedUpdate()
    {
        if(shoot)
        {
            transform.position = target;
            shoot = false;
            grounded = false;
        }
        base.FixedUpdate();
    }

    protected override void EntityInput()
    {
        
    }
}
