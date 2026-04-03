namespace WellessTracking.Tests;

using WellessTracking;

public class HeadacheTest
{
    [Fact]
    public void TestHeadacheOccurenceToString()
    {
        HeadacheOccurence headache = new HeadacheOccurence(new DateTime(2026, 4, 10), 2);
        Assert.Equal(2, headache.Severity);
        Assert.Equal(new DateTime(2026, 4, 10), headache.Date);
    }
}