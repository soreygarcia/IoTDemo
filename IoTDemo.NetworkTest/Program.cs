using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.IO;
using EmbeddedLab.NetduinoPlus.Day5.IO;
using IoTDemo.NetworkTest.DTOs;

namespace IoTDemo.NetworkTest
{
    public class Program
    {
        public static void Main()
        {
            // Directly start logging, no need to create any instance of Logger class
            Logger.LogToFile = true;    // if false it will only do Debug.Print()
            Logger.Append = true;       // will append the information to existing if any
            Logger.PrefixDateTime = true; // add a time stamp on each Log call. Note: Netduino time is not same as clock time.

            // any number of arguments can be passed. They will appended by a white space
            Logger.Log("Hello World in SD Card");
            Debug.Print(Logger.LogFilePath);

            Debug.Print("Enviando mensaje...");
            SendEvent("Hello World");
            Thread.Sleep(Timeout.Infinite);
        }

        public static void SendEvent(string log)
        {
            try
            {
                EventLog eventLog = new EventLog() { Description = log };
                string json = Json.NETMF.JsonSerializer.SerializeObject(eventLog);

                Debug.Print("Log enviado");
                Debug.Print(json);

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://cloudrsmed.azure-mobile.net/tables/EventLog");
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json";
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.ContentLength = json.ToCharArray().Length;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Debug.Print(result);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Error enviando evento: " + ex.Message);
                Logger.Log("Error: " + ex.Message);
            }
        }
    }
}
