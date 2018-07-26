using System.Diagnostics;

namespace UnitTests.EventAggregator
{
    public sealed class TestSubscriber
    {
        public void TestCallBack(string payload)
        {
            Debug.WriteLine(payload);
        }
    }
}