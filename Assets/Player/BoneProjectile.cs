using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class BoneProjectile : MonoBehaviour
{
   private Rigidbody rb;

   [SerializeField] public Vector3 velocity;

   private void Awake()
   {
      rb = GetComponent<Rigidbody>();
   }

   private void Start()
   {
      float speed = 14f;
      //Vector3 aimDir = (transform.forward).normalized;
      rb.velocity =  velocity * speed;
   }

   private void OnTriggerEnter(Collider other)
   {
      Destroy(gameObject);
   }
}
