using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public interface IMyAppSettings
    {
        string AzureFileConnectionString { get; set; }
    }

    public class MyAppSettings : IMyAppSettings
    {
        public string AzureFileConnectionString { get; set; }
    }
}
