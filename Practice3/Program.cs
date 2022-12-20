using System.Security.Cryptography;
using System.Text;

public class Authorisation
{
      private const string SAULT = "FeelsPepeMan"; // Соль
      private Dictionary<string, string> Tier4 = new Dictionary<string, string>(); // Логин + пароль
      private Dictionary<string, string> Tier3 = new Dictionary<string, string>(); // Логин + хеш
      private Dictionary<string, string> Tier2 = new Dictionary<string, string>(); // Логин + хеш с солью
      private Dictionary<string, (string Sault, string Pass)> Tier1 = new Dictionary<string, (string Sault, string Pass)>(); // Логин + хеш с случайной солью

      public void Register(string login, string password)
      {
            Tier4.Add(login, password);
            Tier3.Add(login, GetHashString(password));
            Tier2.Add(login, GetHashString(SAULT + password));

            string randomSault = GetRandomString();
            Tier1.Add(login, (randomSault, GetHashString(randomSault + password)));
            
      }

      public string GetData(string login)
      {
            return $"Tier 4: {Tier4[login]}\n" +
                  $"Tier 3: {Tier3[login]}\n" +
                  $"Tier 2: [Sault: {SAULT}, Pass: {Tier2[login]}]\n" +
                  $"Tier 1: [Sault: {Tier1[login].Sault}, Pass: {Tier1[login].Pass}]";
      }

      private static string GetHashString(string str)
      {
            StringBuilder sb = new StringBuilder();
            byte[] bytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            foreach (byte b in bytes)
                  sb.Append(b.ToString("X2"));

            return sb.ToString();
      }

      private static string GetRandomString(int length = 5)
      {
            const string vocab = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();

            return new string(Enumerable.Repeat(vocab, length)
                  .Select(s => s[rnd.Next(s.Length)])
                  .ToArray());
      }
}

public partial class Program
{
      public static void Main(string[] args)
      {
            Authorisation auth = new Authorisation();
            for (int i = 0; i < 2; i++)
            {
                  Console.Write("Login: ");
                  string login = Console.ReadLine();
                  Console.Write("Password: ");
                  string password = Console.ReadLine();

                  auth.Register(login, password);
                  Console.WriteLine(auth.GetData(login));
                  Console.WriteLine();
            }
      }
}