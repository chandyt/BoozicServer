using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using Boozic.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Boozic.Repositories;

namespace Boozic.Services
{
    public class LocationService : ILocationService
    {
        private readonly ISettingsService appSettingsService;
        public LocationService()
        {
            appSettingsService = new SettingsService(new SettingsRepository(new BoozicEntities()));
            
        }

        /// <summary>
        /// This Function generates a list of StoreInfo objects.
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <param name="Radius">Radius in Miles</param>
        /// <returns> List of StoreInfo Objects</returns>
        /// 
        public List<StoreInfo> getStores(double Latitude, double Longitude, double Radius)
        {
            List<StoreInfo> lstSI = new List<StoreInfo>();


            //TODO: Add the storeID

            // Generating Parameters for Google Places API
            NameValueCollection GP_Details = new NameValueCollection();
            String googleAPIKey = appSettingsService.GetGoogleAPIKey();
            GP_Details.Add("key", googleAPIKey); 
            GP_Details.Add("sensor", "false");
            GP_Details.Add("location", Latitude + "," + Longitude);
            GP_Details.Add("radius", Convert.ToString(Radius * 1609)); // Radius is converted to meters for the API
            GP_Details.Add("types", "liquor_store");

            string GP_Url = "json?" + string.Join("&", GP_Details.AllKeys.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(GP_Details[x]))));

            HttpClient httpGPClient = new HttpClient();
            httpGPClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/search/");
            HttpResponseMessage GP_Response = httpGPClient.GetAsync(GP_Url).Result;
            if (GP_Response.IsSuccessStatusCode)
            {

                string googleResult = GP_Response.Content.ReadAsStringAsync().Result.ToString();
                // parsing Data out of JSON result from google places API
                JObject json = Newtonsoft.Json.Linq.JObject.Parse(googleResult);
                JArray results = (JArray)json["results"];
                for (int i = 0; i < results.Count; i++)
                {
                    StoreInfo si = new StoreInfo();
                    JToken location = json["results"][i]["geometry"]["location"];
                    si.Latitude = location["lat"].Value<double>();
                    si.Longitude = location["lng"].Value<double>();

                    si.StoreName = json["results"][i]["name"].Value<string>();
                    si.StoreAddress = json["results"][i]["vicinity"].Value<string>();

                    JToken hours = json["results"][i]["opening_hours"];
                    //if (hours != null)
                       // si.IsOpenNow = hours["open_now"].Value<bool>();

                    // Generating parameters for Google Matrix API to find the distance and the duration
                    //NameValueCollection GM_Details = new NameValueCollection();
                    //GM_Details.Add("key", googleAPIKey);
                    //GM_Details.Add("origins", Latitude + "," + Longitude);
                    //GM_Details.Add("destinations", si.Latitude + "," + si.Longitude);
                    //GM_Details.Add("units", "imperial");

                    //string GM_Url = "json?" + string.Join("&", GM_Details.AllKeys.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(GM_Details[x]))));

                    //HttpClient httpGMClient = new HttpClient();
                    //httpGMClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/");
                    //HttpResponseMessage GM_Response = httpGMClient.GetAsync(GM_Url).Result;
                    //if (GM_Response.IsSuccessStatusCode)
                    //{
                    //    //Parsing out the distance and duration out of the JSON from Google Matrix API.
                    //    string googleMatrixResult = GM_Response.Content.ReadAsStringAsync().Result.ToString();

                    //    JObject GM_Json = Newtonsoft.Json.Linq.JObject.Parse(googleMatrixResult);
                    //    JArray rows = (JArray)GM_Json["rows"];
                    //    if (rows.Count > 0)
                    //    {
                    //        si.Distance = GM_Json["rows"][0]["elements"][0]["distance"]["value"].Value<double>();
                    //        si.Duration = GM_Json["rows"][0]["elements"][0]["duration"]["value"].Value<double>();
                    //    }
                    //}
                    Dictionary<String, Double> distanceResult = getDistanceAndTime(Latitude, Longitude, si.Latitude, si.Longitude);
                    si.Distance = distanceResult["Distance"];
                    
                    //si.Duration = distanceResult["Duration"];

                    lstSI.Add(si);
                }



            }
            lstSI = lstSI.OrderBy(o => o.Distance).ToList();
            return lstSI;
        }

        public Dictionary<string, Double> getDistanceAndTime(double sourceLatitude,double sourceLongitude,double destLatitude,double destLongitude)
        {
            // Generating parameters for Google Matrix API to find the distance and the duration
            String googleAPIKey = appSettingsService.GetGoogleAPIKey();
            NameValueCollection GM_Details = new NameValueCollection();
            GM_Details.Add("key", googleAPIKey);
            GM_Details.Add("origins", sourceLatitude + "," + sourceLongitude);
            GM_Details.Add("destinations", destLatitude + "," + destLongitude);
            GM_Details.Add("units", "imperial");

            string GM_Url = "json?" + string.Join("&", GM_Details.AllKeys.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(GM_Details[x]))));

            HttpClient httpGMClient = new HttpClient();
            httpGMClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/");
            HttpResponseMessage GM_Response = httpGMClient.GetAsync(GM_Url).Result;
            Dictionary<String, Double> Result= new Dictionary<String, Double>();
            if (GM_Response.IsSuccessStatusCode)
            {
                //Parsing out the distance and duration out of the JSON from Google Matrix API.
                string googleMatrixResult = GM_Response.Content.ReadAsStringAsync().Result.ToString();

                JObject GM_Json = Newtonsoft.Json.Linq.JObject.Parse(googleMatrixResult);
                JArray rows = (JArray)GM_Json["rows"];
                if (rows.Count > 0)
                {
                    try
                    {

                        Result.Add("Distance", GM_Json["rows"][0]["elements"][0]["distance"]["value"].Value<double>() * 0.000621371);
                        Result.Add("Duration", GM_Json["rows"][0]["elements"][0]["duration"]["value"].Value<double>());
                    }
                    catch (Exception ex)
                    {
                        Result.Add("Distance", 9999);
                        Result.Add("Duration", 9999);
                    }
                }
                else
                {
                    Result.Add("Distance", 9999);
                    Result.Add("Duration", 9999);
                }
            }
            return Result;
        }

        public double distance(double lat1, double lon1, double lat2, double lon2)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            return (Math.Round(dist, 2));
        }


        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }


        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}