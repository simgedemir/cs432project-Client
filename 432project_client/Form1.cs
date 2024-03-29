﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;

namespace _432project_client
{
    public partial class Form1 : Form
    {
        string enc_dec_keys;
        string sig_ver_keys;
        string en_dec_session_key; // per session
        string auth_session_key; //per session

        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        string username;
        string IP;
        int port;
        byte[] halfPass; //half of hash of password     
        string hexnum="";
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void sendButton_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            terminating = false;
            if (enc_dec_keys == null)
            {
                using (System.IO.StreamReader fileReader =
                new System.IO.StreamReader("server_enc_dec_pub.txt"))
                {
                    enc_dec_keys = fileReader.ReadLine();
                }

                byte[] encKeyByte = Encoding.Default.GetBytes(enc_dec_keys);
                string hexKeys = generateHexStringFromByteArray(encKeyByte);

                // logs.AppendText("Server's public key: " + hexKeys);
            }

            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IP = ipBox.Text;

            username = usernameBox.Text;
            string password = passwordBox.Text;
            Byte[] buffer = new Byte[256];
            if (Int32.TryParse(portBox.Text, out port))
            {
                try
                {
                    clientSocket.Connect(IP, port);

                    connectButton.Enabled = false;
                    connected = true;
                    logs.AppendText("Connected to server\n");
                    //hashing the password
                    byte[] hashedPass = hashWithSHA256(password);
                    halfPass = new byte[16];
                    Array.Copy(hashedPass, 16, halfPass, 0, 16);
                    //concatenating password and username 
                    string message = Encoding.Default.GetString(halfPass) + username;

                    //RSA encryption
                    byte[] encrptedRSAmessage = encryptWithRSA(message, 3072, enc_dec_keys);
                    //sending to the server
                    try
                    {
                        clientSocket.Send(encrptedRSAmessage);
                        logs.AppendText("Enrollment request has been sent. \n");

                        Thread receiveThread = new Thread(new ThreadStart(Receive));
                        receiveThread.Start();
                    }
                    catch
                    {
                        connected = false;
                        logs.AppendText("Server is down. Your message could not be sent.\n");
                        clientSocket.Close();
                    }
                    
                }
                catch(Exception ex)
                {
                    Console.Write(ex);
                    logs.AppendText("Could not connect to server\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            terminating = false;
            username = usernameBox.Text;
            string password = passwordBox.Text;
            IP = ipBox.Text;

            Byte[] buffer = new Byte[64];
            if (Int32.TryParse(portBox.Text, out port))
            {
                try
                {
                    clientSocket.Connect(IP, port);
                    
                    loginButton.Enabled = false;
                    connected = true;
                    logs.AppendText("Connected to server\n");
                    //hashing the password
                    byte[] hashedPass = hashWithSHA256(password);
                    halfPass = new byte[16];
                    Array.Copy(hashedPass, 16, halfPass, 0, 16);
                    //concatenating password and username 
                    string message = username + "Authenticate";

                    //RSA encryption
                    //sending to the server
                    try
                    {
                        buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                        logs.AppendText("Login request has been sent. \n");
                    }
                    catch
                    {
                        connected = false;
                        logs.AppendText("Server is down. Your message could not be sent.\n");
                        clientSocket.Close();
                    }

                    Thread receiveThread = new Thread(new ThreadStart(Receive));
                    receiveThread.Start();
                }
                catch
                {
                    logs.AppendText("Could not connect to server\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }


        }

        private void Receive()
        {
            if (sig_ver_keys == null)
            {
                using (System.IO.StreamReader fileReader =
                new System.IO.StreamReader("server_signing_verification_pub.txt"))
                {
                    sig_ver_keys = fileReader.ReadLine();
                }

                byte[] signKeyByte = Encoding.Default.GetBytes(sig_ver_keys);
                string signHex = generateHexStringFromByteArray(signKeyByte);
                //logs.AppendText("Server's signing key: " + signHex);
            }

            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[512];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.TrimEnd('\0');
                    if (incomingMessage.Contains("Error"))
                    {
                        logs.AppendText( "Username taken.\n");
                        clientSocket.Close();
                        loginButton.Enabled = true;
                    }
                    else if(incomingMessage.Contains("Broadcast:"))
                    {
                        int index = incomingMessage.IndexOf(":");
                        incomingMessage = incomingMessage.Substring(index + 1);
                        byte [] longMessage= Encoding.Default.GetBytes(incomingMessage);
                        byte[] hmac = new byte[32];
                        byte[] encryptedMes = new byte[16];
                        byte [] IV= new byte[16];
                        Array.Copy(longMessage, 0, hmac, 0, 32);
                        Array.Copy(longMessage, 32, encryptedMes, 0, 16);
                        Array.Copy(longMessage, 48, IV, 0, 16);
                        byte[] hmacsha256 = applyHMACwithSHA256(Encoding.Default.GetString(encryptedMes), Encoding.Default.GetBytes(auth_session_key));
                        string hmacsha256Str = Encoding.Default.GetString(hmacsha256);
                        string hmacstr = Encoding.Default.GetString(hmac);
                        if (hmacstr.Equals(hmacsha256Str))
                        {
                            byte[] decryptedMes = decryptWithAES128(Encoding.Default.GetString(encryptedMes), Encoding.Default.GetBytes(en_dec_session_key), IV);
                            logs.AppendText(Encoding.Default.GetString(decryptedMes)+ "\n");
                        }
                    }
                    else if (incomingMessage.Contains("Challenge:"))
                    {
                        int index = incomingMessage.IndexOf(":");
                        incomingMessage = incomingMessage.Substring(index + 1);
                        byte[] num = Encoding.Default.GetBytes(incomingMessage);
                        hexnum = generateHexStringFromByteArray(num);
             
                        //logs.AppendText("Challenge num:\n" + hexnum + "\n");

                        byte[] hmacsha256 = applyHMACwithSHA256(incomingMessage, halfPass);
                        string message = "HMAC{"+ username +"}" + Encoding.Default.GetString(hmacsha256);
                        byte[] response = Encoding.Default.GetBytes(message);
                        clientSocket.Send(response);
                    }
                    else if(incomingMessage.Contains("OK"))
                    {
                        string message = incomingMessage.Substring(384);
                        string signature = incomingMessage.Substring(0, 384);
                        // signing with RSA 4096
                        byte[] signatureRSA = Encoding.Default.GetBytes(signature);

                        bool verificationResult = verifyWithRSA(message, 3072, sig_ver_keys, signatureRSA);
                        if (verificationResult == true)
                        {
                            
                            if (message.Contains("NOT OK"))
                            {
                                logs.AppendText("HMAC NOT OK. Exiting...\n");
                                clientSocket.Close();
                                connected = false;
                                loginButton.Enabled = true;
                            }
                            else
                            {
                                logs.AppendText("HMAC OK. Connection secure. \n");
                                                              
                                string encryptedKeys = message.Substring(2);
                                byte[] sessionKeys = decryptWithAES128(encryptedKeys, halfPass, hexStringToByteArray(hexnum));
                                string tempKeys = Encoding.Default.GetString(sessionKeys);
                                en_dec_session_key = tempKeys.Substring(0, tempKeys.Length / 2);
                                auth_session_key = tempKeys.Substring(tempKeys.Length / 2);
                                button1.Enabled = true; //enable message sending
                                connectButton.Enabled = false;
                            }
                        }
                        else
                        {
                            logs.AppendText("Invalid signature. Disconnecting... \n");
                            clientSocket.Close();
                            connected = false;
                        }
                    }

                    else if(incomingMessage.Length>0)
                    { 
                        string message = incomingMessage.Substring(384);
                        string signature = incomingMessage.Substring(0, 384);
                        // signing with RSA 4096
                        byte[] signatureRSA = Encoding.Default.GetBytes(signature);

                        bool verificationResult = verifyWithRSA(message, 3072, sig_ver_keys, signatureRSA);
                        if (verificationResult == true)
                        {
                            logs.AppendText("Valid signature \n");
                            if (incomingMessage.Contains("SuccessEnrolled"))
                            {
                                logs.AppendText("Enrollment is successful.\nPlease login \n");
                                loginButton.Enabled = true;                                
                            }
                            else
                            {
                                logs.AppendText("Try with another username.\n");
                                connectButton.Enabled = true;
                                connected = false;
                                clientSocket.Close();
                            }
                        }
                        else
                        {
                            logs.AppendText("Invalid signature. Disconnecting... \n");
                            clientSocket.Close();
                            connected = false;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.Write(ex);
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                    }

                    clientSocket.Close();
                    connected = false;
                }
            }
        }

        // Send button on click --> Broadcast message sending
        private void button1_Click(object sender, EventArgs e)
        {
            String message = messageBox.Text;
            if (message.Length > 0)
            {
                byte [] randomIV = new byte[16];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(randomIV);
                }
                byte[] key = Encoding.Default.GetBytes(en_dec_session_key);
                byte[] encrypedMessage = encryptWithAES128(message,key,randomIV);          
                byte[] hmacMessage = applyHMACwithSHA256(Encoding.Default.GetString(encrypedMessage), Encoding.Default.GetBytes(auth_session_key));
                string newMessage = "HMAC{" + username + "}" + Encoding.Default.GetString(hmacMessage);
                newMessage = newMessage + Encoding.Default.GetString(encrypedMessage)+ Encoding.Default.GetString(randomIV);
                byte[] buffer = Encoding.Default.GetBytes(newMessage);
                try
                {
                    clientSocket.Send(buffer);
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }


        // signing with RSA
        static byte[] signWithRSA(string input, int algoLength, string xmlString)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create RSA object from System.Security.Cryptography
            RSACryptoServiceProvider rsaObject = new RSACryptoServiceProvider(algoLength);
            // set RSA object with xml string
            rsaObject.FromXmlString(xmlString);
            byte[] result = null;

            try
            {
                result = rsaObject.SignData(byteInput, "SHA256");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        // verifying with RSA
        static bool verifyWithRSA(string input, int algoLength, string xmlString, byte[] signature)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create RSA object from System.Security.Cryptography
            RSACryptoServiceProvider rsaObject = new RSACryptoServiceProvider(algoLength);
            // set RSA object with xml string
            rsaObject.FromXmlString(xmlString);
            bool result = false;

            try
            {
                result = rsaObject.VerifyData(byteInput, "SHA256", signature);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        // RSA encryption with varying bit length
        static byte[] encryptWithRSA(string input, int algoLength, string xmlStringKey)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create RSA object from System.Security.Cryptography
            RSACryptoServiceProvider rsaObject = new RSACryptoServiceProvider(algoLength);
            // set RSA object with xml string
            rsaObject.FromXmlString(xmlStringKey);
            byte[] result = null;

            try
            {
                //true flag is set to perform direct RSA encryption using OAEP padding
                result = rsaObject.Encrypt(byteInput, true);
            }
            catch
            {
                //logs.AppendText("Encryption could not be done. \n");
            }

            return result;
        }

        public static string generateHexStringFromByteArray(byte[] input)
        {
            string hexString = BitConverter.ToString(input);
            return hexString.Replace("-", "");
        }

        public static byte[] hexStringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        static byte[] hashWithSHA256(string input)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create a hasher object from System.Security.Cryptography
            SHA256CryptoServiceProvider sha256Hasher = new SHA256CryptoServiceProvider();
            // hash and save the resulting byte array
            byte[] result = sha256Hasher.ComputeHash(byteInput);

            return result;
        }

        static byte[] applyHMACwithSHA256(string input, byte[] key)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);
            // create HMAC applier object from System.Security.Cryptography
            HMACSHA256 hmacSHA256 = new HMACSHA256(key);
            // get the result of HMAC operation
            byte[] result = hmacSHA256.ComputeHash(byteInput);

            return result;
        }
        static byte[] encryptWithAES128(string input, byte[] key, byte[] IV)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);

            // create AES object from System.Security.Cryptography
            RijndaelManaged aesObject = new RijndaelManaged();
            // since we want to use AES-128
            aesObject.KeySize = 128;
            // block size of AES is 128 bits
            aesObject.BlockSize = 128;
            // mode -> CipherMode.*
            aesObject.Mode = CipherMode.CFB;
            // feedback size should be equal to block size
            aesObject.FeedbackSize = 128;
            // set the key
            aesObject.Key = key;
            // set the IV
            aesObject.IV = IV;
            // create an encryptor with the settings provided
            ICryptoTransform encryptor = aesObject.CreateEncryptor();
            byte[] result = null;

            try
            {
                result = encryptor.TransformFinalBlock(byteInput, 0, byteInput.Length);
            }
            catch (Exception e) // if encryption fails
            {
                Console.WriteLine(e.Message); // display the cause
            }

            return result;
        }
        static byte[] decryptWithAES128(string input, byte[] key, byte[] IV)
        {
            // convert input string to byte array
            byte[] byteInput = Encoding.Default.GetBytes(input);

            // create AES object from System.Security.Cryptography
            RijndaelManaged aesObject = new RijndaelManaged();
            // since we want to use AES-128
            aesObject.KeySize = 128;
            // block size of AES is 128 bits
            aesObject.BlockSize = 128;
            // mode -> CipherMode.*
            aesObject.Mode = CipherMode.CFB;
            // feedback size should be equal to block size
            // aesObject.FeedbackSize = 128;
            // set the key
            aesObject.Key = key;
            // set the IV
            aesObject.IV = IV;
            // create an encryptor with the settings provided
            ICryptoTransform decryptor = aesObject.CreateDecryptor();
            byte[] result = null;

            try
            {
                result = decryptor.TransformFinalBlock(byteInput, 0, byteInput.Length);
            }
            catch (Exception e) // if encryption fails
            {
                Console.WriteLine(e.Message); // display the cause
            }

            return result;
        }
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            enc_dec_keys = null;
            sig_ver_keys = null;
            logs.Clear();

            logs.AppendText("Disconnected...\n");
            connected = false;
            terminating = true;
            clientSocket.Close();
            connectButton.Enabled = true;
            loginButton.Enabled = true;

        }

       
    }
}
