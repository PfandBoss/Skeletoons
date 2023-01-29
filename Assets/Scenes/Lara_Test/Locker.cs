using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    // [SerializeField] private float exitDistance = 1.5f;
    [SerializeField] private float _rotationOffset = 0f;

    private static bool hiding = false;
    private Vector3 _formerPosition;
    private GameObject _player;
    
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
        
        _formerPosition = _player.transform.position;
        
        //_player.transform.localRotation.eulerAngles.Set(0, gameObject.transform.rotation.eulerAngles.y + _rotationOffset, 0);
        _player.transform.position = new Vector3(gameObject.transform.position.x, _player.transform.position.y,
            gameObject.transform.position.z) + _player.transform.forward * -0.5f;
        hiding = true;
    }

    private void Exit()
    {
        Debug.Log("exiting");

        //_player.transform.position += _player.transform.forward * 0.5f;
        
        _player.transform.position = _formerPosition;
        _player.transform.rotation = Quaternion.Euler(0, (gameObject.transform.eulerAngles.y  + _rotationOffset) % 360, 0); 

        _player.GetComponent<CharacterController>().enabled = true;
        hiding = false;
    }

    public static bool isHiding()
    {
        return hiding;
    }
}
