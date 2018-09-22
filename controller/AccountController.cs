using System;
using System.Collections.Generic;

namespace SpringHeroBank
{
    public class AccountController
    {
        public Account SignIn()
        {
            Console.Clear();
            Console.Out.Flush();
            Console.WriteLine("\n> Vui lòng nhập đủ các thông tin phía dưới!");
            Console.WriteLine("Tên tài khoản: ");
            var username = Console.ReadLine().ToLower();
            Console.WriteLine("Mật khẩu: ");
            Utility ulti = new Utility();
            var password = ulti.EnterPassword();

            Console.WriteLine();

            // nếu thông tin đăng nhập đúng
            Model md = new Model();
            Account rA = md.GetAccountByName(username);
            if (rA != null && ulti.EncryptedString(password, rA.Salt) == rA.Password)
            {
                Console.WriteLine("Đăng nhập thành công! Ấn ENTER để quay trở về...");
                Console.ReadLine();
                return rA;
            }
            Console.WriteLine("Thông tin đăng nhập không chính xác! Ấn ENTER để quay trở về...");
            Console.ReadLine();
            return null;
        }

        public bool ValidateRegister(string username, string name, string email, string password, string cfm_password)
        {
            if (password != cfm_password)
            {
                Console.WriteLine("Mật khẩu nhập lại không trùng khớp!");
                return false;
            }
            if (username.Length < 4 || username.Length > 23)
            {
                Console.WriteLine("Tên tài khoản không được nhỏ hơn 4 hoặc lớn hơn 23 kí tự.");
                return false;
            }
            if (name.Length < 6 || name.Length >= 50)
            {
                Console.WriteLine("Tên chủ khoản quá ngắn hoặc quá dài!");
                return false;
            }
            if (email.Length < 13 || email.Length > 50)
            {
                Console.WriteLine("Vui lòng không nhập email ngắn hơn 13 kí tự hoặc dài hơn 50 kí tự!");
                return false;
            }
            if (!email.EndsWith("@gmail.com"))
            {
                Console.WriteLine("Vui lòng sử dụng gmail để xác nhận tài khoản!");
                return false;
            }
            if (password.Length < 6)
            {
                Console.WriteLine("Mật khẩu không được ngắn hơn 6 kí tự.");
                return false;
            }
            if (password.Contains(" "))
            {
                Console.WriteLine("Mật khẩu không được chứa kí tự khoảng trắng.");
                return false;
            }
            return true;
        }
        public void SignUp()
        {
            Console.Clear();
            Console.Out.Flush();
            while (true)
            {
                Console.WriteLine("\n> Vui lòng nhập đủ các thông tin phía dưới!");
                Console.WriteLine("Tên tài khoản: ");
                var username = Console.ReadLine().ToLower();
                Console.WriteLine("Tên chủ khoản: ");
                var name = Console.ReadLine().ToUpper();
                Console.WriteLine("Email: ");
                var email = Console.ReadLine().ToLower();
                Console.WriteLine("Mật khẩu: ");
                Utility ulti = new Utility();
                var password = ulti.EnterPassword();
                Console.WriteLine();
                Console.WriteLine("Nhập lại mật khẩu: ");
                var cfm_password = ulti.EnterPassword();
                Console.WriteLine();

                Console.WriteLine("Bạn muốn tạo tài khoản với thông tin vừa nhập? (y/n):");
                var _continue = Console.ReadLine().ToLower();
                if (_continue == "y")
                {
                    Console.Clear();
                    Console.Out.Flush();
                    if (ValidateRegister(username, name, email, password, cfm_password))
                    {
                        Model md = new Model();
                        if (md.CheckAccountExists(username))
                        {
                            Console.WriteLine("Tài khoản đã tồn tại! Hãy thử lại sau");

                        }
                        else if (md.CheckEmailExists(email))
                        {
                            Console.WriteLine("Email này đã tồn tại trong hệ thống! Vui lòng nhập email khác.");
                        }
                        else
                        {
                            // success
                            // Utility ulti = new Utility();
                            // var PwHashed = ulti.EncryptedString(password);
                            if (md.AccountRegister(new Account(username, name, email, password, 0)))
                            {
                                Console.WriteLine("Tài khoản đã được đăng ký thành công!");
                            }
                            else
                            {
                                Console.WriteLine("Đăng ký tài khoản thất bại! Hãy thử lại sau.");
                            }
                        }
                        break;
                    }
                }
            }

            Console.WriteLine("Ấn ENTER để tiếp tục...");
            Console.ReadLine();
        }

        public Account ValidateWhereAccountTransferTo(string content)
        {

            Model md = new Model();
            // Account rA = new Account();
            List<Account> lA = null;
            // var resultcount = 0;
            if (!content.Contains(" "))
            {
                // search with username (tên tài khoản)
                // resultcount = md.TransferTo(content, 0).Count;
                lA = md.TransferTo(content, 0);
            }
            else
            {
                // search with fullname (tên chủ khoản)
                // resultcount = md.TransferTo(content, 1).Count;
                lA = md.TransferTo(content, 1);
            }

            if (lA != null)
            {
                if (lA.Count > 0)
                {
                    return ChooseAccountFromListTransfer(lA);
                }
                // else if (lA.Count > 1)
                // {
                //     return ChooseAccountFromListTransfer(lA);
                // }
            }

            return null;
        }

