using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    Transform transform;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Awake()
    {
        instance = this;
    }
}
