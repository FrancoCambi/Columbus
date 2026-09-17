using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly List<ItemData> _items = new();

    public IReadOnlyList<ItemData> Items => _items;

    public void AddItem(ItemData item)
    {
        if (item == null || _items.Contains(item))
            return;

        _items.Add(item);

        Debug.Log($"Item añadido al inventario: {item.ItemName}");
    }

    public bool HasItem(ItemData item)
    {
        return _items.Contains(item);
    }
}