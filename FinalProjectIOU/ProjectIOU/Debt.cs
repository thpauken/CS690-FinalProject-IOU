namespace ProjectIOU;

using System.IO;
// instantiating the Debt class properties
public class Debt
{
   public Guid ID { get; set; }
   public Person OwingPerson { get; set; }
   public Person OwedToPerson { get; set; }
   public decimal OwedAmount { get; set; }
   public PaidStatus PaidStatus { get; set; }
   public DateTime PaidDate { get; set; }

// defining the constructor
// defining how default values are set like PaidStatus set to Unpaid, and PaidDate default value until it is later updated when PaidStatus is Paid
   public Debt(Person owingPerson, Person owedToPerswon, decimal owedAmount)
   {
    ID = Guid.NewGuid();
    OwingPerson = owingPerson;
    OwedToPerson = owedToPerson;
    OwedAmount = owedAmount;
    PaidStatus = PaidStatus.Unpaid;
    PaidDate = DateTime.MinValue;
   }
}