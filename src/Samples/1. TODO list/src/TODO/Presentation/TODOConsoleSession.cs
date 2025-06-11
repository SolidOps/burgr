namespace SolidOps.TODO.Presentation
{
    public partial class TODOConsoleSession
    {
        public async Task Run()
        {
            string? command = default;
            while (command != "quit")
            {
                Console.WriteLine("Enter command (type help for details):");
                command = Console.ReadLine();
                if (command == "help")
                {
                    DisplayCommands();
                    Console.WriteLine("help: list commands");
                    Console.WriteLine("clear: clear session memory");
                    Console.WriteLine("quit: quit the program");
                }
                else if (command == "clear")
                {
                    await Clear();
                }
                else if (IsCommand(command))
                {
                    await Call(command);
                }
                else if (command != "quit")
                {
                    Console.WriteLine($"{command} is not a known command");
                }
            }
        }
    }
}
