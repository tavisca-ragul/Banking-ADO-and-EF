using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.ADO.NET\n2.Entity Framework\nEnter your choice:");
            int frameType = Convert.ToInt32(Console.ReadLine());
            var BankingMethodsADO = new BankingMethodsADO();
            var BankingMethods = new BankingMethods();
            String accountType, customerName;
            long accountNumber;
            decimal depositAmount, withdrawAmount, initialDeposit;
            userInput:
            Console.Write("1.New Account\n2.Display Account Details\n3.Search by Account No\n4.Deposit\n5.Withdraw\n6.Calculate Interest\n7.Exit\nEnter your choice:");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.Write("Account Type: ");
                    accountType = Console.ReadLine();
                    Console.Write("Customer Name: ");
                    customerName = Console.ReadLine();
                    Console.Write("Initial Deposit: ");
                    initialDeposit = Convert.ToDecimal(Console.ReadLine());
                    if(frameType == 1)
                        BankingMethodsADO.CreateNewAccount(accountType, customerName, initialDeposit);
                    else
                        BankingMethods.CreateNewAccount(accountType, customerName, initialDeposit);
                    Console.WriteLine("Account is Craeted");
                    goto userInput;
                case 2:
                    if (frameType == 1)
                        BankingMethodsADO.DisplayAccountDetails();
                    else
                        BankingMethods.DisplayAccountDetails();
                    goto userInput;
                case 3:
                    Console.Write("Account Number: ");
                    accountNumber = Convert.ToInt64(Console.ReadLine());
                    if (frameType == 1)
                        BankingMethodsADO.SearchByAccountNumber(accountNumber);
                    else
                        BankingMethods.SearchByAccountNumber(accountNumber);
                    goto userInput;
                case 4:
                    Console.Write("Account Number: ");
                    accountNumber = Convert.ToInt64(Console.ReadLine());
                    Console.Write("Deposit Amount: ");
                    depositAmount = Convert.ToDecimal(Console.ReadLine());
                    if (frameType == 1)
                        BankingMethodsADO.Deposit(accountNumber, depositAmount);
                    else
                        BankingMethods.Deposit(accountNumber, depositAmount);
                    goto userInput;
                case 5:
                    Console.Write("Account Number: ");
                    accountNumber = Convert.ToInt64(Console.ReadLine());
                    Console.Write("Withdraw Amount: ");
                    withdrawAmount = Convert.ToDecimal(Console.ReadLine());
                    if (frameType == 1)
                        BankingMethodsADO.Withdraw(accountNumber, withdrawAmount);
                    else
                        BankingMethods.Withdraw(accountNumber, withdrawAmount);
                    goto userInput;
                case 6:
                    Console.Write("Account Number: ");
                    accountNumber = Convert.ToInt64(Console.ReadLine());
                    if (frameType == 1)
                        BankingMethodsADO.CalculateInterest(accountNumber);
                    else
                        BankingMethods.CalculateInterest(accountNumber);
                    goto userInput;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid Choice, try again");
                    goto userInput;
            }
            Console.ReadKey();
        }
    }
}
