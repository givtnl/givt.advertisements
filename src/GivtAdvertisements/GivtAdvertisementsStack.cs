using System;
using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.CloudFront.Origins;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.S3;
using Constructs;
using Attribute = Amazon.CDK.AWS.DynamoDB.Attribute;
using Function = Amazon.CDK.AWS.Lambda.Function;
using FunctionProps = Amazon.CDK.AWS.Lambda.FunctionProps;
using ILifecycleRule = Amazon.CDK.AWS.ECR.ILifecycleRule;
using LifecycleRule = Amazon.CDK.AWS.ECR.LifecycleRule;

namespace GivtAdvertisements
{
    public sealed class GivtAdvertisementsStack : Stack
    {
        internal GivtAdvertisementsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var dockerRepository = new Repository(this, $"{id}-repository", new RepositoryProps
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

            
            var table = new Table(this, $"{id}-table", new TableProps
            {
                BillingMode = BillingMode.PAY_PER_REQUEST,
                PartitionKey = new Attribute { Name = "PK", Type = AttributeType.STRING },
                SortKey = new Attribute {Name = "SK", Type = AttributeType.STRING},
                Stream = StreamViewType.NEW_AND_OLD_IMAGES,
                Encryption = TableEncryption.AWS_MANAGED,
                TableName = "Advertisements-bjorn",
                RemovalPolicy = RemovalPolicy.DESTROY,
            });
            
            var lambdaFunction = new Function(this, $"{id}-lambda", new FunctionProps
            {
                Handler = Handler.FROM_IMAGE,
                Code = Code.FromEcrImage(dockerRepository),
                MemorySize = 4096,
                Timeout = Duration.Seconds(10),
                Runtime = Runtime.FROM_IMAGE,
                LogRetention = RetentionDays.ONE_DAY,
                FunctionName = $"{id}-api"
            });
            
            table.GrantFullAccess(lambdaFunction);
            
            var apiGateWay = new LambdaRestApi(this, $"{id}-lambda-api", new LambdaRestApiProps
            {
                EndpointTypes = new EndpointType[] {EndpointType.REGIONAL},
                Proxy = true,
                Handler = lambdaFunction,
            });
            
            var bucket = new Bucket(this, $"{id}-images-bucket", new BucketProps {
                AccessControl =  BucketAccessControl.PRIVATE,
                Encryption = BucketEncryption.S3_MANAGED,
                EnforceSSL = true,
                AutoDeleteObjects = true,
                RemovalPolicy = RemovalPolicy.DESTROY,
            });
            
            var originAccessForBucket = new OriginAccessIdentity(this, $"{id}-access");
            
            bucket.GrantRead(originAccessForBucket);

            var apiUri = $"{apiGateWay.RestApiId}.execute-api.{this.Region}.amazonaws.com";

            var distribution = new Distribution(this, $"{id}-cloudfront", new DistributionProps
            {
                PriceClass = PriceClass.PRICE_CLASS_100,
                HttpVersion = HttpVersion.HTTP2,
                DefaultBehavior = new BehaviorOptions
                {
                    Origin = new HttpOrigin(apiUri, new HttpOriginProps
                    {
                        ProtocolPolicy = OriginProtocolPolicy.HTTPS_ONLY,
                        OriginPath = "/prod",
                    }),
                    AllowedMethods = AllowedMethods.ALLOW_ALL,
                    OriginRequestPolicy = OriginRequestPolicy.CORS_CUSTOM_ORIGIN,
                },
            });
            
            distribution.AddBehavior("images/*", new S3Origin(bucket, new S3OriginProps
            {
                OriginAccessIdentity = originAccessForBucket
            }), new BehaviorOptions
            {
                Compress = true,
                AllowedMethods = AllowedMethods.ALLOW_GET_HEAD_OPTIONS,
                ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS,
                CachedMethods = CachedMethods.CACHE_GET_HEAD_OPTIONS,
                CachePolicy = CachePolicy.CACHING_OPTIMIZED
            });
        }
    }
}
