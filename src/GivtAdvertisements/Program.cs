using Amazon.CDK;

namespace GivtAdvertisements
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            var customStackName = "bjorn";
            new GivtAdvertisementsStack(app, $"advertisements-{customStackName}", new StackProps
            {
                StackName = $"advertisements-{customStackName}"
            });
            app.Synth();
        }
    }
}
