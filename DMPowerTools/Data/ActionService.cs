using Microsoft.AspNetCore.Components.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPowerTools.Data
{
    public class ActionService
    { 
         string _dbPath;
         public string StatusMessage { get; set; }
         private SQLiteAsyncConnection conn;

        public ActionService(string dbPath)
        {
            _dbPath = dbPath;
        }
        private async Task InitAsync()
        {
            if (conn != null)
                return;
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Action>();
        }
        public async Task<List<Action>> GetMonstersAsync()
        {
            await InitAsync();
            return await conn.Table<Action>().ToListAsync();
        }

        public async Task<Action> CreateActionAsync(Action action)
        {
            await conn.InsertAsync(action);
            return action;
        }
        public async Task<Action> UpdateActionAsync(Action action)
        {
            await conn.UpdateAsync(action);
            return action;
        }
        public async Task<Action> DeleteActionAsync(Action action)
        {
            await conn.DeleteAsync(action);
            return action;
        }
    }
}
