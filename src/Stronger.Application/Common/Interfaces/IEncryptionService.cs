using System;

namespace Stronger.Application.Common.Interfaces;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}
