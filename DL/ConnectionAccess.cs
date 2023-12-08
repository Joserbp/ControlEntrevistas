using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    public enum DataSource
    {
        Principal
    }
    public class DataSourceProvider : IDataSourceProvider
    {
        private readonly IConfiguration _configuration;
        public DataSource CurrentDataSource { get; set; }

        public DataSourceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return CurrentDataSource switch
            {
                DataSource.Principal => _configuration.GetConnectionString("Dev")!,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    public interface IDataSourceProvider
    {
        DataSource CurrentDataSource { set; }
        string GetConnectionString();
    }
}
