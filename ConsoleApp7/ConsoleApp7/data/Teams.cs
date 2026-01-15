public class Team
{
    public string TeamName { get; private set; }
    public List<Character> Members { get; private set; }
    public ConsoleColor TeamColor { get; private set; }

    public Team(string teamName, ConsoleColor color)
    {
        TeamName = teamName;
        Members = new List<Character>();
        TeamColor = color;
    }

    public void AddMember(Character character)
    {
        Members.Add(character);
    }

    public bool HasAliveMembers()
    {
        return Members.Any(m => m.IsAlive);
    }

    public List<Character> GetAliveMembers()
    {
        return Members.Where(m => m.IsAlive).ToList();
    }

    public bool HasCharacterType(Type characterType)
    {
        return Members.Any(m => m.GetType() == characterType);
    }

    public void DisplayTeam()
    {
        Console.ForegroundColor = TeamColor;
        Console.WriteLine($"\n╔═══ {TeamName} ═══╗");

        for (int i = 0; i < Members.Count; i++)
        {
            Character member = Members[i];
            string status = member.IsAlive ? "✓" : "✗";
            Console.WriteLine($"  [{i + 1}] {status} {member.GetStatus()}");
            if (member.IsAlive && !string.IsNullOrEmpty(member.GetDetailedInfo()))
            {
                Console.WriteLine($"      {member.GetDetailedInfo()}");
            }
        }

        Console.WriteLine("╚" + new string('═', TeamName.Length + 10) + "╝");
        Console.ResetColor();
    }
}