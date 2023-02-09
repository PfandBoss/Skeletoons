using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private static MusicController _musicCtrlInstance;
    [SerializeField] private List<FMODUnity.StudioEventEmitter> emitters;
    private FMODUnity.StudioEventEmitter _currentEmitter;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        if (_musicCtrlInstance == null)
            _musicCtrlInstance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _currentEmitter = emitters[SceneManager.GetActiveScene().buildIndex];
        if (!_currentEmitter.IsPlaying())
        {
            _currentEmitter.Play();
        }
        _currentEmitter.SetParameter("ChaseStage", 0);
    }

    public FMODUnity.StudioEventEmitter GetCurrentEmitter()
    {
        return _currentEmitter;
    }

    public void PlayNextLevel()
    {
        _currentEmitter = emitters[SceneManager.GetActiveScene().buildIndex+1];
        if (!_currentEmitter.IsPlaying())
        {
            _currentEmitter.Play();
        }
        _currentEmitter.SetParameter("ChaseStage", 0);
    }
}
