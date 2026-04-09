namespace WellessTracking;
using Spectre.Console;
using System.Linq;


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
                "Enter Headache Treatment",
                "Print Pain Medication Report",
                "Exit App"
            ]));


        if (mode == "Enter Headache Occurrence")
        {
            EnterHeadacheOccurrence();
        }
        else if (mode == "Enter Headache Treatment")
        {
            EnterHeadacheTreatment();
        }
        else if (mode == "Print Headache Occurrence Report")
        {
            PrintHeadacheOccurrenceReport();
        }
        else if(mode == "Print Pain Medication Report")
        {
            PrintPainMedicationReport();
        }
        else if (mode == "Exit App")
        {
            Console.WriteLine("Goodbye");
            System.Environment.Exit(0);
        }
    }

    private void PrintPainMedicationReport()
    {
        var headacheMinDate = GetHeadacheDateTime("What's is the minimum [green]date[/] of your pain medication report?", "What is is the minimum time [green]time[/] of your pain medication report?");
        var headacheMaxDate = GetHeadacheDateTime("What's is the maximum [green]date[/] of your pain medication report?", "What is is the maximum time [green]time[/] of your pain medication report?");
        var painMedicationReport = new PainMedicationReport(headacheMinDate, headacheMaxDate, DataManager);
        AnsiConsole.Clear();
        AnsiConsole.Write(painMedicationReport.GetReportString());
        Show();
    }

    private void EnterHeadacheTreatment()
    {
        AnsiConsole.Clear();
        while (true)
        {
            Console.WriteLine("Please enter the date and time of the headache you treated.");
            var finalDateTime = GetHeadacheDateTime("What was the [green]date[/] of your headache treatment?", "What was the [green]time[/] of your headache treatment?", false);

            if (AnsiConsole.Confirm($"Confirm headache treatment at {finalDateTime}?"))
            {
                var headache = DataManager.GetHeadacheOccurrencesByDate(finalDateTime, finalDateTime).FirstOrDefault();

                if (headache == null)
                {
                    AnsiConsole.Clear();
                    AnsiConsole.WriteLine("No headache occurrence found at that date and time. Please enter a valid headache occurrence.");
                    continue;
                }
                else
                {
                    var medication = AnsiConsole.Ask<string>("What medication did you take for this headache?");
                    var dosage = GetDosage();
                    var headacheTreatment = new HeadacheTreatment(headache, new PainMedication(medication, dosage));

                    // store headache treatment
                    DataManager.AddHeadacheTreatment(headacheTreatment);
                    AnsiConsole.Clear();
                    Console.WriteLine("Headache treatment saved");
                    Show();
                }
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

    private void PrintHeadacheOccurrenceReport()
    {
        AnsiConsole.Clear();
        var headacheMinDate = GetHeadacheDateTime("What's is the minimum [green]date[/] of your headache report?", "What is is the minimum time [green]time[/] of your headache report?");
        var headacheMaxDate = GetHeadacheDateTime("What's is the maximum [green]date[/] of your headache report?", "What is is the maximum time [green]time[/] of your headache report?");
        var headacheReport = new HeadacheOccurenceReport(headacheMinDate, headacheMaxDate, DataManager);
        AnsiConsole.Clear();
        var reportString = new Panel(headacheReport.GetReportString());
        AnsiConsole.Write(reportString);
        Show();
    }

    private void EnterHeadacheOccurrence()
    {
        AnsiConsole.Clear();
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

    public static DateTime GetHeadacheDateTime(string datePrompt, string timePrompt, bool clearConsole = true)
    {
        DateTime headacheDate;
        TimeOnly headacheTime;
        if (clearConsole)
        {
            AnsiConsole.Clear();
        }

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
                AnsiConsole.Clear();
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

    public static int GetDosage()
    {
        AnsiConsole.Clear();
        var dosage = AnsiConsole.Prompt(
            new SelectionPrompt<int>()
            .Title("Please select dosage?")
            .AddChoices(new[] 
            {
                10,20,30,40,50
            }));

        if(AnsiConsole.Confirm($"Confirm dosage of {dosage}?"))
        {
            return dosage;
        }
        else
        {
            return GetDosage();
        }
    }
}