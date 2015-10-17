using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;
using System.Net;
using System.Text;
using System.IO;

namespace Boozic.Services
{
    public class GCMService :IGCMService
    {
        private readonly IGCMRepository repository;
        private readonly ISettingsService appSettingsService;
        public GCMService(IGCMRepository aRepository)
        {
            repository = aRepository;
            appSettingsService = new SettingsService(new SettingsRepository(new BoozicEntities()));
        }

        public void Add(GCMRegKey aRegKey)
        {
            repository.Add(aRegKey);
        }

        public GCMRegKey GetByDeviceID(string DeviceId)
        {
            return repository.GetByDeviceID(DeviceId);
        }

   public string SendNotification(string message)
    {
        try
        {

            string GoogleAPIKey = appSettingsService.GetGCMServerAPIKey();
            //TODO: update deviceId
            string DeviceID = repository.GetByDeviceID("794e6e0e3487c59e").RegistrationToken; 

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
            Request.Method = "POST";
            Request.KeepAlive = false;
            Request.ContentType = "application/json";
            Request.Headers.Add(string.Format("Authorization: key={0}", GoogleAPIKey));

            string postData = "{ \"registration_ids\": [ \"" + DeviceID + "\" ] ,\"notification\": {\"body\": \"" + message + "\", \"collapse_key\":\"" + message + "\"}}";

           byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            

           ServicePointManager.ServerCertificateValidationCallback += delegate(
                       object
                       sender,
                       System.Security.Cryptography.X509Certificates.X509Certificate
                       pCertificate,
                       System.Security.Cryptography.X509Certificates.X509Chain pChain,
                       System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
           {
               return true;
           };

            //-- Create Stream to Write Byte Array --//
            Stream dataStream = Request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //-- Post a Message --//

            WebResponse Response = Request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) ||
            	ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";
            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;


        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    }
}