using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements
{
    public class GetAdvertisementsQueryHandler: IRequestHandler<GetAdvertisementsQuery, List<AdvertisementListModel>>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public GetAdvertisementsQueryHandler(IDynamoDBContext dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public Task<List<AdvertisementListModel>> Handle(GetAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}