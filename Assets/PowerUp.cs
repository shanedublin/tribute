﻿using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    public enum Power{
        WallBoots, TeleBall, DoubleJump
    }
    public Power power;
    // Use this for initialization

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController controller = collision.GetComponent<PlayerController>();
        if (controller != null)
        {
            switch (power)
            {
                case Power.WallBoots:
                    controller.wallBoots = true;
                    break;
                case Power.TeleBall:
                    controller.teleBall = true;
                    break;
                case Power.DoubleJump:
                    controller.doubleJumpBoots = true;
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }

    }
}