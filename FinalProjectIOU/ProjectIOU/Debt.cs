namespace ProjectIOU;

// instantiating the Debt class properties
public class Debt
{
   public Person OwingPerson { get; set; }
   public Person OwedToPerson { get; set; }
   public decimal OwedAmount { get; set; }
   public PaidStatus PaidStatus { get; set; }
   public ExpenseRecord LinkedExpenseRecord { get; set; } // stores reference to expense record

// defining the constructor
// defining how default values are set like PaidStatus set to Unpaid, and PaidDate default value until it is later updated when PaidStatus is Paid
   public Debt(Person owingPerson, Person owedToPerson, decimal owedAmount, ExpenseRecord linkedExpenseRecord)
   {
      OwingPerson = owingPerson;
      OwedToPerson = owedToPerson;
      OwedAmount = owedAmount;
      PaidStatus = PaidStatus.Unpaid;
      LinkedExpenseRecord = linkedExpenseRecord; 
   }
   public static void OutstandingDebts(User loggedInUser)
   {
      while(true) // keep showing the user the list of outstanding debts until they choose to return
      {
         if (!File.Exists("Debts.txt"))
         {
            Console.WriteLine("No debts are owed to you!");
            Console.ReadKey();
            return;
         }
         var debtLines = File.ReadAllLines("Debts.txt"); // define this variable to read all lines 
         // read through all the debt lies in the Debts.txt file, return those that are unpaid and the owedToPerson is the logged in user
         var unpaidDebts = debtLines
            .Select(line => line.Split(','))
            .Where(parts => parts.Length >= 5 && parts[4].Trim() == "Unpaid" && parts[2].Trim() == loggedInUser.Username)
            .ToList();

         if (unpaidDebts.Count == 0)
         {
            Console.WriteLine("No debts are owed to you!");
            Console.ReadKey();
            return;
         }
         // display the unpaid debt records that are found
         Console.Clear();
         Console.WriteLine("Outstanding Debts:\n");
         string[] debtOptions = unpaidDebts.Select((debt, index) =>
            $"{index + 1}. Person: {debt[1]} | Amount: {debt[3]}"
        ).Concat(new[] { "Go Back" }).ToArray();

        int selectedOption = Program.DisplayMenu(debtOptions);

        if (selectedOption == debtOptions.Length - 1) // "Go Back" option
        {
            return;
        }
         
         Console.Clear();
        var selectedDebt = unpaidDebts[selectedOption];
        Console.WriteLine($"Do you want to mark this debt as paid?\nPerson: {selectedDebt[1]} | Amount: {selectedDebt[3]}");
        string[] confirmationOptions = { "Yes, mark as paid", "No, go back" };
        int confirmationChoice = Program.DisplayMenu(confirmationOptions);

        if (confirmationChoice == 1) // "No, go back"
        {
            continue;
        }

        // Mark the debt as paid
        selectedDebt[4] = "Paid"; // Update the PaidStatus to Paid

        var updatedDebtLines = debtLines.Select(line =>
        {
            var parts = line.Split(',');
            if (parts[0] == selectedDebt[0] && parts[1] == selectedDebt[1] && parts[2] == selectedDebt[2] && parts[3] == selectedDebt[3])
            {
                parts[4] = "Paid"; // Mark this specific debt as paid
            }
            return string.Join(",", parts);
        }).ToList();

        File.WriteAllLines("Debts.txt", updatedDebtLines);
        Console.WriteLine($"\nDebt owed by {selectedDebt[1]} for {selectedDebt[3]} has been marked as paid!\n");
        Console.ReadKey();
      }
   }
}