using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 4f;

    private int damage;

    public void Initialize(int damageAmount)
    {
        damage = damageAmount;
    }
    void Start()
    {
        Destroy(gameObject, lifetime);

        Collider projectileCollider = GetComponent<Collider>();

        // IGNORA COLISION CON JUGADOR
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Collider[] playerColliders = player.GetComponentsInChildren<Collider>();

            foreach (Collider playerCollider in playerColliders)
            {
                Physics.IgnoreCollision(projectileCollider, playerCollider);
            }
        }

        //IGNORA COLISION ENTRE PROYECTILES
        Projectile[] otherProjectiles = FindObjectsByType<Projectile>(
            FindObjectsSortMode.None);

        foreach (Projectile other in otherProjectiles)
        {
            if (other != this)
            {
                Collider otherCollider = other.GetComponent<Collider>();

                if (otherCollider != null)
                {
                    Physics.IgnoreCollision(projectileCollider, otherCollider);
                }
            }
        }

        
        //IGNORA COLISION CON ARMAS
        GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");

        if (weapon != null)
        {
            Collider[] weaponColliders = weapon.GetComponentsInChildren<Collider>();

            foreach (Collider weaponCollider in weaponColliders)
            {
                Physics.IgnoreCollision(projectileCollider, weaponCollider);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectil choco con; " +  collision.gameObject.name);

        Health health = collision.collider.GetComponentInParent<Health>();

        if (health != null)
        {
            Debug.Log("encontro health - daño: " + damage);
            health.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
