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
            string debtString = ID + "," + debt.OwingPerson.Name + "," + debt.OwedAmount + "," + debt.PaidStatus;
            File.AppendAllText("Debts.txt", debtString + "\n");
        }
    }
    //mthod to create new expense records
    public static void CreateExpenseRecord(User user)
    {
        Console.WriteLine("Choose a name for your expense record: ");
        string expenseName = Console.ReadLine();
        ExpenseRecord newRecord = new ExpenseRecord(expenseName, user); // creating the new expense record and storing the creating user as the logged in user

        bool moreDebts = true; // variable to check when user is adding more debt records, will end when this is false 

        while (moreDebts)
        {
            Console.WriteLine("Enter the name of the person that owes, or 'done' when finished: ");
            string personName = Console.ReadLine();
            
            if (personName == "done")
            { 
                moreDebts = false;
            }
            else
            {
                
                Console.WriteLine("Enter the amount they owe: ");
                decimal owedAmount = Convert.ToDecimal(Console.ReadLine());
                // create a debt record and add the person name, OwedToPerson as the logged-in-user, and defaulting debts to unpaid
                Person owingPerson = new Person(personName);
                Person owedToPerson = new Person (user.Username);
                Debt newDebt = new Debt(owingPerson, owedToPerson, owedAmount, newRecord);
                newRecord.AddDebt(newDebt);
            }   
        // save the debt records to a debt file
        }
    newRecord.SaveToFile();
    Console.WriteLine("Expense record " + expenseName + "created!");
    }
}    