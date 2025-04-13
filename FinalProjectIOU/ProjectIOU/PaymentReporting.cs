namespace ProjectIOU;
using System.IO;

public class PaymentReporting
{
    public static void DisplayPaidDebtRecords (User loggedInUser)
    {
        if (!File.Exists("Debts.txt"))
        {
            Console.WriteLine("There are no debt records, regardless of paid or unpaid!");
            Console.ReadKey();
            return;
        }
        
        var debtLines = File.ReadAllLines("Debts.txt");
        // this will filter Outstandig debt records just to those that are "Paid" status and created by logged in user
        var paidDebts = debtLines
            .Select(line => line.Split(','))
            .Where(parts => parts.Length == 6 && string.Equals(parts[4].Trim(), "Paid") && string.Equals(parts[2].Trim(), loggedInUser.Username))
            .ToList();

        if (paidDebts.Count == 0)
        {
            Console.WriteLine("No payments have been made to you!");
            Console.ReadKey();
            return;
        }

        //display the list of payments made to logged in user
        Console.Clear();
        Console.WriteLine("Paid Debts previously owed to you:\n");
        foreach (var debt in paidDebts)
        {
            string owingPerson = debt[1];
            string expenseRecord = GetExpenseRecordName(debt[0]);
            string amountPaid = debt[3];
            string paidDate = debt[5];

            Console.WriteLine("Person: " + owingPerson);
            Console.WriteLine("Expense Record: " + expenseRecord);
            Console.WriteLine("Amount Paid: " + amountPaid);
            Console.WriteLine("Paid Date: " + paidDate);
            Console.WriteLine("-------------------------------------");
        }
        Console.WriteLine("Press 'Enter'to go back to the main menu");
        Console.ReadKey();    
    }
    private static string GetExpenseRecordName(string expenseID) // method to get expense record name instead of unique ID from debts.txt and expenserecords.txt files
    {
        var expenseLines = File.ReadAllLines("ExpenseRecords.txt");
        var expenseRecord = expenseLines
            .Select(line => line.Split(','))
            .FirstOrDefault(parts => parts.Length >= 2 && parts[0].Trim() == expenseID);
            // display unknown if can't get the expense record name from unique ID
         return expenseRecord != null ? expenseRecord[1].Trim() : "Unknown"; 
    }   
}



