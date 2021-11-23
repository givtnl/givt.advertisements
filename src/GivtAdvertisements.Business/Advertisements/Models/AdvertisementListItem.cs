using System;
using System.Collections.Generic;

namespace GivtAdvertisements.Business.Advertisements.Models
{
    public class AdvertisementListItem
    {
        public Guid AdvertisementId { get; set; }
        public AdvertisementMetaInfo MetaInfo { get; set; }
        public Dictionary<string, string> Title { get; set; }
        public Dictionary<string, string> Text { get; set; }
        public Dictionary<string, string> ImageUrl { get; set; }
    }
}