using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Quaternion = System.Numerics.Quaternion;
using Random = UnityEngine.Random;

public class BoneProjectile : MonoBehaviour
{

   [SerializeField] private float baitRadius = 7f;
   private void OnTriggerEnter(Collider other)
   {
      FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/BoneThrow/Bone", transform.position);
      if (other.gameObject.GetComponent<CharacterController>() == null)
      {
         Collider[] hitColliders = Physics.OverlapSphere(transform.position, baitRadius);
         foreach (var hitCollider in hitColliders)
         {
            if (hitCollider.gameObject.GetComponent<EnemyAI>() as EnemyAI != null)
            {
               hitCollider.gameObject.GetComponent<EnemyAI>().Distract(this.gameObject);
            }
         }
      }
      Invoke(nameof(Delete),4f);
   }

   private void Delete()
   {
      Destroy(this.gameObject);
   }

   private void Awake()
   {
      // Set a random rotation, because why not
      Vector3 randomRotation = new Vector3(Random.Range(5f, 7f), 0, Random.Range(5f, 7f));
      // Apply the rotation to the rigid body
      GetComponent<Rigidbody>().AddRelativeTorque(randomRotation, ForceMode.Impulse);
   }
}
