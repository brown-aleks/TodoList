using Microsoft.VisualBasic;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TodoList
{
    public static class Authorization
    {
        private static readonly string _fileUsers = "Users.dat";
        private static readonly string _directory = "./SecretDate/";
        private static readonly string _pathUsers = Path.Combine(_directory, _fileUsers);
        private static readonly JsonSerializerOptions optionsJson;

        static Authorization()
        {
            optionsJson = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };
        }
        public static User? AuthorizeUser()
        {
            var loginPassword = InputLoginPassword(confirm: false);
            var user = FindUser(loginPassword, out bool passwordMatches);
            if (user == null)
            {
                Console.WriteLine("Пользователь не найден.");
                return null;
            }
            if (passwordMatches)
            {
                return user;
            }
            else
            {
                Console.WriteLine("Пароль не соответствует.");
                return null;
            }
        }
        public static User? AddUser()
        {
            var loginPassword = InputLoginPassword(confirm: true);

            var user = FindUser(loginPassword, out bool passwordMatches);

            if (user != null)
            {
                Console.WriteLine("\nПользователь с таким ником уже существует.");
                return null;
            }

            Tuple<string, string, Guid> newAccount = new(loginPassword.Item1, loginPassword.Item2, Guid.NewGuid());

            Directory.CreateDirectory(_directory);

            var json = JsonSerializer.Serialize(newAccount, optionsJson);

            var crypt = Cryptography.Encrypt(json);

            using (BinaryWriter writer = new BinaryWriter(File.Open(_pathUsers, FileMode.Append)))
            {
                writer.Write(crypt);
            }

            return new User() { Name = newAccount.Item1, Id = newAccount.Item3 };
        }
        private static User? FindUser(Tuple<string, string> loginPassword, out bool passwordMatches)
        {
            passwordMatches = false;
            if (!Directory.Exists(_directory)) return null;

            using (BinaryReader reader = new BinaryReader(File.Open(_pathUsers, FileMode.Open)))
            {
                while (reader.PeekChar() > -1)
                {
                    string crypt = reader.ReadString();

                    string json = Cryptography.Decrypt(crypt);
                    var account = JsonSerializer.Deserialize<Tuple<string, string, Guid>>(json);

                    if (loginPassword.Item1 == account?.Item1)
                    {
                        if (loginPassword.Item2 == account.Item2) { passwordMatches = true; }
                        return new User() { Name = account.Item1, Id = account.Item3 };
                    }
                }
            }
            return null;
        }
        private static Tuple<string, string> InputLoginPassword(bool confirm = false)
        {
            string login = string.Empty;
            string pervisPas = string.Empty;
            string confirmPas = string.Empty;

            while (string.IsNullOrWhiteSpace(login) && !ConsoleHelper.EescapePressed)
            {
                Console.Write("Введите логин: ");
                login = ConsoleHelper.ReadLine() ?? string.Empty;
                Console.WriteLine();
            }

            while (string.IsNullOrWhiteSpace(pervisPas) && !ConsoleHelper.EescapePressed)
            {
                Console.Write("Введите пароль: ");
                ConsoleHelper.HidePassword = true;
                pervisPas = ConsoleHelper.ReadLine() ?? string.Empty;
                Console.WriteLine();
            }

            if (confirm)
            {
                while (confirmPas != pervisPas && ConsoleHelper.EescapePressed)
                {
                    Console.Write("Повторите пароль: ");
                    confirmPas = ConsoleHelper.ReadLine() ?? string.Empty;
                    Console.WriteLine();
                };
            }
            ConsoleHelper.HidePassword = false;

            return new(login, pervisPas);
        }

    }
}
