using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    // Raycast Variables
    [SerializeField] private int rayLength;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private string blockingLayer;
    
    private string _interactableTag = "Interactable";
    private GameObject _raycastObject;
    private InteractableObject _raycastInteractable;
    // stand in
    public KeyCode interactionKey = KeyCode.Q;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            Debug.Log("Shooting Ray");
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out var hit, rayLength, interactionLayer))
            {
                Debug.Log("Hit Obstacle");
                if (hit.collider.CompareTag(_interactableTag))
                {
                    _raycastObject = hit.collider.gameObject;
                    Debug.Log("Raycast hit: " + _raycastObject.name);
                    _raycastObject.GetComponent<InteractableObject>().Interact(gameObject);
                }
            }
        }
    }
}
