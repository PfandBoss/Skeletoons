using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionSwitch : MonoBehaviour
{
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 rotation;
    [SerializeField] float fadingTime = 1.0f;
    [SerializeField] float waitingTime = 1.0f;
    [SerializeField] Image image;
    private GameObject _player;


    private void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        
        Debug.Log("Player entered " + col.gameObject.transform.position);
        _player = col.gameObject;
        _player.GetComponent<CharacterController>().enabled = false;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        Debug.Log("startin fade in coroutine");
        for (float i = 0; i <= fadingTime/2f; i += Time.deltaTime)
        {
            // set color with i as alpha
            image.color = new Color(0, 0, 0, (i/(fadingTime/2)));
            yield return null;
        }
        _player.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        Debug.Log("ending fade in coroutine: " + image.color);
        yield return new WaitForSeconds(waitingTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Debug.Log("startin fade out coroutine");
        for (float i = fadingTime/2f; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            image.color = new Color(0, 0, 0, (i/(fadingTime/2)));
            yield return null;
        }
        _player.GetComponent<CharacterController>().enabled = true;
    }
}
