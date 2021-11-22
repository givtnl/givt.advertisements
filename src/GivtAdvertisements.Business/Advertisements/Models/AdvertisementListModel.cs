using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace GivtAdvertisements.Business.Advertisements.Models
{
    public class AdvertisementListModel
    {
        [DynamoDBHashKey("PK")]
        public string PrimaryKey { get; set; }
        [DynamoDBRangeKey("SK")]
        public string SortKey { get; set; }
        public Guid AdvertisementId { get; set; }
        public AdvertisementMetaInfo MetaInfo { get; set; }
        public Dictionary<string, string> Title { get; set; }
        public Dictionary<string, string> Text { get; set; }
        public Dictionary<string, string> ImageUrl { get; set; }
    }
}