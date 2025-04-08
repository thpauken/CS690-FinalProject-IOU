namespace ProjectIOU;

using System.IO;

public class ExpenseRecord
{
    // instantiate the class and attributes, including the empty list of debts
    public Guid ID { get; set; }
    public string ExpenseName { get; set; }
    public User CreationUser { get; set; }
    public decimal TotalExpenses { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<Debt> DebtsList { get; set; }
    // define the constructor with defaults such as creationUser as the current logged in user, new unique ID, setting total expenses to 0, createddate as now
    public ExpenseRecord(string expenseName, User creationUser)
    { 
        ID = Guid.NewGuid();
        ExpenseName = expenseName;
        CreationUser = creationUser;
        TotalExpenses = 0;
        CreatedDate = DateTime.UtcNow;
        DebtsList = new List<Debt>();
    }
    // Defining method for adding each debt to the debt list and summing total expenses based on debt
    public void AddDebt(Debt debt)
    {
        DebtsList.Add(debt);
        TotalExpenses += debt.OwedAmount;
    }
    // save the expense record to a file
    public void SaveToFile()
    {
        // create a string for all of the variables saved within an expense record
        string expenseString = ID + "," + ExpenseName + "," + CreationUser.Username + "," + TotalExpenses + "," + CreatedDate;
        // Append this record to the expense records text file
        File.AppendAllText("ExpenseRecords.txt", expenseString + "\n");
        // put the debt records in a separate debts file to be queried in debts mode
        foreach (var debt in DebtsList)
        {
            string debtString = ID + "," + debt.OwingPerson.Name + "," + debt.OwedToPerson.Name + "," + debt.OwedAmount + "," + debt.PaidStatus;
            File.AppendAllText("Debts.txt", debtString + "\n");
        }
    }
    //mthod to create new expense records
    public static void CreateExpenseRecord(User user)
    {
        Console.WriteLine("Choose a name for your expense record: " + "\n" + "Type 'back' to go back");
        string expenseName = Console.ReadLine();
        if (expenseName == "back")
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(expenseName))
        {
            Console.WriteLine("Expense name cannot be empty.");
            return;
        }
        ExpenseRecord newRecord = new ExpenseRecord(expenseName, user); // creating the new expense record and storing the creating user as the logged in user

        bool moreDebts = true; // variable to check when user is adding more debt records, will end when this is false 

        while (moreDebts)
        {
            Console.WriteLine("Enter the name of the person that owes, or 'done' when finished:");
            string personName = Console.ReadLine();
            if (personName == "done")
            { 
                moreDebts = false;
            }
            else
            {
                
                Console.WriteLine("Enter the amount they owe: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal owedAmount) == false)
                {
                    Console.WriteLine("Invalid amount. Please enter a valid number.");
                    continue;
                }
                else 
                {
                decimal owedAmount = Convert.ToDecimal(Console.ReadLine());
                // create a debt record and add the person name, OwedToPerson as the logged-in-user, and defaulting debts to unpaid
                Person owingPerson = new Person(personName);
                Person owedToPerson = new Person (user.Username);
                Debt newDebt = new Debt(owingPerson, owedToPerson, owedAmount, newRecord);
                newRecord.AddDebt(newDebt);
            }   }
        // save the debt records to a debt file
        }
    newRecord.SaveToFile();
    Console.WriteLine("Expense record " + expenseName + " created!");
    }
    
    // method to delete existing expense records
    public static void DeleteExpenseRecord()
    {
        if (!File.Exists("ExpenseRecords.txt"))
        {
            Console.WriteLine("No expense records exist.");
            Console.ReadKey();
            return;
        }
        // fixed issue with blank rows in expense file shifting the index of listed expense records 
        var lines = File.ReadAllLines("ExpenseRecords.txt").Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        if (lines.Length == 0)
        {   
            Console.WriteLine("No expense records exist.");
            Console.ReadKey();
            return;
        }
        // display the names of the existing expense records
        Console.Clear();
        Console.WriteLine("Expense Records:");
        string[] expenseNames = lines.Select((line, index) =>
        {
            var parts = line.Split(',');
            return (index + 1) + ". " + parts[1];
        }).ToArray();
        
        string[] options = expenseNames.Concat(new[] { "Go Back" }).ToArray();
        int choice = DisplayMenu(options);
        if (choice == options.Length - 1)
        {
            return;
        }
        
        Console.WriteLine("Are you sure you want to delete?");
        string[] confirming = {"Yes", "No"};
        int confirmationChoice = DisplayMenu(confirming);
        if (confirmationChoice == 1)
        {
            return;
        }
        var expenseRecordToBeDeleted = lines[recordNumber - 1];
        var expenseRecordParts = expenseRecordToBeDeleted.Split(',');
        var expenseIdToDelete = expenseRecordParts[0];

        // delete the specified expense record
        // isolate the record to delete in an array 
        var updatedLines = lines.Where((line, index) => index != recordNumber - 1).ToArray(); 
        File.WriteAllLines("ExpenseRecords.txt", updatedLines);
        Console.WriteLine("Deleted expense record and associated debts");             
    
        // now delete associated debt records; put them in an array, match to expense record ID, delete
        var debtLines = File.ReadAllLines("Debts.txt").Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        var updatedDebtLines = debtLines.Where(line => !line.StartsWith(expenseIdToDelete)).ToArray();
        File.WriteAllLines("Debts.txt", updatedDebtLines);

        Console.WriteLine("Deleted expense records and associated debts");
        Console.ReadKey();
        
    }
}

