using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppablePlatform : MonoBehaviour
{
    public bool Stepped = false;


    private void OnTriggerEnter(Collider other)
    {
        Stepped = true;
    }
}
