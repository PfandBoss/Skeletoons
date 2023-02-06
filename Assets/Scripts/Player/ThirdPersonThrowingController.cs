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
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform spawnBonePosition;
    private Animator _animator;
    private bool _hasAnimator;

    public Transform attackPoint;
    public GameObject objectToThrow;
    
    private ThirdPersonController _thirdPersonController;
    private StarterAssetsInputs _starterAssetsInputs;


    [Header("Settings")] 
    public int totalThrows;
    public float throwCooldown;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;

    [Header("Throwing")] 
    public float throwForce;
    public float throwUpwardForce;

    private int _animIDThrow;
    private bool readyToThrow = true;

    private void Awake()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        _hasAnimator = TryGetComponent(out _animator);
    }

    private void Start()
    {
        _animIDThrow = Animator.StringToHash("Throwing");
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);
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

        if (_starterAssetsInputs.shoot && readyToThrow && totalThrows > 0)
        {
            /*if (_hasAnimator)
            {
                _animator.SetBool(_animIDThrow, true);
            }*/
            Vector3 aimDir = (mouseWorldPosition - spawnBonePosition.position).normalized;
            readyToThrow = false;
            GameObject projectile = Instantiate(objectToThrow, attackPoint.position, Quaternion.Euler(aimDir));
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/BoneThrow/Throw", transform.position);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            Vector3 forceToAdd = aimDir * throwForce + transform.up * (throwUpwardForce * ((aimDir.y - -0.5f) / (0.5f - -0.5f)));
            
            projectileRigidbody.AddForce(forceToAdd,ForceMode.Impulse);
            totalThrows--;
            GetComponent<ThrowCooldown>().StartCooldown();
            readyToThrow = false;
            Invoke(nameof(ResetThrow),throwCooldown);
            _starterAssetsInputs.shoot = false;
        }
        else
        {
            //_animator.SetBool(_animIDThrow, false);
        }
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
