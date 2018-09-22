using System;

namespace SpringHeroBank
{
    public enum AccountStatus {
        ACTIVE = 1,
        DISABLED = 2
    }
    public class Account
    {
        public int Id {get;set;}
        public string Username {get;set;}
        public string Email {get;set;}
        public string Name {get;set;}
        public string Password {get;set;}
        public decimal Money {get;set;}
        public bool isLogged {get;set;}
        public string Salt {get;set;}
        private AccountStatus Status {get;set;}

        public Account() {
            defaultLoginStatus();
            GenerateSalt();
        }

        public Account(string username, string name, string email, string password, decimal money) {
            Username = username;
            Email = email;
            Name = name;
            Password = password;
            Money = money;
            defaultLoginStatus();
            GenerateSalt();
        }

        public Account(string username, string name, string email, string password, decimal money, string salt) {
            Username = username;
            Email = email;
            Name = name;
            Password = password;
            Money = money;
            defaultLoginStatus();
            // GenerateSalt();
            Salt = salt;
        }

        public Account(int id, string username, string name, string email, string password, decimal money, string salt, AccountStatus status) {
            Id = id;
            Username = username;
            Email = email;
            Name = name;
            Password = password;
            Money = money;
            defaultLoginStatus();
            // GenerateSalt();
            Salt = salt;
            Status = status;
            // getAccountStatus(status);
        }

        private void GenerateSalt()
        {
            Salt = Guid.NewGuid().ToString().Substring(1, 7);
        }

        private void defaultLoginStatus() {
            isLogged = false;
        }
    }
}
