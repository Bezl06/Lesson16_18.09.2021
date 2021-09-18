using System;
using Microsoft.Data.SqlClient;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            string conString = "Server=.;Database=TestDB;Trusted_Connection=true";
            using (SqlWorker sqlWorker = new SqlWorker(conString))
            {
                sqlWorker.InsertAccount(WriteAccount());
                sqlWorker.ShowAccounts();
                sqlWorker.Transfer(WriteAccount(), WriteAccount(), WriteAmount());
            }
        }
        static string WriteAccount()
        {
            System.Console.WriteLine("Enter Account(5.maxSize) : ");
            return Console.ReadLine();
        }
        static decimal WriteAmount()
        {
            System.Console.WriteLine("Enter amount transfer : ");
            return decimal.Parse(Console.ReadLine());
        }
    }
    class SqlWorker : IDisposable
    {
        private readonly SqlConnection sqlConnection;
        public SqlWorker(string conString)
        {
            sqlConnection = new SqlConnection(conString);
            sqlConnection.Open();
        }
        public void InsertAccount(string account)
        {
            string sqlQuery = "insert into Accounts(Account,Created_At) values(@account,@createdAt)";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.Parameters.AddWithValue("@account", account);
            sqlCommand.Parameters.AddWithValue("@createdAt", DateTime.Now);
            try
            {
                sqlCommand.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : {0}", ex.Message);
                sqlTransaction.Rollback();
            }
        }
        public void ShowAccounts()
        {
            string sqlQuery = "select * from Accounts";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;
            try
            {
                using (SqlDataReader sqlReader = sqlCommand.ExecuteReader())
                {
                    System.Console.WriteLine("Id\tAccount\tIsActive\tCreatedAt\tUpdatedAt");
                    while (sqlReader.Read())
                    {
                        System.Console.WriteLine($"{sqlReader.GetValue(0)}\t{sqlReader.GetValue(1)}\t{sqlReader.GetValue(2)}\t{sqlReader.GetValue(3)}\t{sqlReader.GetValue(4)}");
                    }
                    sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : {0}", ex.Message);
                sqlTransaction.Rollback();
            }
        }
        public void Transfer(string fromAcc, string toAcc, decimal amount)
        {
            int idFromAcc = GetId(fromAcc), idToAcc = GetId(toAcc);
            decimal balancFromAcc = GetBalance(fromAcc) - amount;
            string sqlQuery = "insert Transactions values(@idFromAcc,-@amount,@createdAt),(@idToAcc,@amount,@createdAt)";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.Parameters.AddWithValue("@idFromAcc", idFromAcc);
            sqlCommand.Parameters.AddWithValue("@idToAcc", idToAcc);
            sqlCommand.Parameters.AddWithValue("@amount", amount);
            sqlCommand.Parameters.AddWithValue("@createdAt", DateTime.Now);
            try
            {
                if (balancFromAcc < 0 || amount == 0)
                    throw new Exception("Not enougth money");
                sqlCommand.ExecuteNonQueryAsync();
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "update Accounts set Is_Active=@isActive,Updated_At=@updatedAt where Account=@account";
                sqlCommand.Parameters.AddWithValue("@isActive", balancFromAcc > 0 ? 1 : 0);
                sqlCommand.Parameters.AddWithValue("@account", fromAcc);
                sqlCommand.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                sqlCommand.ExecuteNonQueryAsync();
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception : {0}", ex.Message);
                sqlTransaction.Rollback();
            }
        }
        public decimal GetBalance(string account)
        {
            int id = GetId(account);
            string sqlQuery = $"select sum(Amount) from Transactions where Account_Id={id}";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            return (decimal)(sqlCommand.ExecuteScalar() ?? 0);
        }
        public int GetId(string account)
        {
            string sqlQuery = $"select min(Id) from Accounts where Account={account}";
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            return (int)sqlCommand.ExecuteScalar();
        }
        public void Dispose() => sqlConnection.Close();
    }
}
