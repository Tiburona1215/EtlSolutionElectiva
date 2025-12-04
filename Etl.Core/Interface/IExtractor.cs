using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Data
{
    public interface IExtractor<T>
    {
        Task<IEnumerable<T>> ExtractAsync(CancellationToken ct = default);
    }
}
