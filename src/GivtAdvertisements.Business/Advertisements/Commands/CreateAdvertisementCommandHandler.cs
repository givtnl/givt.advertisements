using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using GivtAdvertisements.Business.Advertisements.Models;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements.Commands
{
    public class CreateAdvertisementCommandHandler: IRequestHandler<CreateAdvertisementCommand, Advertisement>
    {
        private readonly IDynamoDBContext _dynamoDb;

        public CreateAdvertisementCommandHandler(IDynamoDBContext context)
        {
            _dynamoDb = context;
        }
        
        public async Task<Advertisement> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
        {
            var writeRequest = _dynamoDb.CreateBatchWrite<Advertisement>(new DynamoDBOperationConfig
            {
                OverrideTableName = "Advertisements"
            });

            var currentTime = DateTime.UtcNow;
            
            var advertisement = new Advertisement();
            advertisement.PrimaryKey = $"#ADVERTISEMENT";
            advertisement.Text = new Dictionary<string, string>();
            advertisement.Text["nl"] = request.Text;
            advertisement.Title = new Dictionary<string, string>();
            advertisement.Title["nl"] = request.Title;

            var metaInfo = new AdvertisementMetaInfo();
            metaInfo.Country = "";
            metaInfo.Featured = false;
            metaInfo.AvailableLanguages = "nl";
            metaInfo.ChangedDate = currentTime;
            metaInfo.CreationDate = currentTime;

            advertisement.SortKey = $"UPDATED#{metaInfo.ChangedDate:yyyy-MM-ddTHH:mm:ss}#ID#{Guid.NewGuid().ToString()}";

            advertisement.MetaInfo = metaInfo;

            writeRequest.AddPutItem(advertisement);

            await writeRequest.ExecuteAsync(cancellationToken);
            
            return advertisement;
        }
    }
}