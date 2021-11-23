using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using MediatR;

namespace GivtAdvertisements.Business.Advertisements.Queries.GetLastUpdated
{
    public class GetLastUpdatedQueryHandler: IRequestHandler<GetLastUpdatedQuery, DateTime>
    {
        private readonly IDynamoDBContext _dynamoDb;
        private readonly IAmazonDynamoDB _client;

        public GetLastUpdatedQueryHandler(IDynamoDBContext dynamoDb, IAmazonDynamoDB client)
        {
            _dynamoDb = dynamoDb;
            _client = client;
        }
        
        public async Task<DateTime> Handle(GetLastUpdatedQuery request, CancellationToken cancellationToken)
        {
            var query = await _client.QueryAsync(new QueryRequest("Advertisements")
            {
                Limit = 1,
                ScanIndexForward = false,
                Select = Select.SPECIFIC_ATTRIBUTES,
                AttributesToGet = new List<string> {"SK"},
                KeyConditions = new Dictionary<string, Condition>
                {
                    {
                        "PK", new Condition
                        {
                            ComparisonOperator = ComparisonOperator.EQ,
                            AttributeValueList = new List<AttributeValue>
                            {
                                new ("#ADVERTISEMENT")
                            }
                        }
                    }
                }
            }, cancellationToken);
            var item = query.Items.Select(x => x.Values).FirstOrDefault();
            if (item != null)
            {
                return DateTime.Parse(item.First().S.Split("#")[1]);
            }
            return DateTime.Now;
        }
    }
}