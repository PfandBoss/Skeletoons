using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class BoneProjectile : MonoBehaviour
{
   

   private void OnTriggerEnter(Collider other)
   {
      Destroy(gameObject);
   }
}
