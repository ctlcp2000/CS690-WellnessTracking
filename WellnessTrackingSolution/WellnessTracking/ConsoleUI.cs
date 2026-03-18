namespace WellessTracking;
using Spectre.Console;


public class ConsoleUI
{
    DataManager DataManager;

    public ConsoleUI()
    {
        DataManager = new DataManager();
    }

    public void Show()
    {
        var mode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select mode")
            .AddChoices(
            [
                "Enter Headache Occurrence",
                "Print Headache Occurrence Report",
                "Exit App"
            ]));


        if (mode == "Enter Headache Occurrence")
        {
            while (true)
            {
                var finalDateTime = GetHeadacheDateTime("What was the [green]date[/] of your headache?", "What was the [green]time[/] of your headache?");

                if (AnsiConsole.Confirm($"Confirm headache occurrence at {finalDateTime}?"))
                {
                    var severity = GetHeadacheSeverity();

                    // store headache occurrence
                    DataManager.AddHeadache(new HeadacheOccurence(finalDateTime, severity));
                    AnsiConsole.Clear();
                    Console.WriteLine("Headache occurrence saved");
                    Show();
                }
                else if(AnsiConsole.Confirm("Do you want to re-enter the date and time?"))
                {
                    Console.WriteLine("Okay, try again.");
                    continue;
                }
                else
                {
                    Console.WriteLine("Thanks!");
                    System.Environment.Exit(0);
                }
            }

        }
        else if (mode == "Print Headache Occurrence Report")
        {
            var headacheMinDate = GetHeadacheDateTime("What's is the minimum [green]date[/] of your headache report?", "What is is the minimum time [green]time[/] of your headache report?");
            var headacheMaxDate = GetHeadacheDateTime("What's is the maximum [green]date[/] of your headache report?", "What is is the maximum time [green]time[/] of your headache report?");
            var headacheReport = new HeadacheOccurenceReport(headacheMinDate, headacheMaxDate, DataManager);
            AnsiConsole.Clear();
            AnsiConsole.Write(headacheReport.GetReportString());
            Show();
        }
        else if (mode == "Exit App")
        {
            Console.WriteLine("Goodbye");
            System.Environment.Exit(0);
        }
    }

    public static DateTime GetHeadacheDateTime(string datePrompt, string timePrompt)
    {
        DateTime headacheDate;
        TimeOnly headacheTime;
        AnsiConsole.Clear();

        while (true)
        {
            var headacheDateString = AnsiConsole.Ask<string>($"{datePrompt} (yyyy-MM-dd format like 2025-05-01)");
            if(!DateTime.TryParseExact(headacheDateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime headacheDateTemp))
            {
                Console.WriteLine("Invalid date format. Use yyyy-MM-dd format.");
            }
            else
            {
                headacheDate = headacheDateTemp;
                break;
            }
        }
        while (true)
        {
            var headacheTimeString = AnsiConsole.Ask<string>($"{timePrompt} (hh:mm tt format like 02:30 PM)");
            if(!TimeOnly.TryParseExact(headacheTimeString, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out TimeOnly headacheTimeTemp))
            {
                Console.WriteLine("Invalid date format. Use hh:mm tt format.");
            }
            else
            {
                headacheTime = headacheTimeTemp;
                break;
            }
        }
        return headacheDate.Add(headacheTime.ToTimeSpan());
    }

    public static int GetHeadacheSeverity()
    {
        AnsiConsole.Clear();
        var severity = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title("Please select severity?")
            .AddChoices(new[] 
            {
                1,2,3,4,5
            }));

        if(AnsiConsole.Confirm($"Confirm severity of {severity}?"))
        {
            return severity;
        }
        else
        {
            return GetHeadacheSeverity();
        }
    }
}