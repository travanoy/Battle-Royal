public abstract class Character
{
    public string Name { get; protected set; }
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public int Level { get; protected set; }

    public bool IsAlive => Health > 0;

    protected Character(string name, int health, int level)
    {
        Name = name;
        Health = health;
        MaxHealth = health;
        Level = level;
    }

    public abstract int GetAttack();

    public virtual void TakeDamage(int damage)
    {
        if (damage < 0) damage = 0;

        Health -= damage;

        if (Health < 0)
            Health = 0;
    }

    public virtual string GetStatus()
    {
        return $"{Name} | HP:  {Health}/{MaxHealth} | Lvl: {Level}";
    }

    public virtual string GetDetailedInfo()
    {
        return "";
    }
    public virtual bool CanAttackMultiple()
    {
        return false;
    }

    public virtual int GetMaxTargets()
    {
        return 1;
    }
}