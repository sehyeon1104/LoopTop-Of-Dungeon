using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Crypto
{
    // Ű�� ����ϱ� ���� ��ȣ
    private static readonly string PASSWORD = "passwordpassword";
    // ����Ű ����
    private static readonly string KEY = PASSWORD.Substring(0, 128 / 8);

    // ��ȣȭ
    public static string AESEncrypt128(string plain)
    {
        byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

        // AES : ��� ��ȣȭ ǥ��
        // ���δ�(Rijndael) : ���⿡ �����ڵ� �̸��� ���� ���� �˰��� �̸�
        RijndaelManaged rijndael = new RijndaelManaged();

        // CipherMode : ��ȣȭ ��� ����
        // https://learn.microsoft.com/ko-kr/dotnet/api/system.security.cryptography.ciphermode?view=net-6.0
        rijndael.Mode = CipherMode.CBC;

        // PaddingMode : �޽��� ������ ����� ��ȣȭ �۾��� �ʿ��� ��ü ���̺��� ª�� �� ���� ä�� ������ ����
        // https://learn.microsoft.com/ko-kr/dotnet/api/system.security.cryptography.paddingmode?view=net-6.0
        rijndael.Padding = PaddingMode.PKCS7;

        // KeySize : �н����� Ű ������
        // https://learn.microsoft.com/ko-kr/dotnet/api/system.security.cryptography.rijndaelmanaged.keysize?view=net-6.0#system-security-cryptography-rijndaelmanaged-keysize
        rijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream();

        ICryptoTransform encryptor = rijndael.CreateEncryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));

        CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
        cryptoStream.FlushFinalBlock();

        byte[] encryptBytes = memoryStream.ToArray();
        string encryptString = Convert.ToBase64String(encryptBytes);

        cryptoStream.Close();
        memoryStream.Close();

        return encryptString;
    }

    public static string AESDecrypt128(string encrypt)
    {
        byte[] encryptBytes = Convert.FromBase64String(encrypt);

        RijndaelManaged rijndael = new RijndaelManaged();
        rijndael.Mode = CipherMode.CBC;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.KeySize = 128;

        MemoryStream memoryStream = new MemoryStream(encryptBytes);
        ICryptoTransform decryptor = rijndael.CreateDecryptor(Encoding.UTF8.GetBytes(KEY), Encoding.UTF8.GetBytes(KEY));
        CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        byte[] plainBytes = new byte[encryptBytes.Length];

        int plainCount = cryptoStream.Read(plainBytes, 0, plainBytes.Length);

        string plainString = Encoding.UTF8.GetString(plainBytes, 0, plainCount);

        cryptoStream.Close();
        memoryStream.Close();

        return plainString;
    }

}
