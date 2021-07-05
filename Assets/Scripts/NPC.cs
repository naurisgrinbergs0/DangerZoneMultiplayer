
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public LayerMask layerFloor, layerPlayer;

    public byte health = 100;
    public byte damage = 10;

    // patroling
    private Vector3 walkPoint;
    private bool _walkPointSet;
    public float walkPointRange;

    // attacking
    public float timeBetweenAttacks;
    private bool _alreadyAttacked;

    // states
    public float sightRange, attackRange;
    private bool _playerInSightRange, _playerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        RaycastHit hit;
        Vector3 playerPos = player.position;
        // check for sight range
        Physics.Raycast(transform.position, playerPos - transform.position, out hit, sightRange, ~gameObject.layer);
        _playerInSightRange = hit.collider != null && layerPlayer.value == (layerPlayer.value | (1 << hit.collider.gameObject.layer));
        
        Debug.DrawRay(transform.position, playerPos - transform.position, Color.red);
        //Debug.Log((hit.collider != null) + " " + hit.collider.gameObject.layer + " " + layerPlayer.value);
        //Debug.Log(layerPlayer.value == (layerPlayer.value | (1 << hit.collider.gameObject.layer)));
        //Debug.Log(LayerMask.LayerToName(hit.collider.gameObject.layer));

        // check attack range
        Physics.Raycast(transform.position, playerPos - transform.position, out hit, attackRange, ~gameObject.layer);
        _playerInAttackRange = hit.collider != null && hit.collider.gameObject.layer == layerPlayer.value;

        // actions
        if (!_playerInSightRange && !_playerInAttackRange) 
            Patroling();
        if (_playerInSightRange && !_playerInAttackRange) 
            ChasePlayer();
        if (_playerInAttackRange && _playerInSightRange) 
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!_walkPointSet)
            SetWalkPoint();

        if (_walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            _walkPointSet = false;
    }
    private void SetWalkPoint()
    {
        // calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, layerFloor))
            _walkPointSet = true;
    }

    private void ChasePlayer()
    {
        Debug.Log("bum");
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // make sure enemy doesn't move
        agent.SetDestination(transform.position);
        // look at player when attacking
        transform.LookAt(player);

        if (!_alreadyAttacked)
        {
            /// attack player
            player.GetComponent<Player>().TakeDamage(damage);

            // don't allow immediate attacks
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void TakeDamage(byte damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(Destroy), 0.5f);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }

}
