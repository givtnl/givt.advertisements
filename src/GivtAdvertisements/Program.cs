using Amazon.CDK;

namespace GivtAdvertisements
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new GivtAdvertisementsStack(app, "GivtAdvertisementsStack", new StackProps
            {
                StackName = "givt-advertisement-stack"
            });
            app.Synth();
        }
    }
}
