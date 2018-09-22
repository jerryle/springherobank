using System;

namespace SpringHeroBank
{
    public enum TransactionType
    {
        DEPOSIT = 1,
        WITHRAW = 2,
        TRANSFER = 3,
        FAIL = 4,
        SUCCESS = 5
    }

    public class Transaction
    {
        public int Id { get; set; }
        public Account fromAccount { get; set; }
        public Account toAccount { get; set; }
        // public int fromAccountId {get;set;}
        // public int toAccountId {get;set;}
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } // type: 1 - Gửi; 2 - Rút; 3 - Chuyển
        public string issueDate { get; set; }
        // public DateTime issueDateF { get; set; }
        // private TransactionModel tm;
        // public string status {get;set;}

        public Transaction()
        {
            // tm = new TransactionModel();
        }
        public Transaction(Account fromAcc, Account toAcc, decimal amount, TransactionType type)
        {
            fromAccount = fromAcc;
            toAccount = toAcc;
            Amount = amount;
            Type = type;
        }

        public Transaction(int id, int fai, int tid, decimal amount, TransactionType type, string issuedate)
        {
            Model md = new Model();
            Id = id;
            fromAccount = md.GetAccountById(fai);
            toAccount = md.GetAccountById(tid);
            Amount = amount;
            Type = type;
            // GetTransactionTypeFromInt(type);
            issueDate = issuedate;
            // issueDateF = DateTime.Parse(issuedate);
        }

        // private void 
    }
}