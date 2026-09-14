using UnityEngine;
using UnityEngine.AI;

public class EnemyFF : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;
    public float combatDistance = 12f;
    public float fireRate = 2f;
    public float repositionCooldown = 3.5f;

    private NavMeshAgent agent;
    private float nextFireTime;
    private float nextRepositionTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // Fija la mirada sin inclinar el modelo en el eje Y
        Vector3 lookPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPosition);

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > combatDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }

            if (Time.time >= nextRepositionTime)
            {
                Reposition();
                nextRepositionTime = Time.time + repositionCooldown;
            }
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(
                projectilePrefab,
                firePoint.position,
                firePoint.rotation);

            EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();

            if (projectileScript != null)
            {
                projectileScript.Initialize(projectileSpeed, 10);
            }
        }
    }

    void Reposition()
    {
        // Genera un vector aleatorio dentro de una esfera de 6 metros
        Vector3 randomDirection = Random.insideUnitSphere * 6f;
        randomDirection += transform.position;

        NavMeshHit hit;
        // Valida que el punto aleatorio exista dentro de las áreas caminables
        if (NavMesh.SamplePosition(randomDirection, out hit, 6f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}