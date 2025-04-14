using ProjectIOU;
using System;
using System.IO;
using Xunit;


public class ExpenseRecordTests
{

    [Fact]
    public void ExpenseRecordConstructor_Test() // testing that expense record constructor works
    {
        // clean up text files before starting
        string expenseFile = "ExpenseRecords.txt";
        string debtsFile = "Debts.txt";
        if (File.Exists(expenseFile)) File.Delete(expenseFile);
        if (File.Exists(debtsFile)) File.Delete(debtsFile);

        User testUser = new User{ Username = "testuser"};
        
        ExpenseRecord expenseRecord = new ExpenseRecord("Test Expense", testUser);


        Assert.Equal("Test Expense", expenseRecord.ExpenseName);
        Assert.Equal(testUser, expenseRecord.CreationUser);
        Assert.NotNull(expenseRecord.DebtsList); // debts list should be initialized
        Assert.NotNull(expenseRecord.ID); // an ID is generated
        Assert.Equal(0, expenseRecord.TotalExpenses); // total expenses should be initialized with 0
        Assert.True(expenseRecord.CreatedDate <= DateTime.Now); 
        
    }
    
    [Fact]
    public void AddDebtToExpenseRecord_Test() // testing adding a debt record to an expense record
    {
        
        string expenseFile = "ExpenseRecords.txt";
        string debtsFile = "Debts.txt";
        if (File.Exists(expenseFile)) File.Delete(expenseFile);
        if (File.Exists(debtsFile)) File.Delete(debtsFile);
        
        
        // initialize 
        User testUser = new User{Username = "testuser"};
        ExpenseRecord expenseRecord = new ExpenseRecord("Test Expense Record", testUser);
        Debt testDebt = new Debt (new Person("Tommy"), new Person("testuser"), 25, expenseRecord); // owing and owed to person
        

        expenseRecord.AddDebt(testDebt); // add the debt
        Assert.Single(expenseRecord.DebtsList); // checking htat a debt has been added to the expense list
        Assert.Equal(25, expenseRecord.TotalExpenses); // checking that the total expense amount has been updated to 25
        Assert.Contains(testDebt, expenseRecord.DebtsList); // making sure the debt has been added to debts list 

    }
    
    [Fact]
    public void AddMultipleDebtToExpenseRecord_Test() // testing the same thing as previous, but multipel debts
    {
        // clean up text files before starting
        string expenseFile = "ExpenseRecords.txt";
        string debtsFile = "Debts.txt";
        if (File.Exists(expenseFile)) File.Delete(expenseFile);
        if (File.Exists(debtsFile)) File.Delete(debtsFile);
        
        // initialize 
        User testUser = new User{Username = "testuser"};
        ExpenseRecord expenseRecord = new ExpenseRecord("Test Expense Record", testUser);
        Debt testdebt1 = new Debt (new Person("Tommy"), new Person("testuser"), 25, expenseRecord); // owing and owed to person
        Debt testdebt2 = new Debt (new Person("Samantha"), new Person("testuser"), 50, expenseRecord); // adding a second debt, new owing Person

        expenseRecord.AddDebt(testdebt1);
        expenseRecord.AddDebt(testdebt2);
        // add the debt

        Assert.Equal(75, expenseRecord.TotalExpenses); // checking that the total has been updated
        Assert.Equal(2, expenseRecord.DebtsList.Count); // checking that the second debt was added 
    }   

    [Fact]
    // deleting an expense record 
    public void DeleteExpenseRecord_Test()
    
    {
        Program.DisplayMenu = options =>
        {
            if (options.Contains("Go Back"))
            {
                return 0;
            }
            if (options.Contains("No"))
            {
                return 0;
            }
            return 0;
        };
        ExpenseRecord.ReadKey = () => new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);
        
        // clean up text files before starting
        string expenseFile = "ExpenseRecords.txt";
        string debtsFile = "Debts.txt";
        if (File.Exists(expenseFile)) File.Delete(expenseFile);
        if (File.Exists(debtsFile)) File.Delete(debtsFile);

        // create a blank debts file
        File.WriteAllText(debtsFile, "");

        User testUser = new User{Username = "testuser"};
        ExpenseRecord expenseRecord = new ExpenseRecord("Test Expense Record", testUser); // initialize a new expense recod
        
        File.AppendAllText(expenseFile, expenseRecord.ID + "," + expenseRecord.ExpenseName + "," + expenseRecord.CreationUser.Username + "," + expenseRecord.TotalExpenses + "," + expenseRecord.CreatedDate); // create the new file and add the record
        Assert.True(File.Exists(expenseFile)); // check that the file was actually created
        ExpenseRecord.DeleteExpenseRecord(testUser); // delete the expense record, but not the text file
        Assert.False(File.ReadAllLines(expenseFile).Any(line => line.Contains(expenseRecord.ID.ToString()))); // confirming that the file does not have the string version of our ID anywhere, meaning it's been deleted

        // return the CLI to default
        ExpenseRecord.ReadKey = Console.ReadKey;
    }

    
    [Fact]
    // testing that associated debts are deleted when an expense record is deleted
    public void DeleteAssociatedDebts_Test()
    {
        Program.DisplayMenu = options =>
        {
            if (options.Contains("Go Back"))
            {
                return 0;
            }
            if (options.Contains("No"))
            {
                return 0;
            }
            return 0;
        };
        // simulate pressing enter 
        ExpenseRecord.ReadKey = () => new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);

        // clean up text files before starting
        string expenseFile = "ExpenseRecords.txt";
        string debtsFile = "Debts.txt";
        if (File.Exists(expenseFile)) File.Delete(expenseFile);
        if (File.Exists(debtsFile)) File.Delete(debtsFile);
        // initialize an expense record with multiple debts

        User testUser = new User{Username = "testuser"};
        ExpenseRecord expenseRecord = new ExpenseRecord("Test Expense Record", testUser);
        // create associated debts
        Debt testdebt1 = new Debt (new Person("Tommy"), new Person("testuser"), 25, expenseRecord); // owing and owed to person
        Debt testdebt2 = new Debt (new Person("Samantha"), new Person("testuser"), 50, expenseRecord); // adding a second debt, new owing Person
        // add the debts to the expense record
        expenseRecord.AddDebt(testdebt1);
        expenseRecord.AddDebt(testdebt2);
        // save expense record to a file
        File.AppendAllText(expenseFile, expenseRecord.ID + "," + expenseRecord.ExpenseName + "," + testUser.Username + "," + expenseRecord.TotalExpenses + "," + expenseRecord.CreatedDate + "\n");
        // save the debts to a file
        File.AppendAllText(debtsFile, expenseRecord.ID + ",Tommy,testuser,25,Unpaid\n");
        File.AppendAllText(debtsFile, expenseRecord.ID + ",Samantha,testuser,50,Unpaid\n");
    
        // delete the expense record
        ExpenseRecord.DeleteExpenseRecord(testUser);
        // make sure expense record is deleted
        Assert.False(File.ReadAllLines(expenseFile).Any(line => line.Contains(expenseRecord.ID.ToString()))); // confirming that the file does not have the string version of our ID anywhere, meaning it's been deleted
        // make sure associated debts are delete
        Assert.False(File.ReadAllLines(debtsFile).Any(line => line.StartsWith(expenseRecord.ID.ToString()))); // file does not have any debts that start with the unique ID of the expense record
        // reset the CLI
        Program.DisplayMenu = Program.DefaultDisplayMenu;
        ExpenseRecord.ReadKey = Console.ReadKey;
    }
}

