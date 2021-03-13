using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BalizaFacil.Models;

[assembly: Xamarin.Forms.Dependency(typeof(BalizaFacil.Services.LogDataStore))]
namespace BalizaFacil.Services
{
    public class LogDataStore : IDataStore<LogBaliza>
    {
        List<LogBaliza> items = new List<LogBaliza>();

        public LogDataStore()
        {
        }

        public async Task<bool> AddItemAsync(LogBaliza item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(string id, LogBaliza item)
        {
            var _item = items.Where((LogBaliza arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((LogBaliza arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<LogBaliza> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<LogBaliza>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
