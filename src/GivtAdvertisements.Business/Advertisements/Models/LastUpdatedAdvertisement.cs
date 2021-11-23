using System;
using Amazon.DynamoDBv2.DataModel;

namespace GivtAdvertisements.Business.Advertisements.Models
{
    public class LastUpdatedAdvertisement
    {
        [DynamoDBHashKey("PK")]
        public string PrimaryKey { get; set; }
        [DynamoDBRangeKey("SK")]
        public string SortKey { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}