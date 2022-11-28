using Microsoft.AspNetCore.Components.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPowerTools.Data
{
    public class AbilityService
    { 
         string _dbPath;
         public string StatusMessage { get; set; }
         private SQLiteAsyncConnection conn;

        public AbilityService(string dbPath)
        {
            _dbPath = dbPath;
        }
        private async Task InitAsync()
        {
            if (conn != null)
                return;
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Monster>();
        }
        public async Task<List<Ability>> GetMonstersAsync()
        {
            await InitAsync();
            return await conn.Table<Ability>().ToListAsync();
        }

        public async Task<Ability> CreateAbilityAsync(Ability ability)
        {
            await conn.InsertAsync(ability);
            return ability;
        }
        public async Task<Ability> UpdateAbilityAsync(Ability ability)
        {
            // Update
            await conn.UpdateAsync(ability);
            // Return the updated object
            return ability;
        }
        public async Task<Ability> DeleteAbilityAsync(Ability ability)
        {
            // Delete
            await conn.DeleteAsync(ability);
            return ability;
        }
    }
}
