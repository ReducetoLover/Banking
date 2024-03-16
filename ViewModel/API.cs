using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Banking.Model.Classes;

namespace Banking.ViewModel
{
    internal class API
    {
        private List<APIValutes> listValute = new List<APIValutes>();
        public List<APIValutes> ListValute
        {
            get { return listValute; }
            set
            {
                listValute = value;
                OnPropertyChanged(nameof(ListValute));
            }
        }
        private int selected;
        public int Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public API()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propetryName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propetryName));
        }
        public async Task<List<APIValutes>> GetValutes()
        {
            try
            {
                var client = new HttpClient();
                var url = "https://www.cbr-xml-daily.ru/daily_json.js";
                var response = await client.GetAsync(url);

                string contentString = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(contentString);
                var attributes = obj["Valute"];

                foreach (JProperty attributeProperty in attributes)
                {
                    var attribute = attributes[attributeProperty.Name];
                    APIValutes valute = new APIValutes()
                    {
                        Valute = attributeProperty.Name,
                        Value = (long)attribute["Value"]
                    };
                    listValute.Add(valute);
                }
            }
            catch
            {
                listValute = null;
            }
            return listValute;

        }
    }
}
