using System;

namespace GivtAdvertisements.Business.Advertisements.Models
{
    public class AdvertisementMetaInfo
    {
        public DateTime CreationDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public bool Featured { get; set; }
        public string AvailableLanguages { get; set; }
        public string Country { get; set; }
    }
}