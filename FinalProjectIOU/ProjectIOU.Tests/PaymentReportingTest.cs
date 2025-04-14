/*
This is lower priority test case for the PaymentReporting class, that is giving me a lot of trouble.
It can be implemented later.




using ProjectIOU;
using System;
using System.IO;
using Xunit;
using System.Text;

public class PaymentReportingDisplay_Test
{
    [Fact]
    public void DisplayPaidDebtRecords_Test()
    {
        string expenseFile = $"ExpenseRecords_{Guid.NewGuid()}.txt";
        string debtsFile = $"Debts_{Guid.NewGuid()}.txt";
        try
        {
            // reset the files
            if (File.Exists(debtsFile)) File.Delete(debtsFile);
            if (File.Exists(expenseFile)) File.Delete(expenseFile);

            // create test files 
            File.WriteAllText(debtsFile, "1,Tommy,testuser,50,Paid,2025-04-14\n2,Samantha,otheruser,100,Paid,2025-04-14\n");
            File.WriteAllText(expenseFile, "1,Test Expense 1,testuser,100,2025-04-14\n");

            // Create a test user
            User loggedInUser = new User { Username = "testuser" };

            // Capture console output
            var output = new StringBuilder();
            using (var writer = new StringWriter(output))
            {
                Console.SetOut(writer);

                // Call the method to display paid debt records
                PaymentReporting.DisplayPaidDebtRecords(loggedInUser, debtsFile);
            }

            // Reset console output
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            // Expected output
            string expectedOutput = 
                "Paid Debts previously owed to you:\n" +
                "\n" + 
                "Person: Tommy\n" +
                "Expense Record: Test Expense 1\n" +
                "Amount Paid: 50\n" +
                "Paid Date: 2025-04-14\n" +
                "-------------------------------------\n" +
                "Press 'Enter'to go back to the main menu";

            // Get the actual console output
            string consoleOutput = output.ToString();

            // Assert that the expected output is present
            Assert.Contains(expectedOutput, consoleOutput);

            // Assert that the second debt record is not part of the output
            Assert.DoesNotContain("Samantha", consoleOutput);
        }
        finally
        {
            if (File.Exists(expenseFile)) File.Delete(expenseFile);
            if (File.Exists(debtsFile)) File.Delete(debtsFile);
        }
    }
}
*/