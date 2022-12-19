using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMPowerTools.Data.Extensions
{
    public static class DbSetExtensions
    {

        public static EntityEntry<TEnt> AddIfNotExists<TEnt, TKey>(this DbSet<TEnt> dbSet, TEnt entity, Func<TEnt, TKey> predicate) where TEnt : class
        {
            var exists = dbSet.Any(c => predicate(entity).Equals(predicate(c)));
            return exists
                ? null
                : dbSet.Add(entity);
        }
    }
}
