namespace WellessTracking.Tests;

using WellessTracking;

public class DataManagerTests
{
    DataManager dataManager;

    public DataManagerTests() {
        File.WriteAllText("stops.txt","One"+Environment.NewLine+"Two"+Environment.NewLine+"Three"+Environment.NewLine+"Four"+Environment.NewLine+"Five");
        dataManager = new DataManager();
    }
}