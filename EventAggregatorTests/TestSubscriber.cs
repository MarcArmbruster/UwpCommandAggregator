using System.Diagnostics;
namespace EventAggregatorTests
{
    public sealed class TestSubscriber
    {
        public void TestCallBack(string payload)
        {
            Debug.WriteLine(payload);
        }
    }
}