using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public enum InteractableType
    {
        Door,
        Locker,
        Item
    }

    public InteractableType type;

    public void Interact(GameObject player)
    {
        Debug.Log("Interact");
        switch (type)
        {
            case InteractableType.Door:
                gameObject.GetComponent<Door>().Interact();
                break;
            case InteractableType.Locker:
                gameObject.GetComponent<Locker>().Interact(player);
                break;
            case InteractableType.Item:
                gameObject.GetComponent<Item>().Interact();
                break;
        }
    }
}
