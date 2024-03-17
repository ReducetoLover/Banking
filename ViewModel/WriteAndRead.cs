using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using static Banking.Model.Classes;

namespace Banking.ViewModel
{
    internal class WriteAndRead
    {
        DBRequests db = new DBRequests();
        public async Task Write(string TBoxSum,string selectedTypeKey, string selectedValuteKey)
        {
            
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(TBoxSum) && TBoxSum.Length < Convert.ToString(long.MaxValue).Length)
            {
                double Sum = Convert.ToInt64(TBoxSum);


               
                if (await CheckSum(selectedValuteKey, Sum, selectedTypeKey) == true)
                {
                    DateTime localDateTime = DateTime.Now;
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                    DateTime utcDateTime = TimeZoneInfo.ConvertTime(localDateTime, localTimeZone);

                    await db.BDInsert(selectedValuteKey, Sum, selectedTypeKey, utcDateTime);
                    MessageDialog msg = new MessageDialog("Снятие прошло успешно!", "Оповещение");
                    await msg.ShowAsync();

                }
                else
                {
                    MessageDialog msg = new MessageDialog("Недостаточно средств для снятия", "Ошибка ввода данных");
                    await msg.ShowAsync();
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("Введено слишком большое число", "Ошибка ввода данных");
                await msg.ShowAsync();
            }
        }
        private async Task<bool> CheckSum(string Valute, double Value, string Type)
        {
            Dictionary<string, double> groups = new Dictionary<string, double>();
            List<Valute> list = await db.BDSelect();
            foreach (var item in list)
            {
                if (groups.ContainsKey(item.Name))
                {
                    if (item.Type == "Зачисление")
                    {
                        groups[item.Name] += item.Value;
                    }
                    else if (item.Type == "Снятие")
                    {
                        groups[item.Name] -= item.Value;
                    }

                }
                else
                {
                    groups.Add(item.Name, item.Value);
                }
            }
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
    }
}
