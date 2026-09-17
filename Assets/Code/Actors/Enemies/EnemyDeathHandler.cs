using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour, IDeathHandler
{
    public void Die()
    {
        Debug.Log($"{gameObject.name} destruido.");
        Destroy(gameObject);
    }

}