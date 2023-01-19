using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : MonoBehaviour
{
    [SerializeField] private float exitDistance = 2f;
    
    private bool _entered = false;
    private GameObject _player;
    
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
        
        _player.transform.position = new Vector3(gameObject.transform.position.x, _player.transform.position.y,
            gameObject.transform.position.z);
        _player.transform.rotation = gameObject.transform.rotation;
    }

    private void Exit()
    {
        _entered = false;
        
        _player.transform.position += gameObject.transform.forward * exitDistance;
        _player.GetComponent<CharacterController>().enabled = true;
    }
}
