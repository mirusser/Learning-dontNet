
//Single Responsibility Principle
//A class should have only one reason to change/only one responsibility.

namespace SOLID._1_SRP;

internal class BankAccount
{
    // This class has only one responsibility:
    // to manage the balance of a bank account.
    private decimal balance;

    public decimal GetBalance()
    {
        return balance;
    }

    public void Deposit(decimal amount)
    {
        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        balance -= amount;
    }
}