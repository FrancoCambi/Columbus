using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("Item Info")]
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;

    public string ItemName => itemName;
    public Sprite Icon => icon;
}