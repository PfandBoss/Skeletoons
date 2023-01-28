using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    [SerializeField] private float exitDistance = 1.5f;
    
    private bool _entered = false;
    private GameObject _player;
    private float _rotationOffset = 90f;
    
    public void Interact(GameObject player)
    {
        if (!_entered)
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
        _entered = true;
        _player.GetComponent<CharacterController>().enabled = false;
        
        _player.transform.localRotation.eulerAngles.Set(0, gameObject.transform.rotation.eulerAngles.y + _rotationOffset, 0);
        _player.transform.position = new Vector3(gameObject.transform.position.x, _player.transform.position.y,
            gameObject.transform.position.z) + _player.transform.forward * -0.5f;
    }

    private void Exit()
    {
        Debug.Log("exiting");
        _entered = false;
        
        _player.transform.position += _player.transform.forward * exitDistance;
        _player.GetComponent<CharacterController>().enabled = true;
    }
}
