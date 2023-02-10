using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicChanger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null)
        {
            var emitter = GameObject.Find("MusicController").GetComponent<MusicController>().GetCurrentEmitter();
            emitter.SetParameter("BossFight", 1);
        }
    }
}
