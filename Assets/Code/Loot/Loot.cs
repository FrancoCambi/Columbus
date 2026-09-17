using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        LootRegistry.Register(this);
    }

    protected virtual void OnDisable()
    {
        LootRegistry.Unregister(this);
    }

    public abstract void Pickup();
}
