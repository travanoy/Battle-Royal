public class TeamBattle
{
    private Team _team1;
    private Team _team2;
    private int _roundNumber;

    public TeamBattle(Team team1, Team team2)
    {
        _team1 = team1;
        _team2 = team2;
        _roundNumber = 0;
    }

    public void StartBattle()
    {
        Console.Clear();
        PrintHeader();

        Console.WriteLine("\n⚔️ TEAM BATTLE BEGINS! ⚔️\n");
        Thread.Sleep(1200);

        while (_team1.HasAliveMembers() && _team2.HasAliveMembers())
        {
            _roundNumber++;

            Console.Clear();
            Console.WriteLine($"\n╔═══════════════════════════════════════════════╗");
            Console.WriteLine($"║            ROUND {_roundNumber}                        ║");
            Console.WriteLine($"╚═══════════════════════════════════════════════╝\n");

            DisplayBothTeams();

            ExecuteTeamTurn(_team1, _team2);

            if (!_team2.HasAliveMembers())
                break;

            Console.WriteLine("\n" + new string('─', 50));
            Console.WriteLine("Press any key for second team turn...");
            Console.ReadKey();

            ExecuteTeamTurn(_team2, _team1);

            Console.WriteLine("\n" + new string('─', 50));
          
            
        }

        AnnounceWinner();
    }

    private void ExecuteTeamTurn(Team attackingTeam, Team defendingTeam)
    {
        Console.WriteLine($"\n\n▶ Turn:  {attackingTeam.TeamName}");

        var aliveAttackers = attackingTeam.GetAliveMembers();

        foreach (var attacker in aliveAttackers)
        {
            if (!defendingTeam.HasAliveMembers())
                break;

            Console.WriteLine($"\n┌─ Attacker: {attacker.Name} ({attacker.GetType().Name}) ─┐");

            var aliveDefenders = defendingTeam.GetAliveMembers();

            if (attacker is Mage mage && mage.CanAttackMultiple() && aliveDefenders.Count > 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n Mage can attack multiple targets!");
                Console.WriteLine("  [1] Single target attack (less mana, more damage)");
                Console.WriteLine($"  [2] Area attack (attack up to {mage.GetMaxTargets()} targets)");
                Console.ResetColor();

                int attackChoice = GetPlayerChoice(1, 2);

                if (attackChoice == 2)
                {
                    ExecuteAoeAttack(mage, aliveDefenders, defendingTeam.TeamColor);
                }
                else
                {
                    ExecuteSingleTargetAttack(attacker, aliveDefenders, defendingTeam.TeamColor);
                }
            }
            else
            {
                ExecuteSingleTargetAttack(attacker, aliveDefenders, defendingTeam.TeamColor);
            }

            Thread.Sleep(1500);
        }
    }

    private void ExecuteSingleTargetAttack(Character attacker, List<Character> aliveDefenders, ConsoleColor defenderColor)
    {
        Console.ForegroundColor = defenderColor;
        Console.WriteLine("\nSelect target:");
        for (int i = 0; i < aliveDefenders.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}] {aliveDefenders[i].Name} ({aliveDefenders[i].Health} HP)");
        }
        Console.ResetColor();

        int targetIndex = GetPlayerChoice(1, aliveDefenders.Count);
        Character target = aliveDefenders[targetIndex - 1];

        ExecuteAttack(attacker, target);
    }

    private void ExecuteAoeAttack(Mage mage, List<Character> aliveDefenders, ConsoleColor defenderColor)
    {
        Console.ForegroundColor = defenderColor;
        Console.WriteLine("\nSelect up to 3 targets for area attack:");
        for (int i = 0; i < aliveDefenders.Count; i++)
        {
            Console.WriteLine($"  [{i + 1}] {aliveDefenders[i].Name} ({aliveDefenders[i].Health} HP)");
        }
        Console.ResetColor();

        List<Character> targets = new List<Character>();
        int maxTargets = Math.Min(3, aliveDefenders.Count);

        Console.WriteLine($"\nEnter target numbers (1 to {maxTargets}), separated by space:");
        Console.Write("Targets: ");

        string input = Console.ReadLine();
        string[] choices = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string choice in choices)
        {
            if (int.TryParse(choice, out int index) && index >= 1 && index <= aliveDefenders.Count)
            {
                Character target = aliveDefenders[index - 1];
                if (!targets.Contains(target))
                {
                    targets.Add(target);
                }
            }

            if (targets.Count >= maxTargets)
                break;
        }

        if (targets.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("No targets selected, attacking first target");
            Console.ResetColor();
            targets.Add(aliveDefenders[0]);
        }

        int aoeDamage = mage.GetAoeAttack();

        if (aoeDamage == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n⏸️ {mage.Name} doesn't have enough mana for area attack!");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n💥🔮 {mage.Name} casts area spell!");
            Console.ResetColor();

            foreach (var target in targets)
            {
                int healthBefore = target.Health;
                target.TakeDamage(aoeDamage);
                int actualDamage = healthBefore - target.Health;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   → {target.Name}:  {aoeDamage} damage → received {actualDamage}");
                Console.ResetColor();

                if (aoeDamage > 0 && actualDamage == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"      💨 {target.Name} dodged!");
                    Console.ResetColor();
                }

                if (!target.IsAlive)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"      💀 {target.Name} defeated!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"      {target.Name}:  {target.Health}/{target.MaxHealth} HP");
                }
            }
        }
    }

    private void ExecuteAttack(Character attacker, Character defender)
    {
        int damage = attacker.GetAttack();

        if (damage == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n⏸️ {attacker.Name} skips turn!");
            Console.ResetColor();
        }
        else
        {
            int healthBefore = defender.Health;
            defender.TakeDamage(damage);
            int actualDamage = healthBefore - defender.Health;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n💥 {attacker.Name} → {defender.Name}");
            Console.WriteLine($"   Damage: {damage} → Received: {actualDamage}");
            Console.ResetColor();

            if (damage > 0 && actualDamage == 0)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"   💨 {defender.Name} dodged!");
                Console.ResetColor();
            }

            if (!defender.IsAlive)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"   💀 {defender.Name} defeated!");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"   {defender.Name}: {defender.Health}/{defender.MaxHealth} HP");
            }
        }
    }

    private int GetPlayerChoice(int min, int max)
    {
        while (true)
        {
            Console.Write($"\nYour choice [{min}-{max}]: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
            {
                return choice;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error!  Enter number from {min} to {max}");
            Console.ResetColor();
        }
    }

    private void DisplayBothTeams()
    {
        _team1.DisplayTeam();
        Console.WriteLine("\n         🆚         \n");
        _team2.DisplayTeam();
        Console.WriteLine();
    }

    private void AnnounceWinner()
    {
        Console.Clear();
        Console.WriteLine("\n\n" + new string('═', 60));
        Console.WriteLine($"   BATTLE ENDED AFTER {_roundNumber} ROUNDS!");
        Console.WriteLine(new string('═', 60));

        Team winner = _team1.HasAliveMembers() ? _team1 : _team2;
        Team loser = _team1.HasAliveMembers() ? _team2 : _team1;

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n🏆 WINNER TEAM: {winner.TeamName}!  🏆");
        Console.ResetColor();

        Console.ForegroundColor = winner.TeamColor;
        Console.WriteLine("\nSurvivors:");
        foreach (var member in winner.GetAliveMembers())
        {
            Console.WriteLine($"  ✓ {member.Name} - {member.Health}/{member.MaxHealth} HP");
        }
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"\n💀 Defeated team: {loser.TeamName}");
        Console.ResetColor();

        Console.WriteLine("\n" + new string('═', 60));
    }

    private void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n" + new string('═', 60));
        Console.WriteLine("           ⚔️  TEAM BATTLE ARENA  ⚔️");
        Console.WriteLine(new string('═', 60));
        Console.ResetColor();
    }
}