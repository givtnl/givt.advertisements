using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements
{
    public class GetAdvertisementsQueryHandler: IRequestHandler<GetAdvertisementsQuery, List<AdvertisementListItem>>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public GetAdvertisementsQueryHandler(IDynamoDBContext dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task<List<AdvertisementListItem>> Handle(GetAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            var query = _dynamoDb.QueryAsync<Advertisement>("#ADVERTISEMENT", new DynamoDBOperationConfig {OverrideTableName = "Advertisements"});
            var items = await query.GetRemainingAsync(cancellationToken);
            var itemList = items.Select(x => new AdvertisementListItem()
            {
                Text = x.Text,
                Title = x.Title,
                AdvertisementId = Guid.Parse(x.SortKey.Split("#")[2]),
                ImageUrl = x.ImageUrl,
                MetaInfo = x.MetaInfo
            }).ToList();
            return itemList;
        }
    }
}