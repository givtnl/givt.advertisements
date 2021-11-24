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
            var writeAdvertisementRequest = _dynamoDb.CreateBatchWrite<Advertisement>(new DynamoDBOperationConfig
            {
                OverrideTableName = "Advertisements"
            });

            var currentTime = DateTime.UtcNow;
            
            var metaInfo = new AdvertisementMetaInfo
            {
                Country = request.Country,
                Featured = request.Featured,
                AvailableLanguages = request.AvailableLanguages,
                ChangedDate = currentTime,
                CreationDate = currentTime
            };

            var id = Guid.NewGuid();

            var advertisement = new Advertisement
            {
                AdvertisementId = id,
                PrimaryKey = $"#ADVERTISEMENT",
                Text = request.Text,
                Title = request.Title,
                SortKey = $"#ID#{id.ToString()}",
                MetaInfo = metaInfo,
                ImageUrl = request.ImageUrl
            };

            writeAdvertisementRequest.AddPutItem(advertisement);
            
            var writeLastUpdatedRequest = _dynamoDb.CreateBatchWrite<LastUpdatedAdvertisement>(new DynamoDBOperationConfig
            {
                OverrideTableName = "Advertisements"
            });
            writeLastUpdatedRequest.AddPutItem(new LastUpdatedAdvertisement
            {
                LastUpdated = currentTime,
                PrimaryKey = "#LASTUPDATED",
                SortKey = $"#UPDATED"
            });

            await writeAdvertisementRequest.Combine(writeLastUpdatedRequest).ExecuteAsync(cancellationToken);
            
            return advertisement;
        }
    }
}