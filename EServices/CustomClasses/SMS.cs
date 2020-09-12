using EServices.Models.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EServices.CustomClasses
{
    public class SMS
    {
        public string Send(string phone, string message)
        {
            string uri;
            //for send.pk account...
            string MyUsername = "923349185389"; //Your Username At Sendpk.com 
            string MyPassword = "8317"; //Your Password At Sendpk.com 
            string toNumber = phone; //Recepient cell phone number with country code 
            string Masking = "EService_By_Janbaz"; //Your Company Brand Name 
            string MessageText = message;
            SMSDevice smsDevice = SmsDevice.GetInfo();
            string sentFromNumberCode = smsDevice.Device_Code.ToString();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            // uri = "https://sendpk.com/api/sms.php?username=" + MyUsername + "&password=" + MyPassword + "&sender=" + Masking + "&mobile=" + toNumber + "&message=" + MessageText + "";
            try
            {
                uri = "http://zitasms.com/services/send.php?key=be18061a44f922cc4925a56359e18f41&number=" + toNumber + "&message=" + MessageText + "&devices=" + sentFromNumberCode;

                //for GSM modem/Phone
                //     uri = "http://127.0.0.1" + ":" + "9501" + "/httpapi" +
                //"?action=sendMessage" +
                //"&username=" + "admin" +
                //"&password=" + "admin" +
                //"&messageType=" + "SMS:TEXT" +
                //"&recipient=" + toNumber +
                //"&messageData=" + MessageText;

                //for bsms account
                //uri = "http://cp.bsms.pk/api/quick/message?user=923358393136&password=janbazkhan&mask=BrainTEL&to=" + toNumber + "&message=" + MessageText + "";
                //creating web http request 
                WebRequest req = WebRequest.Create(uri);
                WebResponse resp = req.GetResponse();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());
                string msg = sr.ReadToEnd().Trim();
                var convetJsonMessage = JsonConvert.DeserializeObject<message>(msg);
                if (convetJsonMessage.success)
                {
                    return "Message Sent Successfully";
                }
                else
                {
                    return "Faild to send message. make sure you login to the app and phone is connected,also check the sms device list and make sure you activate mobile device. system error: "+msg.ToString();
                }
            }
            catch (Exception e)
            {

                return "Failed to send message . Error: " + e.Message.ToString();
            }
        }
    }
    class message
    {
        public bool success { get; set; }
        public string Error { get; set; }
    }
}