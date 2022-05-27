using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class PoiDataParser : MonoBehaviour
{
    [Serializable]
    public class Poi
    {
        public int id;
        public string title;
        public object subtitle;
        public string lat;
        public string lon;
        public string height_offset;
        public bool indoor;
        public string indoor_id;
        public int floor_id;
        public UserData user_data;
        public string tags;
    }
    public class UserData
    {
        public string title;
        public string subtitle;
        public string tags;
        public double lat;
        public double lon;
    }
    [Serializable]
    public class PoiSet
    {
        public int id;
        public string name;
        public DateTime updated_at;
        public List<object> api_key_permissions;
    }

    string DevToken = "430395ea01577a54f11e3e1ffb2fa0324a52fe1a2eb0ffcf6bd429a732a1e73fe9c8d358777a6480";
    string PoiSetID = "11756";

    string allSetUrl = "https://poi.wrld3d.com/v1.1/poisets/?token=";
    string PoisetInSetfirstHalf = "https://poi.wrld3d.com/v1.1/poisets/"; // the sid
    string PoisetInSetsecondHalf = "/pois/?token="; // then this half

    [SerializeField]
    public List<Poi> AllPois = new List<Poi>();

    public AiSpawner spawner;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(4);
        //using (UnityWebRequest webRequest = UnityWebRequest.Get(allSetUrl + DevToken))
        using (UnityWebRequest webRequest = UnityWebRequest.Get(PoisetInSetfirstHalf + PoiSetID + PoisetInSetsecondHalf + DevToken))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = allSetUrl.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                if(spawner)
                {
                    spawner.PoisRecieved(false);
                }
            }
            else
            {
                string json = webRequest.downloadHandler.text;
                AllPois = JsonConvert.DeserializeObject<List<Poi>>(json);

                spawner.PoisRecieved(true);

            }
        }

    }
}
