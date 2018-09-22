using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace SpringHeroBank
{
    public class Model
    {
        private MySqlConnection conn;


        public Model()
        {
            DBInit();
        }

        private void DBInit()
        {
            var connectionString = "Server=localhost;Database=springherobank;Uid=root;Pwd=;SslMode=None;";
            conn = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }


        public bool CheckAccountExists(string username)
        {
            var _exists = false;
            if (this.OpenConnection())
            {
                var sqlQuery = "select id from accounts where username = @uname";

                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                mySqlCommand.Parameters.AddWithValue("@uname", username);
                var dR = mySqlCommand.ExecuteReader();

                while (dR.Read())
                {
                    _exists = !_exists;
                }

                dR.Close();
                this.CloseConnection();
                // mySqlCommand.executes
            }
            return _exists;
        }

        public bool CheckEmailExists(string email)
        {
            var _exists = false;
            if (this.OpenConnection())
            {
                var sqlQuery = "select id from accounts where email = @uemail";

                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                mySqlCommand.Parameters.AddWithValue("@uemail", email);
                var dR = mySqlCommand.ExecuteReader();

                while (dR.Read())
                {
                    _exists = !_exists;
                }

                dR.Close();
                this.CloseConnection();
            }
            return _exists;
        }
        public bool AccountRegister(Account a)
        {

            if (this.OpenConnection())
            {
                var sqlQuery = "insert into accounts (username, `name`, email, `password`,`salt`) values (@username, @name, @email, @password, @salt)";

                Utility ulti = new Utility();
                var PwHashed = ulti.EncryptedString(a.Password, a.Salt);

                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                mySqlCommand.Parameters.AddWithValue("@username", a.Username);
                mySqlCommand.Parameters.AddWithValue("@name", a.Name);
                mySqlCommand.Parameters.AddWithValue("@email", a.Email);
                mySqlCommand.Parameters.AddWithValue("@password", PwHashed);
                mySqlCommand.Parameters.AddWithValue("@salt", a.Salt);
                var result = mySqlCommand.ExecuteNonQuery();

                this.CloseConnection();
                if (result == 1) return true;
                // mySqlCommand.executes
            }
            return false;
        }

        // private bool AccountLogin(Account a) {

        //     if(this.OpenConnection()) {
        //         var sqlQuery = "select into accounts (username, name, email, password) values (@rollNum, @name, @email, @password)";

        //         MySqlCommand mySqlCommand = new MySqlCommand(sqlQuery, conn);
        //         mySqlCommand.Parameters.AddWithValue("@username", a.Username);
        //         mySqlCommand.Parameters.AddWithValue("@name", a.Name);
        //         mySqlCommand.Parameters.AddWithValue("@email", a.Email);
        //         mySqlCommand.Parameters.AddWithValue("@password", a.Password);
        //         mySqlCommand.ExecuteNonQuery();
        //         // mySqlCommand.execute 
        //         return true;
        //     }
        //     return false;
        // }

        public Account GetAccountByName(string username)
        {

            if (this.OpenConnection())
            {
                // List<Account> listAccount = new List<Account>();
                Account rA = null;
                var mySqlCommand = new MySqlCommand("select * from accounts where username = @uname;", this.conn);
                mySqlCommand.Parameters.AddWithValue("@uname", username);
                // mySqlCommand.Parameters.AddWithValue("@upass", password);
                var dR = mySqlCommand.ExecuteReader();
                if (dR.Read())
                {
                    rA = new Account(dR.GetInt32("id"), dR.GetString("username"), dR.GetString("name"), dR.GetString("email"), dR.GetString("password"), dR.GetDecimal("money"), dR.GetString("salt"), (AccountStatus) dR.GetInt32("status"));
                    // listAccount.Add();
                }
                dR.Close();
                this.CloseConnection();
                if (rA != null)
                {
                    return rA;
                }
            }
            return null;
        }

        public Account GetAccountById(int id)
        {

            if (this.OpenConnection())
            {
                // List<Account> listAccount = new List<Account>();
                Account rA = null;
                var mySqlCommand = new MySqlCommand("select * from accounts where id = @uid;", this.conn);
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                // mySqlCommand.Parameters.AddWithValue("@upass", password);
                var dR = mySqlCommand.ExecuteReader();
                if (dR.Read())
                {
                    rA = new Account(dR.GetInt32("id"), dR.GetString("username"), dR.GetString("name"), dR.GetString("email"), dR.GetString("password"), dR.GetDecimal("money"), dR.GetString("salt"),(AccountStatus) dR.GetInt32("status"));
                    // listAccount.Add();
                }
                dR.Close();
                this.CloseConnection();
                if (rA != null)
                {
                    return rA;
                }
            }
            return null;
        }

        public List<Account> TransferTo(string searchKey, int type)
        {
            List<Account> listAccount = new List<Account>();
            if (this.OpenConnection())
            {
                var sqlQuery = "";
                MySqlCommand mySqlCommand;
                switch (type)
                {
                    case 0:
                        {
                            sqlQuery = "select * from accounts where username = @search";

                            break;
                        }
                    case 1:
                        {
                            sqlQuery = "select * from accounts where name like @search";

                            break;
                        }
                }

                mySqlCommand = new MySqlCommand(sqlQuery, this.conn);

                if (type == 0)
                {
                    // mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                    mySqlCommand.Parameters.AddWithValue("@search", searchKey);
                }
                else if (type == 1)
                {
                    // mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                    mySqlCommand.Parameters.AddWithValue("@search", "%" + searchKey + "%");
                }

                // mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                // mySqlCommand.Parameters.AddWithValue("@search", searchKey);

                var dR = mySqlCommand.ExecuteReader();

                while (dR.Read())
                {
                    listAccount.Add(new Account(dR.GetInt32("id"), dR.GetString("username"), dR.GetString("name"), dR.GetString("email"), dR.GetString("password"), dR.GetDecimal("money"), dR.GetString("salt"),(AccountStatus) dR.GetInt32("status")));
                }
                dR.Close();
                // dR.Close();
                this.CloseConnection();
                if (listAccount.Count > 0)
                {
                    return listAccount;
                }

            }
            return null;
        }

        public TransactionType AccountTransaction(Transaction t)
        {
            TransactionType tt = TransactionType.SUCCESS;
            // var _success = false;
            if (this.OpenConnection())
            {
                // a.Money += money;

                MySqlTransaction myTrans = this.conn.BeginTransaction(IsolationLevel.Serializable);
                MySqlDataReader eR;
                // var _validatemoney = true;
                // var _passmoney = false;

                // myTrans = 
                try
                {
                    // var _validatemoney = true;
                    var sqlQuery = "";
       

                    MySqlCommand mySqlCommand;
                    var resultCount = 0;

                    if (t.Type == TransactionType.DEPOSIT || t.Type == TransactionType.WITHRAW)
                    {
                        sqlQuery = "select money from accounts where id=@uid;";
                        mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        mySqlCommand.Parameters.AddWithValue("@uid", t.fromAccount.Id);
                        eR = mySqlCommand.ExecuteReader();
                        if (eR.Read())
                        {
                            t.fromAccount.Money = eR.GetDecimal("money");
                            // Console.WriteLine("Số tiền get được: {0}", t.fromAccount.Money);
                        }
                        eR.Close();

                        if (t.Type == TransactionType.WITHRAW && t.fromAccount.Money < t.Amount)
                        {
                            throw new Exception("Số dư trong tài khoản không đủ để giao dịch.");   
                        }


                        if (t.Type == TransactionType.DEPOSIT)
                        {
                            t.fromAccount.Money += t.Amount;
                        }
                        else if (t.Type == TransactionType.WITHRAW)
                        {
                            t.fromAccount.Money -= t.Amount;
                        }

                        // Console.WriteLine("Số tiền dự kiến sau giao dịch: {0}", t.fromAccount.Money);

                        if (t.Type == TransactionType.WITHRAW) sqlQuery = "update accounts set money=money-@moneyy where id=@uid";
                        else sqlQuery = "update accounts set money=money+@moneyy where id=@uid";

                        mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        // mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.AddWithValue("@uid", t.fromAccount.Id);
                        mySqlCommand.Parameters.AddWithValue("@moneyy", t.Amount);

                        resultCount = mySqlCommand.ExecuteNonQuery();

                        // if(money < 0) t = new Transaction(a, null, money, 2);
                        // else t = new Transaction(a, null, money, 1);


                    }
                    else if (t.Type == TransactionType.TRANSFER)
                    {
                        // test bug money
                        // sqlQuery = "update accounts set money=0 where id=@id";

                        // mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        // // mySqlCommand.Parameters.Clear();
                        // mySqlCommand.Parameters.AddWithValue("@id", t.fromAccount.Id);
                        // // mySqlCommand.Parameters.AddWithValue("@moneyy", t.Amount);
                        // mySqlCommand.ExecuteNonQuery();

                        sqlQuery = "select money from accounts where id=@uid";
                        mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        // mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.AddWithValue("@uid", t.fromAccount.Id);
                        eR = mySqlCommand.ExecuteReader();
                        if (eR.Read())
                        {
                            t.fromAccount.Money = eR.GetDecimal("money");
                        }
                        eR.Close();

                        if (t.fromAccount.Money < t.Amount) {
                            throw new Exception("Số dư trong tài khoản không đủ để giao dịch.");   
                            // _validatemoney = false;
                        }

                        t.fromAccount.Money -= t.Amount;

                        sqlQuery = "select money from accounts where id=@uid";
                        mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        // mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.AddWithValue("@uid", t.toAccount.Id);
                        eR = mySqlCommand.ExecuteReader();
                        if (eR.Read())
                        {
                            t.toAccount.Money = eR.GetDecimal("money");
                        }
                        eR.Close();
                        t.toAccount.Money += t.Amount;


                        // update tiền sau khi +- vân vân
                        sqlQuery = "update accounts set money=money-@moneyy where id=@id";

                        mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        // mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.AddWithValue("@id", t.fromAccount.Id);
                        mySqlCommand.Parameters.AddWithValue("@moneyy", t.Amount);
                        mySqlCommand.ExecuteNonQuery();
                        // mySqlCommand.Parameters.Add()

                        sqlQuery = "update accounts set money=money+@moneyy where id=@id";
                        mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                        // mySqlCommand.Parameters.Clear();
                        mySqlCommand.Parameters.AddWithValue("@id", t.toAccount.Id);
                        mySqlCommand.Parameters.AddWithValue("@moneyy", t.Amount);
                        resultCount = mySqlCommand.ExecuteNonQuery();

                        // Transaction t = new Transaction(a, a2, money, 3);

                    }
                    sqlQuery = "insert into transactions (fromAccount, toAccount, value, type) values (@fAcc, @tAcc, @valuee, @typee)";
                    // mySqlCommand = new MySqlCommand(sqlQuery, conn);
                    mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                    // mySqlCommand.Parameters.Clear();
                    mySqlCommand.Parameters.AddWithValue("@fAcc", t.fromAccount.Id);
                    mySqlCommand.Parameters.AddWithValue("@tAcc", t.toAccount.Id);
                    mySqlCommand.Parameters.AddWithValue("@valuee", t.Amount);
                    mySqlCommand.Parameters.AddWithValue("@typee", t.Type);
                    var transactionResult = mySqlCommand.ExecuteNonQuery();

                    if (transactionResult != 1 || resultCount != 1)
                    {
                        throw new Exception("Không thể thêm giao dịch hoặc update tài khoản.");
                    }

                    myTrans.Commit();
                    return tt;

                    // }
                }
                catch (Exception e)
                {
                    try
                    {
                        myTrans.Rollback();
                    }
                    catch (MySqlException ex)
                    {
                        if (myTrans.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() + " was encountered while attempting to roll back the transaction.");
                        }
                    }
                    // Console.WriteLine("An exception of type " + e.GetType() + " was encountered while inserting the data.");
                    // Console.WriteLine("Neither record was written to database.");
                    Console.WriteLine(e.ToString());
                    Console.WriteLine("Giao dịch chưa được thực hiện vì không đáp ứng đủ yêu cầu!");
                    tt = TransactionType.FAIL;
                    return tt;
                }
                finally
                {
                    this.CloseConnection();
                    // Console.Clear();
                    // Console.Out.Flush();
                    Console.WriteLine("Lệnh giao dịch đã được đóng!");
                    // else Console.WriteLine("Số dư trong tài khoản không hợp lệ! (Invalid account money while beginning transaction)");
                }

            }
            return tt;
        }

        public List<Transaction> GetTransactionHistory5Days(int id)
        {
            if (this.OpenConnection())
            {

                // var _validtype = false;
                List<Transaction> rL = new List<Transaction>();

                var sqlQuery = "select * from transactions where fromAccount=@uid or toAccount=@uid and issueDate > date_sub(CURDATE(), INTERVAL 5 DAY) order by issueDate;";
                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                // mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                var eR = mySqlCommand.ExecuteReader();
                while (eR.Read())
                {
                    rL.Add(new Transaction(eR.GetInt32("id"), eR.GetInt32("fromAccount"), eR.GetInt32("toAccount"), eR.GetInt32("value"),(TransactionType) eR.GetInt32("type"), eR.GetString("issueDate")));
                }
                eR.Close();

                this.CloseConnection();
                if (rL.Count > 0)
                {
                    return rL;
                }
            }
            return null;
        }

        public List<Transaction> GetTransactionHistoryFromTimeToTime(int id, string begintime, string endtime)
        {
            if (this.OpenConnection())
            {

                // var _validtype = false;
                List<Transaction> rL = new List<Transaction>();

                var sqlQuery = "select * from transactions where (fromAccount=@uid or toAccount=@uid) and issueDate between @btime and @etime order by issueDate;";
                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                // mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                mySqlCommand.Parameters.AddWithValue("@btime", begintime);
                mySqlCommand.Parameters.AddWithValue("@etime", endtime);
                // else mySqlCommand.Parameters.AddWithValue("@etime", "curdate()");
                var eR = mySqlCommand.ExecuteReader();
                while (eR.Read())
                {
                    rL.Add(new Transaction(eR.GetInt32("id"), eR.GetInt32("fromAccount"), eR.GetInt32("toAccount"), eR.GetInt32("value"), (TransactionType) eR.GetInt32("type"), eR.GetString("issueDate")));
                }
                eR.Close();

                this.CloseConnection();
                if (rL.Count > 0)
                {
                    return rL;
                }
            }
            return null;
        }

        public List<Transaction> GetTransactionHistory10Newest(int id)
        {
            if (this.OpenConnection())
            {

                // var _validtype = false;
                List<Transaction> rL = new List<Transaction>();

                var sqlQuery = "select * from transactions where fromAccount=@uid or toAccount=@uid order by issueDate limit 10;";
                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                // mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                var eR = mySqlCommand.ExecuteReader();
                while (eR.Read())
                {
                    rL.Add(new Transaction(eR.GetInt32("id"), eR.GetInt32("fromAccount"), eR.GetInt32("toAccount"), eR.GetInt32("value"), (TransactionType) eR.GetInt32("type"), eR.GetString("issueDate")));
                }
                eR.Close();

                this.CloseConnection();
                if (rL.Count > 0)
                {
                    return rL;
                }
            }
            return null;
        }

        public List<Transaction> GetTransactionHistoryWith(int id, int tid)
        {
            if (this.OpenConnection())
            {

                // var _validtype = false;
                List<Transaction> rL = new List<Transaction>();

                var sqlQuery = "select * from transactions where (fromAccount=@uid and toAccount=@tid) or (fromAccount=@tid and toAccount=@uid) order by issueDate limit 20;";
                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                // mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                mySqlCommand.Parameters.AddWithValue("@tid", tid);
                var eR = mySqlCommand.ExecuteReader();
                while (eR.Read())
                {
                    rL.Add(new Transaction(eR.GetInt32("id"), eR.GetInt32("fromAccount"), eR.GetInt32("toAccount"), eR.GetInt32("value"), (TransactionType) eR.GetInt32("type"), eR.GetString("issueDate")));
                }
                eR.Close();

                this.CloseConnection();
                if (rL.Count > 0)
                {
                    return rL;
                }
            }
            return null;
        }

        public List<int> GetListAccountID_TransactionWithAccountID(int id)
        {
            List<int> listAccountID = new List<int>();
            if (this.OpenConnection())
            {
                var sqlQuery = "";
                MySqlCommand mySqlCommand;
                MySqlDataReader dR;
                sqlQuery = "select toAccount from transactions where fromAccount = @uid group by toAccount order by toAccount;";
                mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                dR = mySqlCommand.ExecuteReader();

                while (dR.Read())
                {
                    listAccountID.Add(dR.GetInt32("toAccount"));
                }
                dR.Close();

                sqlQuery = "select fromAccount from transactions where toAccount = @uid group by fromAccount order by fromAccount;";
                mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                // mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@uid", id);
                dR = mySqlCommand.ExecuteReader();

                while (dR.Read())
                {
                    if (!listAccountID.Contains(dR.GetInt32("fromAccount"))) listAccountID.Add(dR.GetInt32("fromAccount"));
                }
                dR.Close();
                // dR.Close();
                this.CloseConnection();
                if (listAccountID.Count > 0)
                {
                    return listAccountID;
                }

            }
            return null;
        }

        public void ResyncAccountMoney(Account a)
        {
            if (this.OpenConnection())
            {
                var sqlQuery = "select * from accounts where id=@uid;";
                // mySqlCommand = new MySqlCommand(sqlQuery, conn);
                var mySqlCommand = new MySqlCommand(sqlQuery, this.conn);
                mySqlCommand.Parameters.AddWithValue("@uid", a.Id);

                var dR = mySqlCommand.ExecuteReader();
                if (dR.Read())
                {
                    a.Money = dR.GetDecimal("money");
                }
                dR.Close();
                this.CloseConnection();
            }
        }

    }
}