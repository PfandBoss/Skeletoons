using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject key;
    [SerializeField] private bool open = false;
    [SerializeField] private GameObject infoText;

    public void Interact()
    {
        if (open) return;
        
        if (key == null | key.GetComponent<Item>().GetHolding())
        {
            OpenDoor();
        }
        else
        {
            infoText.SetActive(true);
            Invoke(nameof(resetInfo),1.0f);
        }
    }

    private void OpenDoor()
    {
        // animate open door
        open = true;
        gameObject.GetComponentInParent<Animation>().Play();
        key.GetComponent<Item>().DropDown();
    }

    private void OnDrawGizmos()
    {
        if (!InteractableItemsOverview.DrawingGizmos()) return;
        
        Gizmos.color = InteractableItemsOverview.DoorColor();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawSphere(new Vector3(-0.6f, 1f, 0f), 0.2f);
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawLine(gameObject.transform.position + Vector3.up, key.transform.position);
    }
    
    private void resetInfo()
    {
        infoText.SetActive(false);
    }
}
