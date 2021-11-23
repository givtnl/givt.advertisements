using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements.Queries.GetLastUpdated
{
    public class GetLastUpdatedQueryHandler: IRequestHandler<GetLastUpdatedQuery, DateTime?>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public GetLastUpdatedQueryHandler(IDynamoDBContext dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public async Task<DateTime?> Handle(GetLastUpdatedQuery request, CancellationToken cancellationToken)
        {
            var item = await _dynamoDb.LoadAsync<LastUpdatedAdvertisement>("#LASTUPDATED","#UPDATED", new DynamoDBOperationConfig
            {
                OverrideTableName = "Advertisements"
            }, cancellationToken);
            return item?.LastUpdated ?? DateTime.UtcNow;
        }
    }
}