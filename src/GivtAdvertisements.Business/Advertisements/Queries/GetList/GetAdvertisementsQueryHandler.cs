using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements
{
    public class GetAdvertisementsQueryHandler: IRequestHandler<GetAdvertisementsQuery, List<Advertisement>>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public GetAdvertisementsQueryHandler(IDynamoDBContext dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task<List<Advertisement>> Handle(GetAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            var query = _dynamoDb.QueryAsync<Advertisement>("#ADVERTISEMENT", new DynamoDBOperationConfig {OverrideTableName = "Advertisements"});
            var itemList = await query.GetRemainingAsync(cancellationToken);
            return itemList;
        }
    }
}