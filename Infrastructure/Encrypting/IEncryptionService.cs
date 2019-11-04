using System;
namespace Infrastructure.Encrypting
{
    interface IEncryptionService
    {
        T DecryptObject<T>(object value, T defaultValue);
        string DecryptString(string encryptedString);
        string DecryptString(string encryptedString, string Key);

        string EncryptString(string decryptedString);
        string EncryptString(string decryptedString, string key);
    }
}
