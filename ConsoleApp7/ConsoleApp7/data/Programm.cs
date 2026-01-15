class Program
{
    private const string CorrectPassword = "DND";

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (!Login())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n❌ Access denied!");
            Console.ResetColor();
            Thread.Sleep(2000);
            return;
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║     ⚔️  TEAM BATTLE SYSTEM  ⚔️               ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");
        Console.ResetColor();

        GameMode mode = SelectGameMode();

        int teamSize = (int)mode;

        Team team1 = new Team("NAVI", ConsoleColor.Yellow);
        Team team2 = new Team("FALCONS", ConsoleColor.Green);

        Console.WriteLine($"\nTeam creation: each team consists of {teamSize} fighter(s)\n");

        SelectTeamMembers(team1, teamSize);
        Console.WriteLine("\n" + new string('─', 50) + "\n");
        SelectTeamMembers(team2, teamSize);

        Console.WriteLine("\n\nTeams formed!");
        Console.WriteLine("\nPress any key to start battle...");
        Console.ReadKey();

        TeamBattle battle = new TeamBattle(team1, team2);
        battle.StartBattle();

        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
    }

    static GameMode SelectGameMode()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(""" 
▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
▒██▄▒▒▒▒▒▒▒▀██▒▒
▒▒▀██▄▒▒▒▒▒▒▒▀▒▒
▒▒▒▒▀██▄▒▒▒▒▒▒▒▒
▒▒▒▒▒▒▀██▄▄▒▒▒▒▒
▒▒▒▒▒▒▒▀████▄▒▒▒
▒▒█▄▒▒▒▒▒▀███▒▒▒
▒▒▀▀▀▒▒▒▒▒▒▒▒▒▒▒
▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
────────────────
▐▀▄─▄▀▄▐██▐▀▌▄▀▄
▐─█─▌─▐─▐─▐▀▌─▄▀
▐▄▀─▀▄▀─▐─▐─▌█▄▄
""");
        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║          🎮  SELECT GAME MODE  🎮            ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");
        Console.ResetColor();

        Console.WriteLine("  [1] 1v1 - Duel (one fighter per team)");
        Console.WriteLine("  [3] 3v3 - Full team battle");

        int choice = GetPlayerChoice(1, 3, null);

        return (GameMode)choice;
    }

    static bool Login()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔═══════════════════════════════════════════════╗");
        Console.WriteLine("║            BATTLE ARENA LOGIN                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════╝\n");
        Console.ResetColor();

        int attempts = 3;

        while (attempts > 0)
        {
            Console.WriteLine("""
                 ________________________________________
                ______________888888888888____________________
                _____________8888888888888888888__________________
                __________18888888888888888888888888______________
                ________88888888888888888888888888888__8__________
                _______888888888888-Elites-888888888888_________
                _______88888888888888888888888888______888________
                ______88888888888888888888888888888888___86_______
                _______8888888888888____88888888865888888_8_______
                _____8_8888888_8888881__88888___________88________
                _____88________8888888888888_____________8________
                ______888____8888__888:8886_______________8_______
                ________8:_8888__________88_______________88______
                _____8__58_88____________8888____________888888___
                ______8__888_____________:8_888_______688888888___
                __________88____________88____:8888888888888888___
                ___________8__________:888______8888888____88_____
                ____________8________888888______8888____________
                ____________888888888888888___18888888___________
                """);
            Console.Write("Enter password: ");
            string password = ReadPassword();

            if (password == CorrectPassword)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n✓ Access granted!  Welcome!");
                Console.ResetColor();
                Thread.Sleep(1500);
                return true;
            }

            attempts--;

            if (attempts > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n❌ Wrong password!  Attempts left: {attempts}\n");
                Console.ResetColor();
            }
        }

        return false;
    }

    static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    static void SelectTeamMembers(Team team, int teamSize)
    {
        Console.ForegroundColor = team.TeamColor;
        Console.WriteLine($"---- : {team.TeamName} ----");
        Console.ResetColor();

        HashSet<Type> selectedTypes = new HashSet<Type>();

        for (int i = 1; i <= teamSize; i++)
        {
            Console.WriteLine($"\n▶ Select fighter #{i}:");

            DisplayClassOptions(selectedTypes);

            int choice = GetPlayerChoice(1, 5, selectedTypes);

            Console.Write("Enter fighter name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
                name = $"Fighter {i}";

            Character character = choice switch
            {
                1 => new Warrior(name, 5),
                2 => new Mage(name, 5),
                3 => new Archer(name, 5),
                4 => new Berserker(name, 5),
                5 => new Paladin(name, 5),
                _ => new Warrior(name, 5)
            };

            selectedTypes.Add(character.GetType());
            team.AddMember(character);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {character.Name} ({character.GetType().Name}) added to team!");
            Console.ResetColor();
        }

        Console.ForegroundColor = team.TeamColor;
        Console.WriteLine($"\n---- Team {team.TeamName} formed!-----");
        Console.ResetColor();
    }

    static void DisplayClassOptions(HashSet<Type> selectedTypes)
    {
        DisplayOption(1, "Warrior", typeof(Warrior), selectedTypes, "high armor");
        DisplayOption(2, "Mage", typeof(Mage), selectedTypes, "area attacks");
        DisplayOption(3, "Archer", typeof(Archer), selectedTypes, "dodge and crits");
        DisplayOption(4, "Berserker", typeof(Berserker), selectedTypes, "rage at low HP");
        DisplayOption(5, "Paladin", typeof(Paladin), selectedTypes, "healing");
    }

    static void DisplayOption(int number, string name, Type characterType, HashSet<Type> selectedTypes, string description)
    {
        bool isSelected = selectedTypes.Contains(characterType);

        if (isSelected)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  [{number}] {name} - {description} ❌ ALREADY SELECTED");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"  [{number}] {name} - {description}");
        }
    }

    static int GetPlayerChoice(int min, int max, HashSet<Type> selectedTypes)
    {
        while (true)
        {
            Console.Write($"\nYour choice [{min}-{max}]: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
            {
                if (selectedTypes != null)
                {
                    Type selectedType = choice switch
                    {
                        1 => typeof(Warrior),
                        2 => typeof(Mage),
                        3 => typeof(Archer),
                        4 => typeof(Berserker),
                        5 => typeof(Paladin),
                        _ => null
                    };

                    if (selectedType != null && selectedTypes.Contains(selectedType))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("❌ This class already selected!  Choose another.");
                        Console.ResetColor();
                        continue;
                    }
                }

                return choice;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error! Enter number from {min} to {max}");
            Console.ResetColor();
        }
    }
}