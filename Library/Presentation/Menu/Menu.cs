namespace Presentation.Menu;

using Interfaces;

public class Menu(IEnumerable<IMenuAction> actions)
{
    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Library Management System");

            foreach (var action in actions)
            {
                Console.WriteLine($"{action.Key}. {action.Description}");
            }

            Console.WriteLine("0. Exit");

            Console.Write("\nSelect an option: ");
            var input = Console.ReadLine();
            if (input == "0")
            {
                return;
            }

            var selected = actions.FirstOrDefault(a => a.Key == input);

            Console.Clear();

            if (selected is null)
            {
                Console.WriteLine("Invalid input.");
            }
            else
            {
                await selected.ExecuteAsync();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}