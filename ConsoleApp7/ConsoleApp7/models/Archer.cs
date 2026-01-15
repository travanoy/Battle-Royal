public class Archer : Character
{
    public int Agility { get; private set; }
    private Random _random = new Random();
    private const double DodgeChance = 0.25;

    public Archer(string name, int level = 1)
        : base(name, 100 + level * 18, level)
    {
        Agility = 18 + level * 3;
    }

    public override int GetAttack()
    {
        int baseDamage = Agility * 2 + Level * 4;

        if (_random.NextDouble() < 0.2)
        {
            return (int)(baseDamage * 1.5);
        }

        return baseDamage;
    }

    public override void TakeDamage(int damage)
    {
        if (_random.NextDouble() < DodgeChance)
        {
            return;
        }

        base.TakeDamage(damage);
    }

    public override string GetDetailedInfo()
    {
        return $"🏹 Agility: {Agility} | 💨 Dodge: {DodgeChance * 100}%";
    }
}