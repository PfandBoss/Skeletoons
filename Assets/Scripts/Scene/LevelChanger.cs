using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private Animator fading;
    private void OnTriggerEnter(Collider other)
    {
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        var emitter = GameObject.Find("MusicController").GetComponent<MusicController>().GetCurrentEmitter();
        int scene = SceneManager.GetActiveScene().buildIndex;
        switch (scene)
        {
            case 0:
                emitter.SetParameter("GameStarted", 1);
                StartCoroutine(LoadAfterDelay(4f, emitter));
                return;
            case 1:
                emitter.SetParameter("Level1Done", 1);
                StartCoroutine(LoadAfterDelay(4f, emitter));
                return;
            case 2:
                emitter.SetParameter("Level2Done", 1);
                StartCoroutine(LoadAfterDelay(4f, emitter));
                return;
            case 3:
                emitter.SetParameter("Level3Done", 1);
                StartCoroutine(LoadAfterDelay(4f, emitter));
                return;
            case 4:
                emitter.SetParameter("Level4Done", 1);
                StartCoroutine(EndGameAfterDelay(45f, emitter));
                return;
            default:
                return;
        }
    }

    private IEnumerator LoadAfterDelay(float delay, FMODUnity.StudioEventEmitter emitter)
    {
        fading.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(delay/2f);
        GameObject.Find("MusicController").GetComponent<MusicController>().PlayNextLevel();
        yield return new WaitForSeconds(delay);
        emitter.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    private IEnumerator EndGameAfterDelay(float delay, FMODUnity.StudioEventEmitter emitter)
    {
        fading.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(delay);
        emitter.Stop();
        Application.Quit();
    }
}
