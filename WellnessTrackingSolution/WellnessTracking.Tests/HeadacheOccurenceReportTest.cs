namespace WellessTracking.Tests;

using WellessTracking;

public class HeadacheOccurenceReportTest
{
    DataManager dataManager;

    public HeadacheOccurenceReportTest()
    {
        dataManager = new DataManager();
    }

    [Fact]
    public void TestHeadacheOccurenceReport_Severity()
    {
        dataManager.DeleteAllHeadaches();
        dataManager.AddHeadache(new HeadacheOccurence(new DateTime(2024, 1, 1), 5));
        dataManager.AddHeadache(new HeadacheOccurence(new DateTime(2024, 1, 10), 1));
        HeadacheOccurenceReport report = new HeadacheOccurenceReport(new DateTime(2024, 1, 1), new DateTime(2024, 1, 11), dataManager);

        var minDate = report.MinDate;
        var maxDate = report.MaxDate;
        var averageSeverity = report.GetAverageSeverity();

        Assert.Equal(new DateTime(2024, 1, 1), minDate);
        Assert.Equal(new DateTime(2024, 1, 11), maxDate);
        Assert.Equal(3, averageSeverity);
    }
}