        private Account ChooseAccountFromListTransactionHistory(List<Account> lA)
        {
            Console.WriteLine("Đã tìm thấy {0} tài khoản đã từng giao dịch với bạn.", lA.Count);
            foreach (var acc in lA)
            {
                Console.WriteLine("STK: {0} - Tên tài khoản: {1} - Tên chủ khoản: {2}", acc.Id, acc.Username, acc.Name);
            }
            Console.WriteLine("Vui lòng nhập mã số tài khoản mà bạn muốn chọn trong danh sách phía trên:");

            Utility ulti = new Utility();
            var stk = ulti.GetIntNum();
            foreach (var acc in lA)
            {
                if (acc.Id == stk) return acc;
            }
            return null;
        }
        private Account ChooseAccountFromListTransfer(List<Account> lA)
        {
            Console.WriteLine("Đã tìm thấy {0} kết quả có tên chủ khoản tương ứng!", lA.Count);
            foreach (var acc in lA)
            {
                Console.WriteLine("STK: {0} - Tên tài khoản: {1} - Tên chủ khoản: {2}", acc.Id, acc.Username, acc.Name);
            }
            Console.WriteLine("Vui lòng nhập mã số tài khoản mà bạn muốn chọn trong danh sách phía trên:");

            Utility ulti = new Utility();
            var stk = ulti.GetIntNum();
            foreach (var acc in lA)
            {
                if (acc.Id == stk) return acc;
            }
            return null;
        }

        public bool GetAccountTransactionHistory(int type)
        {
            Model md = new Model();
            List<Transaction> lT = null;
            switch (type)
            {
                case 1:
                    {
                        // Lịch sử 5 ngày gần nhất
                        lT = md.GetTransactionHistory5Days(Program.a.Id);
                        break;
                    }
                case 2:
                    {
                        // Lịch sử 10 giao dịch gần nhất
                        lT = md.GetTransactionHistory10Newest(Program.a.Id);
                        break;
                    }
                case 3:
                    {
                        // Lịch sử theo thời gian chỉ định (X-Y hoặc X)
                        lT = GetTransactionWhereTimeBetween(Program.a);
                        break;
                    }
                case 4:
                    {
                        // lịch sử giao dịch giữa bạn với tài khoản khác
                        List<int> lAID = md.GetListAccountID_TransactionWithAccountID(Program.a.Id);
                        List<Account> rLA = new List<Account>();
                        if (lAID != null)
                        {
                            foreach (var aid in lAID)
                            {
                                Account curAcc = md.GetAccountById(aid);
                                if (curAcc != null) rLA.Add(curAcc);
                            }
                        }

                        if (rLA != null && rLA.Count > 0)
                        {
                            Account rA;
                            rA = ChooseAccountFromListTransactionHistory(rLA);
                            if (rA != null)
                            {
                                lT = md.GetTransactionHistoryWith(Program.a.Id, rA.Id);
                            }
                        }
                        else Console.WriteLine("Tài khoản của bạn chưa thực hiện giao dịch với tài khoản khác!");
                        break;
                    }
            }

            if (lT != null)
            {
                // var count = 0;
                if (lT.Count > 0)
                {
                    Console.Out.Flush();
                    Console.Clear();

                    Console.WriteLine("> Tìm thấy {0} kết quả tương ứng.", lT.Count);
                    Console.WriteLine(String.Format("|{0,30}|{1,15}|{2,15}|{3,20}|{4,15}|{5,15}|", "Thời gian", "Mã giao dịch", "Người gửi", "Nội dung", "Số tiền", "Người nhận"));
                    Console.WriteLine(String.Format("|{0,30}|{1,15}|{2,15}|{3,20}|{4,15}|{5,15}|", "------------------------------", "---------------", "---------------", "--------------------", "---------------", "---------------"));
                    foreach (var t in lT)
                    {
                        if (t.Type == TransactionType.DEPOSIT || t.Type == TransactionType.WITHRAW || t.Type == TransactionType.TRANSFER)
                        {
                            Console.WriteLine("|{0,30}|{1,15}|{2,15}|{3,20}|{4,15}|{5,15}|", t.issueDate, t.Id, t.fromAccount.Name, t.Type != TransactionType.TRANSFER ? t.Type == TransactionType.DEPOSIT ? "đã gửi" : "đã rút" : "đã chuyển khoản", t.Amount.ToString("N0"), t.Type == TransactionType.TRANSFER ? "" + t.toAccount.Name : "");
                        }
                    }
                    Console.WriteLine(String.Format("|{0,30}|{1,15}|{2,15}|{3,20}|{4,15}|{5,15}|", "------------------------------", "---------------", "---------------", "--------------------", "---------------", "---------------"));
                }
                else Console.WriteLine("Không tìm thấy giao dịch phù hợp với điều kiện tương ứng!");
            }
            else return false;

            if (lT.Count > 0) return true;
            return false;
        }

