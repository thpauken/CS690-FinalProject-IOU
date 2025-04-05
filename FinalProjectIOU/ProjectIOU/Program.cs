namespace ProjectIOU;

using System.IO;
class Program
{

    static string loginsFile = "logins.txt";

    static void Main()
    {
        // intro
        Console.WriteLine("Welcome to IOU! Your expense sharing solution");
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
                Proceed(username);
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
        Proceed(username);
    }
    static void Proceed(string username)
    {
        Console.WriteLine("Welcome. Please choose a mode: ");
        Console.WriteLine("1. Expense Records");
        Console.WriteLine("2. Outstanding Debts");
        Console.WriteLine("3. Reporting Activity");
    }



}
