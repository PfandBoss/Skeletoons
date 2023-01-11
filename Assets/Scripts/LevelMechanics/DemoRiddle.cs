using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoRiddle : MonoBehaviour
{

    [SerializeField] SteppablePlatform[] platforms;
    [SerializeField] ParticleSystem fire;

    private string _currentOrder;
    private bool _solved;

    // Update is called once per frame
    void Update()
    {
        if (_solved)
        {
            return;
        }
        _currentOrder = "";
        _currentOrder += platforms[0].Stepped ? 1 : 0;
        _currentOrder += platforms[1].Stepped ? 1 : 0;
        _currentOrder += platforms[2].Stepped ? 1 : 0;

        switch (_currentOrder)
        {
            case "100" or "110":
                return;
            case "111":
                _solved = true;
                StartCoroutine(Solved());
                return;
            default:
                platforms[0].Stepped = false;
                platforms[1].Stepped = false;
                platforms[2].Stepped = false;
                break;
        }
    }

    private IEnumerator Solved()
    {
        fire.Play();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
