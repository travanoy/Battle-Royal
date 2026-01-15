public class Warrior : Character
{
    public int Strength { get; private set; }
    public int Armor { get; private set; }

    public Warrior(string name, int level = 1)
        : base(name, 120 + level * 20, level)
    {
        Strength = 15 + level * 2;
        Armor = 10 + level;
    }

    public override int GetAttack()
    {
        return Strength * 2 + Level * 3;
    }

    public override void TakeDamage(int damage)
    {
        int reducedDamage = damage - Armor;

        if (reducedDamage < 0)
            reducedDamage = 0;

        base.TakeDamage(reducedDamage);
    }

    public override string GetDetailedInfo()
    {
        return $"⚔️ Strength: {Strength} | 🛡️ Armor:  {Armor}";
    }
}