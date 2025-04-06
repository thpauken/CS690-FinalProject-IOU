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
      if (!File.Exists("Debts.txt"))
      {
         Console.WriteLine("No debts are owed to you!");
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
         return;
      }
      // display the unpaid debt records that are found
      Console.WriteLine("Outstanding Debts:");
      foreach (var debt in unpaidDebts)
      {
         Console.WriteLine("Person: " + debt[1] + "|" + " Amount: " + debt[3] + "\n");
      }
   }
}