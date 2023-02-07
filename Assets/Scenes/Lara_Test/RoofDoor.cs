using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoofDoor : Door
{
    private string _old = "";
    protected override void OpenDoor()
    {
        // animate open door
        open = true;
        gameObject.GetComponentInParent<Animation>().Play();
        key.GetComponent<Item>().DropOn();
    }
    
    public override void Interact()
    {
        if (open) return;
        
        if (key == null | key.GetComponent<Item>().GetHolding())
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Door", transform.position);
            OpenDoor();
        }
        else
        {
            _old = infoText.GetComponent<TMP_Text>().text;
            infoText.GetComponent<TMP_Text>().text = "Maybe I need to put something in there?";
            infoText.SetActive(true);
            Invoke(nameof(resetInfo),3.0f);
        }
    }

    protected void resetInfo()
    {
        infoText.SetActive(false);
        infoText.GetComponent<TMP_Text>().text = _old;
    }
}
