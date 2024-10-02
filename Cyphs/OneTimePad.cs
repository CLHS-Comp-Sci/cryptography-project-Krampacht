using System;
using System.Security.Cryptography;
public class Program
{
    private static Encryptor? encryptor;
    private static Decryptor? decryptor;
    
    private static string text = "TOWARDS";
    public static void Main(string[] args)
    {
        encryptor = new Encryptor(text);
        encryptor.Encrypt();
        decryptor = new Decryptor(encryptor.GetCyphertext(), encryptor.GetKey());
        decryptor.Decrypt();
    }
}

public class Decryptor
{
    private char[]? plainText;
    private char[] key;
    private char[] cypherText;
    
    public Decryptor(char[] CypherText, char[] Key)
    {
        cypherText = CypherText;
        key = Key;
        if (key.Length < cypherText.Length)
        {
            throw new ArgumentException("Key length must be equal or greater in size than the cyphertext");
        }
    }
    
    public void Decrypt()
    {
        plainText = new char[cypherText.Length];
        for (int i = 0; i < cypherText.Length; i++)
        {
            plainText[i] = (char)((byte)cypherText[i] - ((byte)key[i]) + 65);
        }
        for (int i = 0; i < plainText.Length; i++)
        {
            if (plainText[i] < 65)
            {
                plainText[i] += (char)26;
            }
        }
        Console.WriteLine("\nDecrypted Text: ");
        Console.WriteLine(plainText);
    }
}

public class Encryptor
{
    private char[] plainText;
    private char[]? key;
    private char[]? cypherText;
    
    public Encryptor(string PlainText)
    {
        plainText = PlainText.ToCharArray();
    }
    
    public void Encrypt()
    {
        key = new char[plainText.Length];
        cypherText = new char[plainText.Length];
        Random random = new Random();
        
        for (var i = 0; i < key.Length; i++)
        {
            key[i] = (char)random.Next(65, 91);
        }
        for (var i = 0; i < cypherText.Length; i++)
        {
            cypherText[i] = (char)((((byte)plainText[i] + (byte)key[i]) % 26 ) + 65);
        }
        for (var i = 0; i < cypherText.Length; i++)
        {
            if (cypherText[i] > 91)
            {
                cypherText[i] -= (char)26;
            }
        }
        
        Console.WriteLine("Plaintext: ");
        Console.WriteLine(plainText);
        Console.WriteLine("Key: ");
        Console.WriteLine(key);
        Console.WriteLine("Cyphertext: ");
        Console.WriteLine(cypherText);
        
    }
    public char[] GetKey()
    {
        return key!;
    }
    public char[] GetCyphertext()
    {
        return cypherText;
    }
}