namespace WellessTracking.Tests;

using WellessTracking;

public class FileSaverTests
{
    FileSaver fileSaver;
    string testFileName;

    public FileSaverTests() {
        testFileName = "test-doc.txt";
        File.Delete(testFileName);
        fileSaver = new FileSaver(testFileName);
    }
}