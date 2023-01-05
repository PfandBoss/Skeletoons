using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class BoneProjectile : MonoBehaviour
{
   

   private void OnTriggerEnter(Collider other)
   {
      Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50f);
      foreach (var hitCollider in hitColliders)
      {
         if (hitCollider.gameObject.GetComponent<EnemyAI>() as EnemyAI != null)
         {
            hitCollider.gameObject.GetComponent<EnemyAI>().Distract(this.gameObject);
            
         }
      }
      Invoke(nameof(Delete),0.5f);
   }

   private void Delete()
   {
      Destroy(this.gameObject);
   }
}
