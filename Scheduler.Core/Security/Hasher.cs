using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

// https://www.devtrends.co.uk/blog/hashing-encryption-and-random-in-asp.net-core
// Simple class to generate and verify a Hash

namespace Scheduler.Core.Security
{
    public static class Hasher
    {    
        // AMC - verify if a string is already hashed
        public static bool IsHashed(string base64)
        {
            if (base64.Contains(':'))
            {
                base64 = base64.Trim();
                var parts = base64.Split(':');
                var isSaltBase64  = (parts[0].Length % 4 == 0) && Regex.IsMatch(parts[0], @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
                var isHashBaseH64 = (parts[1].Length % 4 == 0) && Regex.IsMatch(parts[1], @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
                return isSaltBase64 && isHashBaseH64;
            }
            return false;           
        }
        
        // Generated Salt and Hash returned
        public static string CalculateHash(string input)
        {
            // AMC check that input is not already hashed
            if (IsHashed(input)) return input;

            var salt = GenerateSalt(16);
            var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);
            return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
        }

        private static byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }
            return salt;
        }

        // Combined Salt and Hash string verified against the input string
        public static bool ValidateHash(string hash, string input)
        {
            try
            {
                var parts = hash.Split(':');
                var salt = Convert.FromBase64String(parts[0]);
                var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);
                return parts[1].Equals(Convert.ToBase64String(bytes));
            } catch { return false; }            
        }

    }
}