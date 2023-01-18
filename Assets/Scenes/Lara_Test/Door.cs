using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject key;
    [SerializeField] private bool open = false;

    public void Interact()
    {
        if (open) return;
        
        if (key.GetComponent<Item>().GetHolding())
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // animate open door
        open = true;
        gameObject.GetComponentInParent<Animation>().Play();
    }
}
