using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements.Commands
{
    public class CreateAdvertisementCommand: IRequest
    {
        public AdvertisementMetaInfo MetaInfo { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
    }
}