using CsvHelper;
using CsvHelper.Configuration;
using Etl.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etl.Infrastructure.Estractors
{
    public class CsvExtractor<T> : IExtractor<T>
    {
        private readonly string _filePath;

        public CsvExtractor(string filePath) => _filePath = filePath;

        public async Task<IEnumerable<T>> ExtractAsync(CancellationToken ct = default)
        {
            using var reader = new StreamReader(_filePath);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var csv = new CsvReader(reader, config);

            var list = new List<T>();
            await foreach (var record in csv.GetRecordsAsync<T>().WithCancellation(ct))
                list.Add(record);

            return list;
        }
    }
}
