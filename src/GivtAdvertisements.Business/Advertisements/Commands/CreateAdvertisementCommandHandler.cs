using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements.Commands
{
    public class CreateAdvertisementCommandHandler: IRequestHandler<CreateAdvertisementCommand, Unit>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public CreateAdvertisementCommandHandler(IDynamoDBContext context)
        {
            _dynamoDb = context;
        }
        
        public async Task<Unit> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
        {
            
            return Unit.Value;
        }
    }
}