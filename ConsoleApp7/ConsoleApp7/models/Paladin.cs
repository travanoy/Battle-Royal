public class Paladin : Character
{
    public int HolyPower { get; private set; }
    public int Armor { get; private set; }
    private const int HealThreshold = 40;
    private const int HealCost = 50;

    public Paladin(string name, int level = 1)
        : base(name, 130 + level * 22, level)
    {
        HolyPower = 100;
        Armor = 8 + level;
    }

    public override int GetAttack()
    {
        if (Health < MaxHealth * 0.4 && HolyPower >= HealCost)
        {
            int healAmount = 30 + Level * 5;
            Health += healAmount;
            if (Health > MaxHealth)
                Health = MaxHealth;

            HolyPower -= HealCost;
            return 0;
        }

        HolyPower += 10;
        if (HolyPower > 100)
            HolyPower = 100;

        return 12 * Level + 20;
    }

    public override void TakeDamage(int damage)
    {
        int reducedDamage = damage - Armor / 2;
        if (reducedDamage < 0)
            reducedDamage = 0;

        base.TakeDamage(reducedDamage);
    }

    public override string GetDetailedInfo()
    {
        return $"✨ Holy Power: {HolyPower} | 🛡️ Armor: {Armor}";
    }
}