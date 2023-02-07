using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{ 
    [Header("Navigation")]
    private WaypointCreator _waypointCreator;
    private List<Transform> path;

    [Space(10)] [Header("Enemy Vision")] 
    [SerializeField] private float senseRadius;
    [Range(0,360)] [SerializeField] private float viewAngle;
    [Space(5)]
    [SerializeField] private LayerMask target;
    [SerializeField] private LayerMask obstacles;
    private GameObject _player;
    private bool _seePlayer = false;
    
    private float _animationBlend;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private Animator _animator;
    private bool _hasAnimator;
    [SerializeField] private float animationSpeed;
    [SerializeField] private Animator fading;
    
    private NavMeshAgent _navMesh;

    private bool _scouting = false;
    private bool _boning = false;

    public GameObject excl;

    [SerializeField] private float catchDistance = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hasAnimator = TryGetComponent(out _animator);
        _waypointCreator = GetComponent<WaypointCreator>();
    }

    private void Start()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        StartCoroutine(Scan());
        path = _waypointCreator.waypoints.waypoints;
        _navMesh.destination = path[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_seePlayer)
        {
            CatchPlayer();
        }
        
        else if (Vector3.Distance(transform.position, _navMesh.destination) < 1.5f && !_scouting)
        {
            Scouter();
        }
        

        Animate();
    }

    private  void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Animate()
    {
        _animationBlend = Mathf.Lerp(_animationBlend, _navMesh.velocity.magnitude * animationSpeed, Time.deltaTime * 10f);
        if (_animationBlend < 0.01f) _animationBlend = 0f;
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
        }
    }

    private void Scouter()
    {
        _scouting = true;
        if (!_boning)
        {
            Transform nextEnd = path[0];
            path.Remove(nextEnd);
            path.Add(nextEnd);
        }
        else
            _boning = false;
        _navMesh.destination = path[0].position;
        _navMesh.isStopped = true;
        StartCoroutine(Scout());
    }
    
    
    private void Scanner()
    {
        if (_seePlayer) return;
        Collider[] inRange = Physics.OverlapSphere(transform.position, senseRadius, target);
        if (inRange.Length != 0)
        {
            Transform player = inRange[0].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                _seePlayer = !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacles);
                if (_seePlayer)
                    ChasePlayer();
            }
        }
    }



    private void ChasePlayer()
    {
        StopAllCoroutines();
        _scouting = false;
        _navMesh.isStopped = false;
        _navMesh.destination = _player.transform.position;
        var emitter = GameObject.Find("MusicController").GetComponent<MusicController>().GetCurrentEmitter();
        emitter.SetParameter("ChaseStage", 1);
        excl.GetComponent<Animator>().SetTrigger("Detect");
        _navMesh.speed = 3.4f;
        animationSpeed = 1.5f;
        _player.GetComponentInParent<ThirdPersonController>().MoveSpeed = 0.3f;
        _player.GetComponentInParent<ThirdPersonController>().SprintSpeed = 0.7f;
    }

    
    private void CatchPlayer()
    {
        _navMesh.destination = _player.transform.position;
        var distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= catchDistance)
        {
            fading.GetComponent<Animator>().SetTrigger("FadeOut");
            StartCoroutine(RestartAfterDelay());
        }
        else if (distance <= 3f)
        {
            var emitter = GameObject.Find("MusicController").GetComponent<MusicController>().GetCurrentEmitter();
            emitter.SetParameter("ChaseStage", 2);
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Restart();
    }


    private IEnumerator Scout()
    {
        yield return new WaitForSeconds(.5f);
        
        float t = 0.0f;
        while ( t  < 3f )
        {
            t += Time.deltaTime;
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * 25f);
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);
        
        t = 0.0f;
        while ( t  < 7f )
        {
            t += Time.deltaTime;
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * -30f);
            yield return null;
        }
        
        yield return new WaitForSeconds(3f);
        _scouting = false;
        _navMesh.isStopped = false;
    }
    

    private IEnumerator Scan()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            Scanner();
        }
    }


    private IEnumerator PlaySounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Monster", transform.position);
        }
    }


    public void Distract(GameObject bone)
    {
        if(_seePlayer)
            return;
        
        StopAllCoroutines();
        _scouting = false;
        _navMesh.isStopped = false;
        StartCoroutine(Scan());
        _navMesh.destination = bone.transform.position;
        _boning = true;
        var anim = excl.GetComponent<Animator>();
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("PopUp") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f))
            excl.GetComponent<Animator>().SetTrigger("Detect");
    }
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/EnemyFootstepsMonster", transform.position);
        }
    }
    
    
    [CustomEditor(typeof(EnemyAI))]
    public class FieldOfViewEditor : Editor
    {
        private void OnSceneGUI()
        {
            EnemyAI vision = (EnemyAI)target;
            Handles.color = Color.yellow;
            Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.senseRadius);

            Vector3 viewAngle01 = DirectionFromAngle(vision.transform.eulerAngles.y, -vision.viewAngle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(vision.transform.eulerAngles.y, vision.viewAngle / 2);

            Handles.color = Color.magenta;
            Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngle01 * vision.senseRadius);
            Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngle02 * vision.senseRadius);

            if (vision._seePlayer)
            {
                Handles.color = Color.green;
                Handles.DrawLine(vision.transform.position, vision._player.transform.position);
            }
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }


    public bool SeePlayer()
    {
        return _seePlayer;
    }
}
