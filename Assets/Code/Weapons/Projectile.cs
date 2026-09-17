using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifetime = 4f;

    private int _damage;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int damageAmount)
    {
        _damage = damageAmount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectil choco con; " +  collision.gameObject.name);

        Health health = collision.collider.GetComponentInParent<Health>();

        if (health != null)
        {
            Debug.Log("encontro health - daño: " + _damage);
            health.TakeDamage(_damage);
        }

        Destroy(gameObject);
    }
}
