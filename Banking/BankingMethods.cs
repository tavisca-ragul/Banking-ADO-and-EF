using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking
{
    class BankingMethods 
    {
        BankEntities BankEntity;
        List<AccountInfo> list;

        public BankingMethods()
        {
            BankEntity = new BankEntities();
            list = new List<AccountInfo>();
        }
        public void CreateNewAccount(String accountType, String customerName, decimal balance)
        {
            var accountInfo = new AccountInfo()
            {
                AccountType = accountType,
                IFSCCode = "CDC008",
                CustomerName = customerName,
                Balance = balance
            };
            BankEntity.AccountInfoes.Add(accountInfo);
            BankEntity.SaveChanges();
        }

        public void DisplayAccountDetails()
        {
            list = BankEntity.AccountInfoes.ToList();
            foreach(AccountInfo detail in list)
            {
                Console.WriteLine("\t\tAccount Deatils");
                Console.WriteLine("Account Number : {0}", detail.AccountNumber);
                Console.WriteLine("Account Type : {0}", detail.AccountType);
                Console.WriteLine("Customer Name : {0}", detail.CustomerName);
                Console.WriteLine("IFSC Code : {0}", detail.IFSCCode);
                Console.WriteLine("Balance : {0}\n\n", detail.Balance);
            }
        }

        public void SearchByAccountNumber(long accountNumber)
        {
            try
            {
                Console.WriteLine("\t\tAccount Deatils");
                Console.WriteLine("Account Number : {0}", BankEntity.AccountInfoes.Find(accountNumber).AccountNumber);
                Console.WriteLine("Account Type : {0}", BankEntity.AccountInfoes.Find(accountNumber).AccountType);
                Console.WriteLine("Customer Name : {0}", BankEntity.AccountInfoes.Find(accountNumber).CustomerName);
                Console.WriteLine("IFSC Code : {0}", BankEntity.AccountInfoes.Find(accountNumber).IFSCCode);
                Console.WriteLine("Balance : {0}\n\n", BankEntity.AccountInfoes.Find(accountNumber).Balance);
            } catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        public void Deposit(long accountNumber, decimal depositAmount)
        {
            try
            {
                BankEntity.AccountInfoes.Find(accountNumber).Balance = BankEntity.AccountInfoes.Find(accountNumber).Balance + depositAmount;
                BankEntity.SaveChanges();
                Console.WriteLine("\t\tUpdated Balance : {0}\n\n", BankEntity.AccountInfoes.Find(accountNumber).Balance);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Withdraw(long accountNumber, decimal withdrawAmount)
        {
            try
            {
                var balance = BankEntity.AccountInfoes.Find(accountNumber).Balance;
                var accountType = BankEntity.AccountInfoes.Find(accountNumber).AccountType;
                var minimumBalance = 0;
                if (String.Equals("Savings", accountType.Trim()))
                {
                    minimumBalance = 1000;
                }
                else if (String.Equals("Current", accountType.Trim()))
                {
                    minimumBalance = 0;
                }
                else if (String.Equals("DMAT", accountType.Trim()))
                {
                    minimumBalance = -10000;
                }

                if (balance - withdrawAmount >= minimumBalance)
                {
                    BankEntity.AccountInfoes.Find(accountNumber).Balance = balance - withdrawAmount;
                    Console.WriteLine("\t\tUpdated Balance : " + BankEntity.AccountInfoes.Find(accountNumber).Balance + "\n\n");
                    BankEntity.SaveChanges();
                }
                else
                {
                    Console.WriteLine("You cannot withdraw entered amount, it goes under Minimum Balance.., Try again with less amount in next transaction..\n\n");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
        
        public void CalculateInterest(long accountNumber)
        {
            var accountType = BankEntity.AccountInfoes.Find(accountNumber).AccountType;
            if (String.Equals("Savings", accountType.Trim()))
            {
                Console.WriteLine("\t\tInterest : " + (BankEntity.AccountInfoes.Find(accountNumber).Balance * 4) / 100 + "\n\n");
            }
            else if (String.Equals("Current", accountType.Trim()))
            {
                Console.WriteLine("\t\tInterest : " + BankEntity.AccountInfoes.Find(accountNumber).Balance / 100 + "\n\n");
            }
            else
            {
                Console.WriteLine("\n\t\tThere is no interest for DMAT Accounts\n\n");
            }
        }
    }
}