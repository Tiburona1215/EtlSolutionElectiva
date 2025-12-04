using Microsoft.EntityFrameworkCore;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Infrastructure.Utils
{
    public class BatchSaver
    {
        public static async Task SaveInBatchesAsync<T>(
           DbContext context,
           IEnumerable<T> entities,
           int batchSize = 500) where T : class
        {
            var list = entities.ToList();

            for (int i = 0; i < list.Count; i += batchSize)
            {
                var batch = list.Skip(i).Take(batchSize);

                context.AddRange(batch);
                await context.SaveChangesAsync();
                context.ChangeTracker.Clear();
            }
        }
    }
}
