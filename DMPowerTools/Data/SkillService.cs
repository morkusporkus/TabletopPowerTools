using Microsoft.AspNetCore.Components.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMPowerTools.Data
{
    public class SkillService
    { 
         string _dbPath;
         public string StatusMessage { get; set; }
         private SQLiteAsyncConnection conn;

        public SkillService(string dbPath)
        {
            _dbPath = dbPath;
        }
        private async Task InitAsync()
        {
            if (conn != null)
                return;
            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Skill>();
        }
        public async Task<List<Skill>> GetSkillsAsync()
        {
            await InitAsync();
            return await conn.Table<Skill>().ToListAsync();
        }

        public async Task<Skill> CreateSkillAsync(Skill skill)
        {
            await conn.InsertAsync(skill);
            return skill;
        }
        public async Task<Skill> UpdateSkillAsync(Skill skill)
        {
            await conn.UpdateAsync(skill);
            return skill;
        }
        public async Task<Skill> DeleteSkillAsync(Skill skill)
        {
            await conn.DeleteAsync(skill);
            return skill;
        }
    }
}