        private List<Transaction> GetTransactionWhereTimeBetween(Account a)
        {
            List<Transaction> rT = null;
            Utility ulti = new Utility();
            Model md = new Model();

            Console.WriteLine("Vui lòng nhập thời gian bắt đầu:");
            var beginTime = ulti.GetDateTime();
            DateTime endTime;
            // var endTime = beginTime;
            while (true)
            {
                Console.WriteLine("Bạn có muốn chỉ định thời gian kết thúc của tìm kiếm không? (y/n)\n * Nếu chọn 'n' hệ thống sẽ tìm kiếm lịch sử giao dịch từ thời gian bắt đầu đến thời điểm hiện tại.");
                var _cend = Console.ReadLine().ToLower();
                if (_cend == "y")
                {
                    Console.WriteLine("Vui lòng nhập thời gian kết thúc:");
                    endTime = ulti.GetDateTime();
                    break;
                }
                else if (_cend == "n")
                {
                    endTime = DateTime.Now;
                    break;
                    // break;
                }
                // else
                // {
                //     Console.WriteLine("Bạn có muốn chỉ định thời gian kết thúc của tìm kiếm không? (y/n)\n * Nếu chọn 'n' hệ thống sẽ tìm kiếm lịch sử giao dịch từ thời gian bắt đầu đến thời điểm hiện tại.");
                //     break;
                // }
            }

            Console.WriteLine("Begin Time: {0} - End Time: {1}", beginTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (beginTime < endTime) rT = md.GetTransactionHistoryFromTimeToTime(a.Id, beginTime.ToString("yyyy-MM-dd HH:mm:ss"), endTime.ToString("yyyy-MM-dd HH:mm:ss"));

            if (rT != null) return rT;
            else Console.WriteLine("Không tìm thấy kết quả tương ứng!");

            return null;
        }

        public void AccountInformation()
        {
            Model md = new Model();
            md.ResyncAccountMoney(Program.a);
            Console.WriteLine("Số dư trong tài khoản: {0} VNĐ", Program.a.Money.ToString("N0"));
            // Console.WriteLine("Tên chủ khoản")
        }
        public bool AccountTransactionAction(Account a1, Account a2, decimal amount, int type)
        {
            Transaction t = null;
            // TransactionModel tm = new TransactionModel();
            Model md = new Model();
            md.ResyncAccountMoney(a1);
            Console.WriteLine("Số tiền hiện có trong tài khoản trước giao dịch: {0}", a1.Money.ToString("N0"));
            switch (type)
            {
                case 1:
                    {
                        // deposit
                        if (a1 != null)
                        {
                            t = new Transaction(a1, a2, amount, TransactionType.DEPOSIT);
                            // if(md.AccountTransaction(t) == TransactionType.SUCCESS) return true;
                        }
                        break;
                    }
                case 2:
                    {
                        // withdraw
                        if (a1 != null)
                        {
                            // t = new Transaction(a1.Username,a2.Username,amount,2);
                            if (a1.Money < amount)
                            {
                                Console.WriteLine("Số tiền trong tài khoản không đủ!");
                                return false;
                            }

                            t = new Transaction(a1, a2, amount, TransactionType.WITHRAW);
                        }
                        break;
                    }
                case 3:
                    {
                        // transfer
                        if (a1 != null && a2 != null)
                        {
                            if (a1.Money < amount)
                            {
                                Console.WriteLine("Số tiền trong tài khoản không đủ!");
                                return false;
                            }
                            t = new Transaction(a1, a2, amount, TransactionType.TRANSFER);
                        }
                        break;
                    }

            }



            if (t != null)
            {
                var _cancel = false;
                while (true)
                {
                    if (t.Type == TransactionType.TRANSFER) Console.WriteLine("Bạn có chắc muốn thực hiện giao dịch trên với tài khoản này không?\n- STK: {0} - Tên chủ khoản: {1}", a2.Id, a2.Name);
                    else Console.WriteLine("Bạn có chắc muốn thực hiện giao dịch trên không?");

                    Console.WriteLine("Vui lòng bấm phím 'Y' để đồng ý, 'N' để hủy:");
                    var choice = Console.ReadKey().KeyChar;
                    if (choice == 'y')
                    {
                        break;
                    }
                    else if (choice == 'n')
                    {
                        Console.WriteLine("Bạn đã hủy giao dịch! Bấm ENTER để quay lại...");
                        Console.ReadLine();
                        _cancel = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Vui lòng nhập lựa chọn của bạn phía dưới:");
                    }

                }

                if (_cancel) return false;
            }

            if (t != null && md.AccountTransaction(t) == TransactionType.SUCCESS)
            {
                md.ResyncAccountMoney(a1);
                Console.WriteLine("Số tiền trong tài khoản sau giao dịch: {0}", a1.Money.ToString("N0"));
                return true;
            }
            return false;
        }
    }
}