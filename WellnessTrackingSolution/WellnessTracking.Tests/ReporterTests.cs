namespace WellessTracking.Tests;

using WellessTracking;

public class ReportTests
{
    List<PassengerData> sampleData;

    public ReportTests() {
        sampleData = new List<PassengerData>();
    }

    [Fact]
    public void Test_FindBusiestStop_Just2stops()
    {
        Stop sampleStop = new Stop("MyStop");
		Loop sampleLoop = new Loop("MyLoop");
		Driver sampleDriver = new Driver("Sample");
		PassengerData samplePassengerData = new PassengerData(5,sampleStop,sampleLoop,sampleDriver);
        sampleData.Add(samplePassengerData);
        
        Stop sampleStop2 = new Stop("MyStop2");
        PassengerData samplePassengerData2 = new PassengerData(6,sampleStop2,sampleLoop,sampleDriver);
        sampleData.Add(samplePassengerData2);

        var result = Reporter.FindBusiestStop(sampleData);
        Assert.Equal("MyStop2",result.Name);
    }

    [Fact]
    public void Test_FindBusiestStop_Just2stops_MoreData()
    {
        sampleData.Add(new PassengerData(4,new Stop("MyStop"),new Loop("MyLoop"),new Driver("MyDriver")));
        sampleData.Add(new PassengerData(5,new Stop("MyStop2"),new Loop("MyLoop2"),new Driver("MyDriver2")));
        sampleData.Add(new PassengerData(2,new Stop("MyStop"),new Loop("MyLoop"),new Driver("MyDriver")));
        
        var result = Reporter.FindBusiestStop(sampleData);
        Assert.Equal("MyStop",result.Name);
    }

}