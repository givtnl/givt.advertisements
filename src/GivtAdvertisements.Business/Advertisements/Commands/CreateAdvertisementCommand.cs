using System;
using System.Collections.Generic;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements.Commands
{
    public class CreateAdvertisementCommand: IRequest<Advertisement>
    {
        public bool Featured { get; set; }
        public string AvailableLanguages { get; set; }
        public Dictionary<string, string> Text { get; set; }
        public Dictionary<string, string> Title { get; set; }
        public Dictionary<string, string> ImageUrl { get; set; }
        public string Country { get; set; }
    }
}