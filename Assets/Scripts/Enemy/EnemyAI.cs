using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{ 
    [Header("Navigation")]
    [SerializeField] private List<Transform> path;

    [Space(10)] [Header("Enemy Vision")] 
    [SerializeField] private float senseRadius;
    [Range(0,360)] [SerializeField] private float viewAngle;
    [Space(5)]
    [SerializeField] private LayerMask target;
    [SerializeField] private LayerMask obstacles;
    private GameObject _player;
    private bool _seePlayer = false;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    public AudioClip[] FootstepAudioClips;
    
    private float _animationBlend;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private Animator _animator;
    private bool _hasAnimator;
    
    private NavMeshAgent _navMesh;

    private bool _scouting = false;
    private bool _boning = false;
    // Start is called before the first frame update
    void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hasAnimator = TryGetComponent(out _animator);
    }

    private void Start()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        StartCoroutine(Scan());
        _navMesh.destination = path[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _navMesh.destination) < 1.5f && !_scouting)
        {
            Scouter();
        }
        
        //Do here maybe something when _seePlayer true
        if(_seePlayer)
            Debug.Log("I SEE U BITCH");
        Animate();
    }

    private void Animate()
    {
        _animationBlend = Mathf.Lerp(_animationBlend, _navMesh.velocity.magnitude * 1.71f, Time.deltaTime * 10f);
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
        //Get everything inRange of radius if it is a target
        Collider[] inRange = Physics.OverlapSphere(transform.position, senseRadius, target);

        //if target inRange check for obstacles in way (There can only be 1 target)
        if (inRange.Length != 0)
        {
            //Get Dir to target
            Transform player = inRange[0].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            //Check if it is in our viewing angel
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                _seePlayer = !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacles);
            }
            else
                _seePlayer = false;
        }
        //Reset
        else if (_seePlayer)
            _seePlayer = false;
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


    public void Distract(GameObject bone)
    {
        StopAllCoroutines();
        _scouting = false;
        _navMesh.isStopped = false;
        StartCoroutine(Scan());
        _navMesh.destination = bone.transform.position;
        _boning = true;
    }
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(transform.position),
                    FootstepAudioVolume * 2f);
            }
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
}
