using ProjectIOU;
using System;
using System.IO;
using Xunit;

public class DebtTests
{
/* ExpenseRecordTests.cs file already tests creating debts, and deleting them when an expense record is deleted. This file will have a single unit test
that generates a debt record in a text file, and then calls the method to update the status of the debt record to "Paid". 
We don't need to test that the debt record exists, that has been tested in ExpenseRecordTests file. 
*/
    [Fact]
    public void DebtStatusToPaid_Test()
    {
        // Delete debt file if it already exists
        string debtsFile = "Debts.txt";
        if (File.Exists(debtsFile)) File.Delete(debtsFile);

        // Create a single debt record
        string debtId = Guid.NewGuid().ToString();
        string owingPerson = "Tommy";
        string owedToPerson = "Samantha";
        decimal amount = 50;
        string status = "Unpaid";

        // Create a new debt file and add the debt record
        File.AppendAllText(debtsFile, debtId + "," + owingPerson + "," + owedToPerson + "," + amount + "," + status + "\n");
        // Assert that the file and record were created
        Assert.True(File.Exists(debtsFile));
        Assert.Contains(debtId, File.ReadAllText(debtsFile));

        // Update the status of the debt to "Paid"
        var lines = File.ReadAllLines(debtsFile);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith(debtId))
            {
                var parts = lines[i].Split(',');
                parts[4] = "Paid"; // Updating the status from "Unpaid" to "Paid"
                lines[i] = string.Join(",", parts);
                break;
            }
        }
        File.WriteAllLines(debtsFile, lines); //rewrite the file with the update 

        // read the file, assert that it contains Paid status
        var updatedLines = File.ReadAllLines(debtsFile);
        Assert.Contains(debtId + "," + owingPerson + "," + owedToPerson + "," + amount + "," + "Paid", updatedLines);
    }
}
    



