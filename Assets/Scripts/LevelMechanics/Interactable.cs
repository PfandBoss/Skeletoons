using System;
using StarterAssets;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Vector3 solution;
    public float totalRings;
    private GameObject _outerRing;
    private GameObject _middleRing;
    private GameObject _innerRing;
    private GameObject[] rings;
    private int _activeRing;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ThirdPersonController>().SetInteractable(this);
        }
    }

    private void Start()
    {
        totalRings = 3;
        _outerRing = transform.GetChild(0).gameObject;
        _middleRing = transform.GetChild(1).gameObject;
        _innerRing = transform.GetChild(2).gameObject;
        rings = new[] { _outerRing, _middleRing, _innerRing };
    }

    public void Interact(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.A:
                if (_activeRing > 0) _activeRing--;
                break;
            case KeyCode.D:
                if (_activeRing < totalRings) _activeRing++;
                break;
            case KeyCode.W:
                rings[_activeRing].transform.Rotate(Vector3.back, 90);
                break;
            case KeyCode.S:
                rings[_activeRing].transform.Rotate(Vector3.back, -90);
                break;
        }
        var solved = true;

        for (var i = 0; i < totalRings; i++)
        {
            print(rings[i].transform.rotation.eulerAngles.z);
            if (Math.Abs(rings[i].transform.rotation.eulerAngles.z - solution[i]) > TOLERANCE)
            {
                solved = false;
                break;
            }

        }
        if (solved) print("SOLVED");
    }

    private const double TOLERANCE = 1;
}