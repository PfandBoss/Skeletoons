using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

public class ThirdPersonThrowingController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;

    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform pfBone;
    [SerializeField] private Transform spawnBonePosition;
    private ThirdPersonController _thirdPersonController;
    private StarterAssetsInputs _starterAssetsInputs;


    private void Awake()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }
        if (_starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            _thirdPersonController.SetSensitivity(aimSensitivity);
            _thirdPersonController.SetRotateOnMove(false);
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime + 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            _thirdPersonController.SetRotateOnMove(true);
            _thirdPersonController.SetSensitivity(normalSensitivity);
        }

        if (_starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBonePosition.position).normalized;
            Instantiate(pfBone, spawnBonePosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            
            _starterAssetsInputs.shoot = false;
        }
    }
}
