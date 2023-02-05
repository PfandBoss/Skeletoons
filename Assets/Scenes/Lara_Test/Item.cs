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
    private Vector3 _itemScale;

    [SerializeField] private float dropDownForce = 1.0f;
    [SerializeField] private bool scaleDown = true;
    [SerializeField] private Vector3 scale = Vector3.one;
    public Transform itemContainer;
    public Transform finalParent;
    
    // stand in
    private KeyCode dropKey = KeyCode.K;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (_holding && Input.GetKeyDown(dropKey))
        {
            DropDown();
        }
    }

    public void Interact()
    {
        if (!_holding)
        {
            PickUp();
        }
    }

    private void PickUp()
    {
        _holding = true;
        ItemController.HoldingItem = true;
        ItemController.HeldItem = gameObject;
        _itemScale = transform.localScale;
        
        _rb.isKinematic = true;
        
        transform.SetParent(itemContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (scaleDown)
        {
            transform.localScale = scale;
        }
    }

    public void DropDown()
    {
        _holding = false;
        ItemController.HoldingItem = false;
        ItemController.HeldItem = null;

        transform.SetParent(null);

        _rb.isKinematic = false;
        transform.localScale = _itemScale;

        _rb.AddForce(Vector3.down * dropDownForce, ForceMode.Impulse);
        float random = Random.Range(-1f, 1f);
        _rb.AddTorque(new Vector3(random, random, random) * 10);
    }
    
    public void DropOn()
    {
        _holding = false;
        ItemController.HoldingItem = false;
        ItemController.HeldItem = null;

        transform.SetParent(finalParent);

        transform.localScale = _itemScale;
        transform.position = new Vector3(0.2826202f, 12.291f, 21.106f);
    }

    public bool GetHolding()
    {
        return _holding;
    }
    
    private void OnDrawGizmos()
    {
        if (!InteractableItemsOverview.DrawingGizmos()) return;
        
        Gizmos.color = InteractableItemsOverview.DoorColor();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawSphere(Vector3.zero, 0.2f);
    }
}
