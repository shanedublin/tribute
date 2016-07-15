using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour
{

    public static Globals instance;

    public Vector3 spawnPoint;
    public float gravity = -1f;

    public void Awake()
    {
        instance = this;
    }
}
