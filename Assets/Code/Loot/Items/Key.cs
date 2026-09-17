using UnityEngine;

public class Key : Loot
{
    [Header("Item Data")]
    [SerializeField] private KeyData keyData;

    private bool _pickedUp;

    public override void Pickup()
    {
        if (_pickedUp)
            return;

        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning("No se encontró un Inventory en la escena.");
            return;
        }

        _pickedUp = true;

        inventory.AddItem(keyData);

        Debug.Log("Llave recogida");

        gameObject.SetActive(false);
    }
}