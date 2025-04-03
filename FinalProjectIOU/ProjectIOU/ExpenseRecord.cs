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
}