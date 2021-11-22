using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Constructs;

namespace GivtAdvertisements
{
    public class GivtAdvertisementsStack : Stack
    {
        internal GivtAdvertisementsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var dockerRepository = new Repository(this, "advertisement-repository", new RepositoryProps
            {
                RemovalPolicy = RemovalPolicy.DESTROY,
                LifecycleRules = new ILifecycleRule[]
                {
                    new LifecycleRule
                    {
                        MaxImageCount = 2,
                        TagStatus = TagStatus.ANY
                    }
                }
            });
            
            var table = new Table(this, "advertisement-table", new TableProps
            {
                BillingMode = BillingMode.PAY_PER_REQUEST,
                PartitionKey = new Attribute { Name = "PK", Type = AttributeType.STRING },
                SortKey = new Attribute {Name = "SK", Type = AttributeType.STRING},
                Stream = StreamViewType.NEW_AND_OLD_IMAGES,
                Encryption = TableEncryption.AWS_MANAGED,
                TableName = "Advertisements",
                RemovalPolicy = RemovalPolicy.DESTROY,
            });

            var lambdaFunction = new Function(this, "advertisement-lambda", new FunctionProps
            {
                Handler = Handler.FROM_IMAGE,
                Code = Code.FromEcrImage(dockerRepository),
                MemorySize = 4096,
                Timeout = Duration.Seconds(10),
                Runtime = Runtime.FROM_IMAGE,
                LogRetention = RetentionDays.ONE_DAY,
                FunctionName = "givt-advertisement-api"
            });
            
            table.GrantFullAccess(lambdaFunction);
            
            var apiGateWay = new LambdaRestApi(this, "advertisement-lambda-api", new LambdaRestApiProps
            {
                EndpointTypes = new EndpointType[] {EndpointType.REGIONAL},
                Handler = lambdaFunction,
            });
        }
    }
}
