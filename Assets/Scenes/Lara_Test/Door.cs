using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] protected GameObject key;
    [SerializeField] protected bool open = false;

    public void Interact()
    {
        if (open) return;
        
        if (key == null | key.GetComponent<Item>().GetHolding())
        {
            OpenDoor();
        }
    }

    protected virtual void OpenDoor()
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
}
