using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float attackRate = 1.5f;

    private NavMeshAgent _agent;
    private float _nextAttackTime;
    private Health _playerHealth;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;

        _playerHealth = player.GetComponent<Health>();
    }
    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            _agent.isStopped = false;
            _agent.SetDestination(player.position);
        }
        else
        {
            _agent.isStopped = true;

            // Lógica de ataque con temporizador
            if (Time.time >= _nextAttackTime)
            {
                AttackPlayer();
                _nextAttackTime = Time.time + attackRate;
            }
        }
    }
    private void AttackPlayer()
    {
        if (_playerHealth != null)
        {
            _playerHealth.TakeDamage(damage);
        }
    }
}