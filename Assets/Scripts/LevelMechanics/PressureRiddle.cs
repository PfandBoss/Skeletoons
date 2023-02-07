using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureRiddle : MonoBehaviour
{
    [SerializeField] private SteppablePlatform[] platforms;
    private bool solved;

    [SerializeField] private GameObject PuzzleHint;


    private void Update()
    {
        if (solved) return;

        foreach(SteppablePlatform platform in platforms)
        {
            if(platform.Stepped == false) return;
        }
        
        PuzzleHint.SetActive(true);
        
    }
}
