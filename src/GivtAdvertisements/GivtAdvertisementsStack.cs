using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Constructs;

namespace GivtAdvertisements
{
    public class GivtAdvertisementsStack : Stack
    {
        internal GivtAdvertisementsStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var table = new Table(this, "advertisement-table", new TableProps
            {
                BillingMode = BillingMode.PAY_PER_REQUEST,
                PartitionKey = new Attribute { Name = "id", Type = AttributeType.STRING },
            });
        }
    }
}
