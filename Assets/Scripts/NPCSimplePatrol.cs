using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSimplePatrol : MonoBehaviour{

    //Tells whether agent waits on each node
    [SerializeField]
    bool _patrolWaiting;

    //Time we wait at each node
    [SerializeField]
    float _totalWaitTime = 3f;

    //Probability of switching direction 
    [SerializeField]
    float _switchProbability = 0.2f;

    //list of all patrol nodes to visit
    [SerializeField]
    List<Waypoint> _patrolPoints;

    //private Vars for base behavior
    NavMeshAgent _navMeshAgent;
    int _currentPatrolIndex;
    bool _travelling;
    bool _waiting;
    bool _patrolFoward;
    float _waitTimer;

    private bool followPlayer;
    public Transform playerTarget;

    // Use this for initialization

    public void Start(){
        followPlayer = false;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null){
            Debug.Log("Nav mesh agent component is not attached to " + gameObject.name);
        }
        else{
            if (_patrolPoints != null && _patrolPoints.Count >= 2){
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else{
                Debug.Log("Insufficent patrol points for basic patrolling behavior");
            }
        }
    }

    // Update is called once per frame
    void Update(){
        if (followPlayer){
            playerSeen();
        }
        else{
            playerNotSeen();
        }
    }

    private void playerSeen(){
        Debug.Log("player is seen");
        _navMeshAgent.destination = playerTarget.position;
    }

    private void playerNotSeen(){
        // check if near to destination
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0f){
            _travelling = false;
            //if wait true then wait
            if (_patrolWaiting){
                _waiting = true;
                _waitTimer = 0f;
            }
            else{
                ChangePatrolPoint();
                SetDestination();
            }
        }
        else{
            if (_waiting){
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= _totalWaitTime){
                    _waiting = false;
                    ChangePatrolPoint();
                    SetDestination();
                }
            }
        }
    }

    private void SetDestination(){
        if (_patrolPoints != null){
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
        }
    }
    //summary
    //selects a new patrol point in the list but also with a
    // small chance to allow us to move foward or backward
    private void ChangePatrolPoint(){
        if (UnityEngine.Random.Range(0f, 1f) <= _switchProbability){
            _patrolFoward = !_patrolFoward;
        }
        if (_patrolFoward){
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else{
            _currentPatrolIndex--;
            if (_currentPatrolIndex < 0){
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
    //Vector3.distance
    /*private void OnTriggerStay(Collider player)
    {
        followPlayer = true;
    }*/



    private void OnTriggerEnter(Collider player)
    {
        followPlayer = true;
    }
    /*private void OnTriggerExit(Collider player)
    {
        followPlayer = false;
    }*/
}
