using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowCooldown : MonoBehaviour
{
    
    public Image coooldownIcon;
    private float cooldownTime;
    private float remainingCooldownTime;
    
    
    private void Start()
    {
        cooldownTime = GetComponent<ThirdPersonThrowingController>().throwCooldown;
    }

    private void Update()
    {
        remainingCooldownTime -= Time.deltaTime;
        coooldownIcon.fillAmount = remainingCooldownTime / cooldownTime;
    }

    public void StartCooldown()
    {
        remainingCooldownTime = cooldownTime;
    }
   
}
