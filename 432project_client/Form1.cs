using System;
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
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        string username;
        string IP;
        int port;
        byte[] halfPass; //half of hash of password

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

                logs.AppendText("Server's public key: " + hexKeys);
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
                logs.AppendText("Server's signing key: " + signHex);
            }

            while (connected)
            {
                try
                {
                    Byte[] buffer = new Byte[512];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.TrimEnd('\0');

                    if (incomingMessage.Contains("Challenge:"))
                    {
                        int index = incomingMessage.IndexOf(":");
                        incomingMessage = incomingMessage.Substring(index + 1);
                        byte[] num = Encoding.Default.GetBytes(incomingMessage);
                        string hexnum = generateHexStringFromByteArray(num);
             
                        logs.AppendText("Challenge num:\n" + hexnum + "\n");

                        byte[] hmacsha256 = applyHMACwithSHA256(incomingMessage, halfPass);
                        string message = "HMAC{"+ username +"}" + Encoding.Default.GetString(hmacsha256);
                        byte[] response = Encoding.Default.GetBytes(message);
                        clientSocket.Send(response);
                    }
                    else if(incomingMessage.Contains("HMAC"))
                    {
                        string message = incomingMessage.Substring(384);
                        string signature = incomingMessage.Substring(0, 384);
                        // signing with RSA 4096
                        byte[] signatureRSA = Encoding.Default.GetBytes(signature);

                        bool verificationResult = verifyWithRSA(message, 3072, sig_ver_keys, signatureRSA);
                        if (verificationResult == true)
                        {
                            if (message.Contains("success"))
                            {
                                logs.AppendText("HMAC valid.\n");
                            }
                            else if (message.Contains("error"))
                            {
                                logs.AppendText("HMAC failed.\n");
                                clientSocket.Close();
                                connected = false;
                            }
                            else
                            {
                                logs.AppendText("Invalid HMAC response.\n");
                                clientSocket.Close();
                                connected = false;
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

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            logs.AppendText("Disconnected...\n");
            connected = false;
            terminating = true;
            clientSocket.Close();
            connectButton.Enabled = true;
        }

    }
}
