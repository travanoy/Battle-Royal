public class Berserker : Character
{
    public int Rage { get; private set; }
    public int Strength { get; private set; }
    private const double RageThreshold = 0.5;
    private const double RageDamageMultiplier = 1.5;
    private const double RageDamageReduction = 0.3;

    public Berserker(string name, int level = 1)
        : base(name, 140 + level * 25, level)
    {
        Strength = 18 + level * 2;
        Rage = 0;
    }

    private bool IsEnraged => (double)Health / MaxHealth < RageThreshold;

    public override int GetAttack()
    {
        int baseDamage = Strength * 2 + Level * 3 + Rage;

        if (IsEnraged)
        {
            Rage += 5;
            return (int)(baseDamage * RageDamageMultiplier);
        }

        return baseDamage;
    }

    public override void TakeDamage(int damage)
    {
        if (IsEnraged)
        {
            damage = (int)(damage * (1 - RageDamageReduction));
        }

        base.TakeDamage(damage);
    }

    public override string GetDetailedInfo()
    {
        string rageStatus = IsEnraged ? "🔥 ENRAGED!" : " Calm";
        return $"💪 Strength: {Strength} | ⚡ Rage: {Rage} | {rageStatus}";
    }
}