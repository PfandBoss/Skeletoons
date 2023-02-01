using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using StarterAssets;
using UnityEngine;

public class Locker : MonoBehaviour
{
    // [SerializeField] private float exitDistance = 1.5f;
    [SerializeField] private float _rotationOffset = 0f;

    private static bool hiding = false;
    private Vector3 _formerPosition;
    private GameObject _player;
    private SkinnedMeshRenderer _playerRenderer;

    public void Interact(GameObject player)
    {
        if (hiding)
        {
            Exit();
        }
        else if (!hiding)
        {
            _player = player;
            Enter();
        }
        else
        {
            Exit();
        }
    }

    private void Enter()
    {
        _player.GetComponent<CharacterController>().enabled = false;
        _player.GetComponent<ThirdPersonThrowingController>().enabled = false;
        _player.GetComponent<StarterAssetsInputs>().enabled = false;

        _formerPosition = _player.transform.position;
        
        _player.transform.position = new Vector3(gameObject.transform.position.x, _player.transform.position.y,
            gameObject.transform.position.z) + _player.transform.forward * -0.5f;
        
        _player.transform.rotation = Quaternion.Euler(0, (gameObject.transform.eulerAngles.y) % 360, 0);

        hiding = true;
    }

    private void Exit()
    {
        Debug.Log("exiting");

        _player.transform.position = _formerPosition;

        _player.GetComponent<CharacterController>().enabled = true;
        _player.GetComponent<ThirdPersonThrowingController>().enabled = true;
        _player.GetComponent<StarterAssetsInputs>().enabled = true;

        hiding = false;
    }

    public static bool isHiding()
    {
        return hiding;
    }

    private void OnDrawGizmos()
    {
        if (!InteractableItemsOverview.DrawingGizmos()) return;
        
        Gizmos.color = InteractableItemsOverview.CoffinColor();
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawSphere(new Vector3(0, 0.25f, 0), 0.1f);
    }
}
