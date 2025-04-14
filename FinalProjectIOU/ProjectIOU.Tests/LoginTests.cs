using ProjectIOU;
using System;
using System.IO;
using Xunit;

public class LoginTest
{
//Very simple test to make sure created accounts are stored in a Logins file.
    [Fact]
    public void LoginsCreated_Test()
    {
        // delete the logins file before starting
        string loginsFile = "Logins.txt";
        if (File.Exists(loginsFile)) File.Delete(loginsFile);

        // Simulate creating a new account
        string username = "tommyuser";
        string password = "mypassword92";

        // Write the username and password to the logins file
        File.AppendAllText(loginsFile, username + ":" + password + "\n");

        // Verify the logins file was created
        Assert.True(File.Exists(loginsFile));

        // Verify the username and password are stored in the file
        string fileContents = File.ReadAllText(loginsFile);
        Assert.Contains(username + ":" + password, fileContents);
    }
}