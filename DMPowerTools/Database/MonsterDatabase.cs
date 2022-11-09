using DMPowerTools.Database;
using SQLite;

public class MonsterItemDatabase
{
    SQLiteAsyncConnection Database;
   
    public async Task<List<MonsterItem>> GetMonstersAsync()
    {
        await Init();
        return await Database.Table<MonsterItem>().ToListAsync();
    }
    public async Task<List<AbilityItem>> GetAbilitysAsync()
    {
        await Init();
        return await Database.Table<AbilityItem>().ToListAsync();
    }
    public async Task<List<SkillItem>> GetSkillsAsync()
    {
        await Init();
        return await Database.Table<SkillItem>().ToListAsync();
    }
    public async Task<List<ActionItem>> GetActionsAsync()
    {
        await Init();
        return await Database.Table<ActionItem>().ToListAsync();
    }
    public async Task<MonsterItem> GetMonsterAsync(int id)
    {
        await Init();
        return await Database.Table<MonsterItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }
    public async Task<SkillItem> GetSkillAsync(int id)
    {
        await Init();
        return await Database.Table<SkillItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }
    public async Task<ActionItem> GetActionAsync(int id)
    {
        await Init();
        return await Database.Table<ActionItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }
    public async Task<AbilityItem> GetAbilityAsync(int id)
    {
        await Init();
        return await Database.Table<AbilityItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
    }
    public async Task<int> SaveMonsterAsync(MonsterItem item)
    {
        await Init();
        if (item.ID != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }
    public async Task<int> SaveActionAsync(ActionItem item)
    {
        await Init();
        if (item.ID != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }
    public async Task<int> SaveSkillAsync(SkillItem item)
    {
        await Init();
        if (item.ID != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }
    public async Task<int> SaveAbilityAsync(AbilityItem item)
    {
        await Init();
        if (item.ID != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        await Database.CreateTableAsync<ActionItem>();
        await Database.CreateTableAsync<SkillItem>();
        await Database.CreateTableAsync<AbilityItem>();
        await Database.CreateTableAsync<MonsterItem>();

    }
}