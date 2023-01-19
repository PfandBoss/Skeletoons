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
    
    // stand in
    private KeyCode dropKey = KeyCode.E;
    
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

    public bool GetHolding()
    {
        return _holding;
    }
}
