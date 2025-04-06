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
}