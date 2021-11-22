using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using GivtAdvertisements.Business.Advertisements.Models;
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
            var writeRequest = _dynamoDb.CreateBatchWrite<Advertisement>(new DynamoDBOperationConfig
            {
                OverrideTableName = "Advertisements"
            });

            var advertisement = new Advertisement();
            advertisement.Text = new Dictionary<string, string>();
            advertisement.Text["nl"] = request.Text;
            advertisement.Title = new Dictionary<string, string>();
            advertisement.Title["nl"] = request.Title;

            var metaInfo = new AdvertisementMetaInfo();
            metaInfo.Country = "";
            metaInfo.Featured = false;
            metaInfo.AvailableLanguages = "nl";
            metaInfo.ChangedDate = DateTime.Now;
            metaInfo.CreationDate = DateTime.Now;

            advertisement.MetaInfo = metaInfo;

            writeRequest.AddPutItem(advertisement);

            await writeRequest.ExecuteAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}