namespace ProjectIOU;

using System.IO;
public static class Program
{
    public static Func<string[], int> DisplayMenu = DefaultDisplayMenu;

    public static int DefaultDisplayMenu(string[] options)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.WriteLine($"> {options[i]}");
                }
                else
                {
                    Console.WriteLine($"  {options[i]}");
                }
            }

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % options.Length;
                    break;
                case ConsoleKey.Enter:
                    return selectedIndex; // Return the selected index
            }
        }
    }   
    static string loginsFile = "logins.txt";

    static void Main()
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("\n"+ "Welcome to IOU! Your expense sharing solution");
            string[] mainMenu = {"Log in", "Create Account", "Quit"};
            int chosenOption = DisplayMenu(mainMenu);
            if (chosenOption == 0)
            {
                Login();
            }
            else if (chosenOption == 1)
            {
                CreateAccount();
            }

            else
            {
                Console.WriteLine("See ya later!");
                return;
            }
        }
    
    }
    static void Login()
    {
        Console.Clear();
        Console.WriteLine("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (!File.Exists(loginsFile))
        {
            Console.WriteLine("No User Account exists, please create an account.");
            return;
        }

        var lines = File.ReadAllLines(loginsFile);
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            if (parts.Length == 2 && parts[0] == username && parts[1] == password)
            {
                // continue onto the main menu for mode selection?
                Proceed(new User {
                    Username = username, Password = password
                });
                return;  
            }
        }
        Console.WriteLine("Login failed, incorrect username and password.");
        Console.ReadKey();
    }


    static void CreateAccount()
    {
        Console.Write("Choose a username: ");
        string username = Console.ReadLine();

        Console.Write("Choose a password: ");
        string password = Console.ReadLine();

        // Make sure that the username does not already exist
        if (File.Exists(loginsFile))
        {
            var lines = File.ReadAllLines(loginsFile);
            foreach (var line in lines)
            {
                var parts = line.Split(':');
                if (parts[0] == username)
                {
                    Console.WriteLine("Username already exists! Pick a different username.");
                    return;
                }
            }
        }
        // append new username to logins file
        File.AppendAllText(loginsFile, username + ":" + password + "\n");
        User user = new User {
            Username = username, 
            Password = password
        }; 
        
        Proceed(user); 
    }
    static void Proceed(User user)
    {
        while (true) // need to return the user to the main menu if they enter an invalid input
        {
            Console.Clear();
            Console.WriteLine("Welcome. Please choose a mode: ");
            string[] modeOptions = { "Expense records", "Outstanding Debts", "Payment Reporting", "Go Back"};
            int chooseOption = DisplayMenu(modeOptions);

            switch (chooseOption)
            {
                case 0:
                    ExpenseRecords(user);
                    break;
                case 1:
                    Debt.OutstandingDebts(user);
                    break;
                case 2:
                    PaymentReporting.DisplayPaidDebtRecords(user);
                    break;
                case 3:
                    Console.WriteLine("Logging Out");
                    return; 
            }
        }

        static void ExpenseRecords(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose an option: ");
                string[] expenseChoice = {"Create a new expense record", "Delete an expense record", "Go Back"};            
                int selectedOption = DisplayMenu(expenseChoice);            
                if (selectedOption == 0)
                {
                    // call the method to create a new expense record from expenserecord.cs
                    ExpenseRecord.CreateExpenseRecord(user);
                }
                else if (selectedOption == 1)
                {
                    ExpenseRecord.DeleteExpenseRecord(user, "ExpenseRecords.txt", "Debts.txt"); 
                }
                else if (selectedOption == 2)
                {
                    return; 
                } 
            }
        }
    }
}
    
       