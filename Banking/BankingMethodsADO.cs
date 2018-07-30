using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking
{
    class BankingMethodsADO
    {

        SqlConnection Connection;
        SqlCommand Statement;
        String Query;
        public BankingMethodsADO()
        {
            Connection = new SqlConnection("Data Source=TAVDESK092;initial catalog=Bank;integrated security=true");
            Query = null;
            Statement = null;
        }

        public void CreateNewAccount(String accountType, String customerName, decimal initialDeposit)
        {
            try
            {
                Query = "insert into AccountInfo(AccountType, IFSCCode, CustomerName, Balance) values(@AccountType, @IFSCCode, @CustomerName, @Balance)";
                Connection.Open();
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Statement.Parameters.AddWithValue("@AccountType", accountType);
                Statement.Parameters.AddWithValue("@IFSCCode", "CDC008");
                Statement.Parameters.AddWithValue("@CustomerName", customerName);
                Statement.Parameters.AddWithValue("@Balance", initialDeposit);
                Statement.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisplayAccountDetails()
        {
            try
            {
                Query = "select * from AccountInfo";
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Connection.Open();
                SqlDataReader ResultSet = Statement.ExecuteReader();
                while (ResultSet.Read())
                {
                    Console.WriteLine("\t\tAccount Deatils");
                    Console.WriteLine("Account Number : {0}", ResultSet.GetValue(0));
                    Console.WriteLine("Account Type : {0}", ResultSet.GetValue(1));
                    Console.WriteLine("Customer Name : {0}", ResultSet.GetValue(3));
                    Console.WriteLine("IFSC Code : {0}", ResultSet.GetValue(2));
                    Console.WriteLine("Balance : {0}\n\n", ResultSet.GetValue(4));
                }
                Connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        public void SearchByAccountNumber(long accountNumber)
        {
            try
            {
                Query = "select * from AccountInfo where AccountNumber = (@AccountNumber)";
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Statement.Parameters.AddWithValue("@AccountNumber", accountNumber);
                Connection.Open();
                SqlDataReader ResultSet = Statement.ExecuteReader();
                if (ResultSet.Read())
                {
                    Console.WriteLine("\t\tAccount Deatils");
                    Console.WriteLine("Account Number : {0}", ResultSet.GetValue(0));
                    Console.WriteLine("Account Type : {0}", ResultSet.GetValue(1));
                    Console.WriteLine("Customer Name : {0}", ResultSet.GetValue(3));
                    Console.WriteLine("IFSC Code : {0}", ResultSet.GetValue(2));
                    Console.WriteLine("Balance : {0}\n\n", ResultSet.GetValue(4));
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Deposit(long accountNumber, decimal depositAmount)
        {
            decimal balance = 0m;
            try
            {
                Query = "select Balance from AccountInfo where AccountNumber = (@AccountNumber)";
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Statement.Parameters.AddWithValue("@AccountNumber", accountNumber);
                Connection.Open();
                SqlDataReader ResultSet = Statement.ExecuteReader();
                if (ResultSet.Read())
                    balance = depositAmount + Convert.ToDecimal(ResultSet.GetValue(0));
                ResultSet.Close();
                Query = "update AccountInfo set Balance = (@Balance) where AccountNumber = (@AccountNumber)";
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Statement.Parameters.AddWithValue("@AccountNumber", accountNumber);
                Statement.Parameters.AddWithValue("@Balance", balance);
                Statement.ExecuteNonQuery();
                Console.WriteLine("\t\tUpdated Balance : {0}", balance);
                Connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Withdraw(long accountNumber, decimal withdrawAmount)
        {
            Console.WriteLine("ADO------");
            decimal balance = 0m, minimumBalance = 0m;
            String accountType = "";
            try
            {
                Query = "select Balance, AccountType from AccountInfo where AccountNumber = (@AccountNumber)";
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Statement.Parameters.AddWithValue("@AccountNumber", accountNumber);
                Connection.Open();
                SqlDataReader ResultSet = Statement.ExecuteReader();
                if (ResultSet.Read())
                {
                    balance = Convert.ToDecimal(ResultSet.GetValue(0)) - withdrawAmount;
                    accountType = Convert.ToString(ResultSet.GetValue(1));
                }
                ResultSet.Close();
                if (String.Equals("Savings", accountType.Trim()))
                {
                    minimumBalance = 1000m;
                }
                else if (String.Equals("Current", accountType.Trim()))
                {
                    minimumBalance = 0m;
                }
                else if (String.Equals("DMAT", accountType.Trim()))
                {
                    minimumBalance = -10000m;
                }

                if (balance >= minimumBalance)
                {
                    Query = "update AccountInfo set Balance = (@Balance) where AccountNumber = (@AccountNumber)";
                    Statement = new SqlCommand(Query, Connection);
                    Statement.CommandType = CommandType.Text;
                    Statement.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    Statement.Parameters.AddWithValue("@Balance", balance);
                    Statement.ExecuteNonQuery();
                    Console.WriteLine("\t\tUpdated Balance : {0}", balance);
                }
                else
                {
                    Console.WriteLine("You cannot withdraw entered amount, it goes under Minimum Balance.., Try again with less amount in next transaction..\n\n");
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void CalculateInterest(long accountNumber)
        {
            decimal balance = 0m;
            String accountType = "";
            try
            {
                Query = "select Balance, AccountType from AccountInfo where AccountNumber = (@AccountNumber)";
                Statement = new SqlCommand(Query, Connection);
                Statement.CommandType = CommandType.Text;
                Statement.Parameters.AddWithValue("@AccountNumber", accountNumber);
                Connection.Open();
                SqlDataReader ResultSet = Statement.ExecuteReader();
                if (ResultSet.Read())
                {
                    balance = Convert.ToDecimal(ResultSet.GetValue(0));
                    accountType = Convert.ToString(ResultSet.GetValue(1));
                }
                if (String.Equals("Savings", accountType.Trim()))
                {
                    Console.WriteLine("\t\tInterest : " + (balance * 4) / 100 + "\n\n");
                }
                else if (String.Equals("Current", accountType.Trim()))
                {
                    Console.WriteLine("\t\tInterest : " + balance / 100 + "\n\n");
                }
                else
                {
                    Console.WriteLine("\n\t\tThere is no interest for DMAT Accounts\n\n");
                }
                ResultSet.Close();
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
