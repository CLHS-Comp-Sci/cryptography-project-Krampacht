using System;
using System.Security.Cryptography;

public class Program
{
    public static void Main()
    {
        int length = 2048; //Set the maximum bound for the cypher

        var encryptor = new Encryptor(length, "Out, out, brief candle! Life's but a walking shadow, a poor player. That struts and frets his hour upon the stage. And then is heard no more."); //Create a new encryptor with the length and the plaintext
        encryptor.Encrypt();

        var decryptor = new Decryptor(length, encryptor.Cyphertext, encryptor.Key); //Create a new decryptor with the length and both the Cyphertext and Key generated by Encryptor.
        decryptor.Decrypt();

        Console.WriteLine($"{encryptor}\n\n{decryptor}"); //Print the results of both the encryptor and decryptor classes.
    }
}

public class Decryptor
{
    private int cypherLength; //The length of the cypher, used to ensure that the characters stay in bounds of the encryption
    private char[]? plainText; //The plain unencrypted text
    private char[] key; //The key that is subtracted and modulo'd from the cyphertext to create the plaintext
    private char[] cypherText; //The output created by encryption

    public Decryptor(int CypherLength, string CypherText, string Key)
    {
        cypherLength = CypherLength;
        cypherText = CypherText.ToCharArray();
        key = Key.ToCharArray();

        if (key.Length < cypherText.Length)
        {
            throw new ArgumentException("The key length must be equal or greater in size than the cyphertext!");
            //One-Time Pads only are valid if the key is at least the size of the plaintext. In this implementation the plaintext, key, and cyphertext are the same length.
        }
    }

    public void Decrypt()
    {
        plainText = new char[cypherText.Length];

        for (var i = 0; i < plainText.Length; i++) //For every value in plainText's length
        {
            //Subtract the key from the cyphertext and modulo by the cypherLength, then cast this to an int implicitly to avoid unsigned overflows
            int plainValue = ((cypherText[i] - key[i]) % cypherLength);

            if (plainValue < 0) //If the value is less than 0 add the cypherLength
            {
                plainValue += cypherLength;
            }
            plainText[i] = (char)plainValue; //Set the character at index I to an char explicit cast from plainValue
        }
    }

    public override string ToString()
    {
        return $"Decryptor:\nDecrypted Plaintext: {Plaintext}"; //Returns a string of the contents of decryptor with new line formatting
    }

    public string Key
    {
        get
        {
            return new string(key); //Return the key char array as a string
        }
    }

    public string Plaintext
    {
        get
        {
            return new string(plainText); //Return the plainText char array as a string
        }
    }

    public string Cyphertext
    {
        get
        {
            return new string(cypherText); //Return the cypherText char array as a string
        }
    }
}

public class Encryptor
{
    private int cypherLength; //The length of the cypher, used to ensure that the characters stay in bounds of the encryption
    private char[] plainText; //The plain unencrypted text
    private char[]? key; //The key that is added and modulo'd to the plaintext to create the cyphertext
    private char[]? cypherText; //The output created by encryption

    public Encryptor(int CypherLength, string PlainText)
    {
        cypherLength = CypherLength;
        plainText = PlainText.ToCharArray();
    }

    public Encryptor(int CypherLength, string PlainText, string Key)
    {
        cypherLength = CypherLength;
        plainText = PlainText.ToCharArray();
        key = Key.ToCharArray();
    }

    public void Encrypt()
    {
        if (key is null) //Check if the key is currently null
        {
            key = new char[plainText.Length];
            GenerateKey(); //Generate a key
        }

        cypherText = new char[plainText.Length];
        for (var i = 0; i < cypherText.Length; i++) //For every value in cypherText's length
        {
            cypherText[i] = (char)((plainText[i] + key[i]) % cypherLength); //Set the character at index I to the plainText + Key at the same index modulo'd by the cypher length
            if (cypherText[i] > cypherLength) //If the value exceeds the cypher length subtract the cypherlength
            {
                cypherText[i] -= (char)cypherLength;
            }
        }
    }

    public override string ToString() //Returns the plaintext, key, and cyphertext with newline formatting
    {
        return $"Encryptor:\nPlaintext: {Plaintext}\nKey: {Key}\nCyphertext: {Cyphertext}";
    }

    private void GenerateKey()
    {
        for (var i = 0; i < key.Length; i++) //For every value in key's length
        {
            var bytes = RandomNumberGenerator.GetBytes(sizeof(char)); //Generate a cryptographically secure array of bytes,, a char is 2 bytes large (16 bit)
            var character = (char)(BitConverter.ToChar(bytes) % cypherLength); //Convert the byte array into a character, modulo'd to ensure it stays within cypher bounds.
            key[i] = character; //Set the character at index I to the resulting character
        }
    }

    public string Key
    {
        get
        {
            return new string(key); //Return the char array key as a string
        }
    }
}