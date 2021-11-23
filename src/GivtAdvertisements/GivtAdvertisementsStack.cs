using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Deployment;
using Constructs;
using Function = Amazon.CDK.AWS.Lambda.Function;
using FunctionProps = Amazon.CDK.AWS.Lambda.FunctionProps;
using ILifecycleRule = Amazon.CDK.AWS.ECR.ILifecycleRule;
using LifecycleRule = Amazon.CDK.AWS.ECR.LifecycleRule;

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
                Proxy = true,
                Handler = lambdaFunction,
            });
            
            var bucket = new Bucket(this, "advertisement-bucket", new BucketProps {
                AccessControl =  BucketAccessControl.PUBLIC_READ,
                Encryption = BucketEncryption.S3_MANAGED,
                EnforceSSL = true,
                PublicReadAccess = true,
                AutoDeleteObjects = true,
                RemovalPolicy = RemovalPolicy.DESTROY,
            });

            var originAccessForBucket = new OriginAccessIdentity(this, "advertisement-bucket-access");

            bucket.GrantRead(originAccessForBucket); 
        }
    }
}
