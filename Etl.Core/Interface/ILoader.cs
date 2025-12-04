using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Innterface
{
    public interface ILoader<T>
    {
        Task LoadAsync(IEnumerable<T> data, CancellationToken ct = default);
    }
}
