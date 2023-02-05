using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static MusicController _musicCtrlInstance;
    
    [SerializeField] private List<EnemyAI> Enemies;
    
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
        
        var emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        if (!emitter.IsPlaying())
        {
            emitter.Play();
        }
        emitter.SetParameter("ChaseStage", 0);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
