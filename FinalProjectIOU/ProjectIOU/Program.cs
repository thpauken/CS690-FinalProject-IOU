namespace ProjectIOU;

using System.IO;
class Program
{

    static string loginsFile = "logins.txt";

    static void Main()
    {
        // intro
        Console.WriteLine("\n"+ "Welcome to IOU! Your expense sharing solution");
        Console.WriteLine("Choose an option (1 or 2) ");
        Console.WriteLine("1. Log in");
        Console.WriteLine("2. Create Account");
        string option = Console.ReadLine();

        if (option == "1")
        {
            Login();
        }
        else if (option == "2")
        {
            CreateAccount();
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }

        
    }
    static void Login()
    {
        Console.WriteLine("\nUsername: ");
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
    }

    static void CreateAccount()
    {
        Console.Write("\nChoose a username: ");
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
        Console.WriteLine("Welcome. Please choose a mode: ");
        Console.WriteLine("1. Expense Records");
        Console.WriteLine("2. Outstanding Debts");
        Console.WriteLine("3. Reporting Activity");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                ExpenseRecords(user);
                break;
            case "2":
                Console.WriteLine("Outstanding Debts is not yet implemented.");
                break;
        
            // add this logic
            case "3":
                Console.WriteLine("Reporting Activity is not implemented yet.");
                break;
        
            // add this logic
        }
    }

    static void ExpenseRecords(User user)
    {
        Console.WriteLine(" Do you want to:" + "\n" + "1. Create a new expense record" + "\n" + "2. Delete an expense record");
        string option = Console.ReadLine();
        if (option == "1")
        {
            // call the method to create a new expense record from expenserecord.cs
            ExpenseRecord.CreateExpenseRecord(user);
        }
        else if (option == "2")
        {
            ExpenseRecord.DeleteExpenseRecord(); 
        }
        else
        {
            Console.WriteLine("Invalid option selected. Exiting program.");
        }
    }


}
