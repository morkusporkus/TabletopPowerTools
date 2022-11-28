using Microsoft.AspNetCore.Components.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPowerTools.Data
{
    public class MonsterService
    { 
         string _dbPath;
         public string StatusMessage { get; set; }
         private SQLiteAsyncConnection conn;

        public MonsterService(string dbPath)
        {
            _dbPath = dbPath;
        }
        private async Task InitAsync()
        {
            if (conn != null)
                return;
            // Create database and WeatherForecast Table
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Monster>();
        }
        public async Task<List<Monster>> GetMonstersAsync()
        {
            await InitAsync();
            return await conn.Table<Monster>().ToListAsync();
        }
        public async Task UploadMonstersAsync(IList<IBrowserFile> files)
        {
            var monsters = new List<Monster>();
            foreach (IBrowserFile f in files)
            {
                var monster = new Monster();
                f.OpenReadStream();
            }
        }
        public async Task<Monster> CreateForecastAsync(Monster monster)
        {
            await conn.InsertAsync(monster);
            return monster;
        }
        public async Task<Monster> UpdateForecastAsync(Monster monster)
        {
            // Update
            await conn.UpdateAsync(monster);
            // Return the updated object
            return monster;
        }
        public async Task<Monster> DeleteForecastAsync(Monster monster)
        {
            // Delete
            await conn.DeleteAsync(monster);
            return monster;
        }
    }
}
