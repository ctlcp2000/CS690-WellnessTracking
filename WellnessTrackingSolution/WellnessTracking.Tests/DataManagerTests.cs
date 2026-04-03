namespace WellessTracking.Tests;

using WellessTracking;

public class DataManagerTests
{
    DataManager dataManager;

    public DataManagerTests() 
    {
        dataManager = new DataManager("test-headaches2.txt");
    }

    [Fact]
    public void TestRemoveAndAddHeadache()
    {
        dataManager.DeleteAllHeadaches();
        Assert.Empty(dataManager.Headaches);

        dataManager.AddHeadache(new HeadacheOccurence(new DateTime(2026, 4, 10), 1));
        Assert.Single(dataManager.Headaches);
    }

}