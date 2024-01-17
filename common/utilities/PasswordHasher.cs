using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Common.Utilities;
public class PasswordHasher
{
    public string RandomPassword(int len)
    {
        string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
        string _numericChars = "0123456789";
        string res = "";
        Random random = new Random();
        for (int i = 0; i < len; i++)
        {
            if (random.Next(2) == 0)
            {
                res += _allowedChars[random.Next(_allowedChars.Length)];
            }
            else
            {
                res += _numericChars[random.Next(_numericChars.Length)];
            }
        }
        Console.WriteLine(res);
        return res;
    }
    public String HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 8);
    }
    public bool VerifyPassword(string hashedPassword, string passwordToCheck)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(passwordToCheck, hashedPassword);
    }
}
