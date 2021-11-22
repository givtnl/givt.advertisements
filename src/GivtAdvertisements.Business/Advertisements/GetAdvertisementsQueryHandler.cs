using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements
{
    public class GetAdvertisementsQueryHandler: IRequestHandler<GetAdvertisementsQuery, Unit>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public GetAdvertisementsQueryHandler(IDynamoDBContext dynamoDb)
        {
            _dynamoDb = dynamoDb;
        }
        
        public Task<Unit> Handle(GetAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}