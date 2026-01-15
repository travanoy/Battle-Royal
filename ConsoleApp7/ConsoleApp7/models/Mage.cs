public class Mage : Character
{
    public int Intelligence { get; private set; }
    public int Mana { get; private set; }
    public int MaxMana { get; private set; }
    private const int SpellCost = 30;
    private const int AoeSpellCost = 60;

    public Mage(string name, int level = 1)
        : base(name, 80 + level * 15, level)
    {
        Intelligence = 20 + level * 3;
        MaxMana = 150 + level * 15;
        Mana = MaxMana;
    }

    public override int GetAttack()
    {
        if (Mana < SpellCost)
        {
            Mana += (int)(MaxMana * 0.2);
            if (Mana > MaxMana)
                Mana = MaxMana;

            return 0;
        }

        Mana -= SpellCost;
        return Intelligence * 3 + Level * 5;
    }

    public int GetAoeAttack()
    {
        if (Mana < AoeSpellCost)
        {
            Mana += (int)(MaxMana * 0.2);
            if (Mana > MaxMana)
                Mana = MaxMana;

            return 0;
        }

        Mana -= AoeSpellCost;
        return Intelligence * 2 + Level * 4;
    }

    public override bool CanAttackMultiple()
    {
        return Mana >= AoeSpellCost;
    }

    public override int GetMaxTargets()
    {
        return 3;
    }

    public override string GetDetailedInfo()
    {
        return $"🔮 Intelligence: {Intelligence} | 💙 Mana: {Mana}/{MaxMana}";
    }
}