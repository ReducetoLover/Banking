using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;
using static Banking.Model.Classes;

namespace Banking.ViewModel
{
    internal class WriteAndRead
    {
        List<APIValutes> api = new List<APIValutes>();
        DBRequests db = new DBRequests();
        public async Task Write(string TBoxSum, string selectedTypeKey, string selectedValuteKey)
        {

            Regex regex = new Regex(@"^[0-9]*[.,]?[0-9]+$");
            if (regex.IsMatch(TBoxSum) && TBoxSum.Length < Convert.ToString(long.MaxValue).Length)
            {
                double Sum = Convert.ToDouble(TBoxSum);



                if (await CheckSum(selectedValuteKey, Sum, selectedTypeKey) == true)
                {
                    DateTime localDateTime = DateTime.Now;
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                    DateTime utcDateTime = TimeZoneInfo.ConvertTime(localDateTime, localTimeZone);

                    await db.BDInsert(selectedValuteKey, Sum, selectedTypeKey, utcDateTime);
                    MessageDialog msg = new MessageDialog("Зачисление/Снятие прошло успешно!", "Оповещение");
                    await msg.ShowAsync();

                }
                else
                {
                    MessageDialog msg = new MessageDialog("Недостаточно средств для снятия", "Оповещение");
                    await msg.ShowAsync();
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("Ошибка ввода данных", "Оповещение");
                await msg.ShowAsync();
            }
        }
        private async Task<Dictionary<string, double>> GetDictValutes()
        {
            Dictionary<string, double> Groups = new Dictionary<string, double>();
            List<Valute> list = await db.BDSelect();
            foreach (var item in list)
            {
                if (Groups.ContainsKey(item.Name))
                {
                    if (item.Type == "Зачисление")
                    {
                        Groups[item.Name] += item.Value;
                    }
                    else if (item.Type == "Снятие")
                    {
                        Groups[item.Name] -= item.Value;
                    }
                }
                else
                {
                    Groups.Add(item.Name, item.Value);
                }
            }
            return Groups;
        }
        private async Task<bool> CheckSum(string Valute, double Value, string Type)
        {
            Dictionary<string, double> groups = await GetDictValutes();
            double AllSumValue = 0;
            foreach (var item in groups)
            {
                AllSumValue += item.Value;
            }
            if (Type == "Снятие")
            {
                if (AllSumValue - Value < 0)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<List<Valute>> GetListHistory()
        {
            return await db.BDSelect();
        }
        public async Task<string> GetSumInValute(double selectedValue, List<APIValutes> ListApi)
        {
            List<Valute> list = await db.BDSelect();
            
            double AllSumValue = 0;
            foreach (var item in list)
            {
                foreach (var itemAPI in ListApi)
                {
                    if (item.Name.Contains(itemAPI.Valute))
                    {
                        if (item.Type == "Зачисление")
                        {
                            AllSumValue += item.Value * itemAPI.Value;
                            break;
                        }
                        else if (item.Type == "Снятие")
                        {
                            AllSumValue -= item.Value * itemAPI.Value;
                            break;
                        }
                    }
                }

            }
            return Math.Round(AllSumValue / selectedValue, 2).ToString();
        }
    }
}
