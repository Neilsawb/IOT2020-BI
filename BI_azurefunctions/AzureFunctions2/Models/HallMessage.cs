using Microsoft.Azure.Cosmos.Table;
using System.Text;

namespace AzureFunctions.Models
{
    public class HallMessage : TableEntity
    {
        public string Deviceid { get; set; }
        public string Type { get; set; }
        public long EpochTime { get; set; }

        public string School { get; set; }
        public string Student { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int HallValue { get; set; }
        public string State { get; set; }
        public int StateCode { get; set; }

    }
}