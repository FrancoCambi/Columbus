using System.Collections.Generic;

public static class LootRegistry
{
    private static readonly List<Loot> _loots = new();

    public static IReadOnlyList<Loot> Loots => _loots;

    public static void Register(Loot loot)
    {
        if (!_loots.Contains(loot))
            _loots.Add(loot);
    }

    public static void Unregister(Loot loot)
    {
        _loots.Remove(loot);
    }
}