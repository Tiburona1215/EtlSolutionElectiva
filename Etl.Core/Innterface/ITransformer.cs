using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Core.Innterface
{
    public interface ITransformer<TInput, TOutput>
    {
        Task<IEnumerable<TOutput>> TransformAsync(IEnumerable<TInput> input, CancellationToken ct = default);
    }
}
