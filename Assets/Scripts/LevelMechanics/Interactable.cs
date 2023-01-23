using System;
using StarterAssets;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ThirdPersonController>().SetInteractable(this);
        }
    }

    public void Interact()
    {
        print("INTERACTING");
    }
}
