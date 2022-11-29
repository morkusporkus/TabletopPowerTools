using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
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
                string line;
               var test = f.Name;
                using (StreamReader reader = new StreamReader(f.OpenReadStream(512000,default)))
                {
                    while ((line = reader.ReadLineAsync().ToString()) != null)
                    {
                        monsters.Add(JsonConvert.DeserializeObject<Monster>(line));
                    }
                }

            }
        }
        public async Task<Monster> CreateMonsterAsync(Monster monster)
        {
            await conn.InsertAsync(monster);
            return monster;
        }
        public async Task<Monster> UpdateMonsterAsync(Monster monster)
        {
            // Update
            await conn.UpdateAsync(monster);
            // Return the updated object
            return monster;
        }
        public async Task<Monster> DeleteMonsterAsync(Monster monster)
        {
            // Delete
            await conn.DeleteAsync(monster);
            return monster;
        }
    }
}
