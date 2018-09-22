using System;

namespace SpringHeroBank
{
    public class TransactionController
    {
        // private Transaction t;
        private Utility ult = new Utility();
        // private TransactionModel tm = new TransactionModel();
        private AccountController ac = new AccountController();
        public bool AccountDeposit()
        {
            Console.Clear();
            Console.Out.Flush();
            Console.WriteLine("> Vui lòng nhập số tiền bạn muốn gửi:");
            var amount = ult.GetUnsignedDecimalNum();

            if (ac.AccountTransactionAction(Program.a, Program.a, amount, 1)) return true;
            return false;
        }

        public bool AccountWithdraw()
        {
            Console.Clear();
            Console.Out.Flush();
            Console.WriteLine("> Vui lòng nhập số tiền bạn muốn rút:");
            var amount = ult.GetUnsignedDecimalNum();
            if (ac.AccountTransactionAction(Program.a, Program.a, amount, 2)) return true;
            return false;
        }

        public bool AccountTransfer()
        {
            Console.Clear();
            Console.Out.Flush();
            Console.WriteLine("Vui lòng nhập chính xác tên tài khoản hoặc tên chủ khoản mà bạn muốn chuyển tới:");
            AccountController ac = new AccountController();
            var searchKey = Console.ReadLine().ToLower();
            Account rA = ac.ValidateWhereAccountTransferTo(searchKey);
            if (rA == null)
            {
                Console.WriteLine("Không tìm thấy tài khoản mà bạn vừa nhập!");
                return false;
            }


            Console.WriteLine("> Vui lòng nhập số tiền bạn muốn gửi:");
            var amount = ult.GetUnsignedDecimalNum();
            if (ac.AccountTransactionAction(Program.a, rA, amount, 3)) return true;
            return false;
        }

        public bool GetAccountTransactionHistoryMain()
        {

            // if (ac.GetAccountTransactionHistory(a))
            // {
            //     // Console.WriteLine("Kiểm tra");
            //     return true;
            // }
            // else _success = false;

            var _success = true;
            // Console.WriteLine("Vui lòng chọn một trong các kiểu kiểm tra lịch sử mà bạn muốn phía dưới:");
            Console.WriteLine("Vui lòng chọn một trong các kiểu kiểm tra lịch sử mà bạn muốn phía dưới:");
            while (true)
            {
                Console.Clear();
                Console.Out.Flush();
                Console.WriteLine("========== TRANSACTION HISTORY MENU ==========");
                Console.WriteLine("1. Lịch sử 5 ngày gần nhất");
                Console.WriteLine("2. Lịch sử 10 giao dịch gần nhất");
                Console.WriteLine("3. Lịch sử theo khoảng thời gian (ngày/tháng/năm ~ ngày/tháng/năm)");
                Console.WriteLine("4. Lịch sử giao dịch giữa bạn với một ai đó");
                Console.WriteLine("5. Quay trở về");
                Console.WriteLine("==============================================");


                Utility ult = new Utility();
                var choice = ult.GetIntNum();
                if (choice > 0 && choice < 5)
                {
                    if (ac.GetAccountTransactionHistory(choice))
                    {
                        Console.WriteLine("Kiểm tra lịch sử thành công!");
                    }
                    else _success = false;
                }
                else if (choice == 5) _success = false;
                else Console.WriteLine("Bạn đã nhập lựa chọn không phù hợp! Vui lòng chọn lại..");


                if ((Program.a != null && !Program.a.isLogged) || !_success)
                {

                    break;
                }
                else
                {
                    Console.WriteLine("\tBấm ENTER để quay lại...");
                    Console.ReadLine();
                    // break;
                }
            }

            if (_success) return true;

            return false;
        }

    }
}