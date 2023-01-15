using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    private bool _holding;
    private Rigidbody _rb;
    private BoxCollider _collider;

    [SerializeField] private float dropDownForce = 1.0f;
    public Transform itemContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected: " + collision.gameObject.name);
        if (!collision.gameObject.CompareTag("Player")) return;
        PickUp();
    }

    public void PickUp()
    {
        _holding = true;
        ItemController.HoldingItem = true;
        ItemController.HeldItem = gameObject;
        
        _rb.isKinematic = true;
        
        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
    }

    public void DropDown()
    {
        _holding = false;
        ItemController.HoldingItem = false;
        ItemController.HeldItem = null;

        transform.SetParent(null);

        _rb.isKinematic = false;

        _rb.AddForce(Vector3.down * dropDownForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        _rb.AddTorque(new Vector3(random, random, random) * 10);
    }
}
