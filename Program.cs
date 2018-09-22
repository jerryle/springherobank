using System;

namespace SpringHeroBank
{
    class Program
    {
        private static string Author = "Dũng Lê";
        private static string Version = "v1.01";
        // public static bool isLogged = false;
        public static Account a = new Account();
        private static TransactionController tc = new TransactionController();
        // public static AccountController ac = new AccountController();
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.Clear();
            Console.Out.Flush();
            if (a != null && !a.isLogged) MainMenu();
            else AccountMenu();
        }

        private static void GetAppInformation()
        {
            Console.Clear();
            Console.Out.Flush();
            Console.WriteLine("============ Thông tin ứng dụng ============");
            Console.WriteLine("Ứng dụng được viết và phát triển bởi {0}.", Author);
            Console.WriteLine("Phiên bản hiện tại là {0}.", Version);
            Console.WriteLine("Nếu phát hiện lỗi vui lòng báo cho SHB tại địa chỉ report@springherobank.com");
            Console.WriteLine("============================================");
        }
        // Tạo ra menu cho người dùng tương tác.
        private static void MainMenu()
        {
            if (a != null && !a.isLogged)
            {
                AccountController ac = new AccountController();
                while (true)
                {
                    Console.Clear();
                    Console.Out.Flush();
                    Console.WriteLine("---------SPRING HERO BANK---------");
                    Console.WriteLine("1. Đăng nhập tài khoản.");
                    Console.WriteLine("2. Đăng ký tài khoản.");
                    Console.WriteLine("3. Thoát chương trình.");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Vui lòng nhập lựa chọn (1|2|3): ");
                    // Yêu cầu người dùng nhập lựa chọn
                    Utility ult = new Utility();
                    var choice = ult.GetIntNum();
                    switch (choice)
                    {
                        case 1:
                            {
                                // đăng nhập
                                a = ac.SignIn();
                                if (a != null)
                                {
                                    a.isLogged = true;
                                }
                                break;
                            }
                        case 2:
                            {
                                // đăng ký
                                ac.SignUp();
                                break;
                            }
                        case 3:
                            {
                                // thoát
                                GetAppInformation();
                                Console.WriteLine("Chương trình đã dừng!");
                                Environment.Exit(1);
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Bạn đã nhập lựa chọn không phù hợp! Vui lòng chọn lại..");
                                break;
                            }
                    }

                    if (a != null && a.isLogged)
                    {
                        AccountMenu();
                        break;
                    }
                }
            }
            else AccountMenu();
        }

        private static void AccountMenu()
        {
            if (a != null && a.isLogged)
            {

                while (true)
                {
                    Console.Clear();
                    Console.Out.Flush();
                    Console.WriteLine("---------SPRING HERO BANK---------");
                    Console.WriteLine("\t| Chào mừng, {0}! |", a.Name.ToUpper());
                    Console.WriteLine("1. Truy vấn thông tin tài khoản.");
                    Console.WriteLine("2. Rút tiền.");
                    Console.WriteLine("3. Gửi tiền.");
                    Console.WriteLine("4. Chuyển khoản.");
                    Console.WriteLine("5. Lịch sử giao dịch.");
                    Console.WriteLine("6. Đăng xuất.");
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Vui lòng nhập lựa chọn (1|2|3|4|5|6): ");
                    // resync tài khoản trước
                    Model md = new Model();
                    md.ResyncAccountMoney(a);
                    // Yêu cầu người dùng nhập lựa chọn
                    Utility ult = new Utility();
                    var choice = ult.GetIntNum();
                    switch (choice)
                    {
                        case 1:
                            {
                                AccountController ac = new AccountController();
                                ac.AccountInformation();
                                break;
                            }
                        case 2:
                            {
                                // TransactionController tc = new TransactionController();
                                if (tc.AccountWithdraw())
                                {
                                    Console.WriteLine("Rút tiền thành công!");
                                    // Console.WriteLine("Số tiền trong tài khoản hiện tại:")
                                    // thành công
                                    // Console.WriteLine("");

                                }
                                // else {
                                //     Console.WriteLine("Có lỗi xảy ra trong quá trình giao dịch! Vui lòng thử lại sau.");
                                //     // không thành công
                                // }
                                // rút tiền
                                break;
                            }
                        case 3:
                            {
                                // gửi tiền
                                // var amount = ult.GetDecimalNum();

                                if (tc.AccountDeposit())
                                {
                                    // thành công
                                    // Console.WriteLine("");
                                    Console.WriteLine("Gửi tiền thành công!");

                                }
                                // else {
                                //     // không thành công
                                //     Console.WriteLine("Có lỗi xảy ra trong quá trình giao dịch! Vui lòng thử lại sau.");
                                // }
                                break;
                            }
                        case 4:
                            {
                                // chuyển tiền
                                if (tc.AccountTransfer())
                                {
                                    // thành công
                                    // Console.WriteLine("");
                                    Console.WriteLine("Gửi tiền thành công!");

                                }
                                // else {
                                //     // không thành công
                                //     Console.WriteLine("Có lỗi xảy ra trong quá trình giao dịch! Vui lòng thử lại sau.");
                                // }
                                break;
                            }
                        case 5:
                            {
                                // lịch sử giao dịch
                                if (tc.GetAccountTransactionHistoryMain())
                                {
                                    // thành công
                                    // Console.WriteLine("");
                                    Console.WriteLine("Đã kiểm tra lịch sử!");

                                }
                                // else Console.WriteLine("Lấy lịch sử giao dịch thất bại!");
                                break;
                            }
                        case 6:
                            {
                                // thoát
                                Console.Clear();
                                Console.Out.Flush();
                                Console.WriteLine("Đã đăng xuất! Ấn ENTER để quay trở về màn hình chính..");
                                Console.ReadLine();
                                // a.isLogged = !a.isLogged;
                                a = new Account();
                                a.isLogged = false;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Bạn đã nhập lựa chọn không phù hợp! Vui lòng chọn lại..");
                                break;
                            }
                    }

                    if (a != null && !a.isLogged)
                    {
                        MainMenu();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\tBấm ENTER để quay lại...");
                        Console.ReadLine();
                    }
                }
            }
            else MainMenu();
        }
    }
}
