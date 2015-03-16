using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.IO;

namespace IoTDemo.NetworkTest
{
    public class Program
    {
        static OutputPort ledOnBoard = new OutputPort(Pins.ONBOARD_LED, false);
        public static void Main()
        {
            var potPin = new Microsoft.SPOT.Hardware.AnalogInput(Cpu.AnalogChannel.ANALOG_5);
            //En una futura version la idea es adecuar el nivel de sensibilidad de la luz con el potenciometro
            double limit = 0.70;
            double rawValue;
            var externalLedPortRed = new OutputPort(Pins.GPIO_PIN_D0, false);
            var externalLedPortGreen = new OutputPort(Pins.GPIO_PIN_D7, false);
            var voltagePortluz = new Microsoft.SPOT.Hardware.AnalogInput(Cpu.AnalogChannel.ANALOG_1);

            while (true)
            {
                int potValue = (int)potPin.Read();
                Debug.Print("Potenciómetro: " + potValue.ToString());

                rawValue = voltagePortluz.Read();
                Debug.Print("Voltage de fotoresistencia =" + rawValue.ToString());

                if (potValue == 1)
                {

                    if (rawValue >= limit) // si sobrepasa un umbral 
                    {
                        ledOnBoard.Write(false);
                        Debug.Print("La luz está encendida.");
                        SendEvent("La luz está encendida.");

                        externalLedPortRed.Write(true);
                        externalLedPortGreen.Write(false);
                    }
                    else
                    {
                        ledOnBoard.Write(true);
                        Debug.Print("La luz está apagada.");
                        externalLedPortRed.Write(false); 
                        externalLedPortGreen.Write(true);
                    }
                }
                else
                {
                    Debug.Print("La alarma está apagada.");
                    SendEvent("La alarma está apagada.");
                    externalLedPortRed.Write(false);
                    externalLedPortGreen.Write(false);
                }

                Thread.Sleep(1000);
            }
        }

        public static void SendEvent(string log)
        {

            try
            {
                ledOnBoard.Write(true);

                string json = "{\"Description\":\"" + log + "\"}";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://cloudrsmed.azure-mobile.net/tables/EventLog");
                //{"Description":"Sample"}
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = json.ToCharArray().Length;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        Debug.Print(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Error enviando evento: " + ex.Message);
            }
            finally
            {
                ledOnBoard.Write(false);
            }
        }

    }
}
