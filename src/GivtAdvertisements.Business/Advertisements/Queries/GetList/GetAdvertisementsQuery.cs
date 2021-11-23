using System.Collections.Generic;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements
{
    public class GetAdvertisementsQuery: IRequest<List<AdvertisementListItem>>
    {
        
    }
}