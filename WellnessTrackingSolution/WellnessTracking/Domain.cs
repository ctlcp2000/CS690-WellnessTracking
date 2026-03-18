namespace WellessTracking;

public class HeadacheOccurence
{
    public DateTime Date { get; set; }
    public int Severity { get; set; }

    public HeadacheOccurence(DateTime date, int severity)
    {
        Date = date;
        Severity = severity;
    }

    public override string ToString()
    {
        return $"{Date.ToString()},{Severity}";
    }
}

public class HeadacheTreatment
{
    public HeadacheOccurence Headache { get; set; }
    public PainMedication Medication { get; set; }

    public HeadacheTreatment(HeadacheOccurence headache, PainMedication medication)
    {
        Headache = headache;
        Medication = medication;
    }

    public override string ToString()
    {
        return $"{Headache.ToString()},{Medication.ToString()}";
    }
}

public class PainMedication
{
    public string Name { get; set; }
    public int Dosage { get; set; }

    public PainMedication(string name, int dosage)
    {
        Name = name;
        Dosage = dosage;
    }

    public override string ToString()
    {
        return $"{Name},{Dosage}";
    }
}

public abstract class Report
{
    public DateTime MinDate { get; set; }
    public DateTime MaxDate { get; set; }
    public DataManager DataManager { get; set; }

    public Report(DateTime minDate, DateTime maxDate, DataManager dataManager)
    {
        MinDate = minDate;
        MaxDate = maxDate;
        DataManager = dataManager; 
    }

    abstract public string GetReportString();
}

public class HeadacheOccurenceReport : Report
{
    public List<HeadacheOccurence> HeadacheOccurenceList { get; set; }

    public HeadacheOccurenceReport(DateTime minDate, DateTime maxDate, DataManager dataManager) : base(minDate, maxDate, dataManager)
    {
        HeadacheOccurenceList = DataManager.GetHeadacheOccurrencesByDate(minDate, maxDate);
    }

    public override string GetReportString()
    {
        var averageSeverity = GetAverageSeverity();
        var reportString = $"You had {HeadacheOccurenceList.Count} headaches between {MinDate} and {MaxDate}. The average severity was {averageSeverity}.\n\n";
        return reportString;        
    }

    private double GetAverageSeverity()
    {
        if (HeadacheOccurenceList.Count == 0)
        {
            return 0.0;
        }

        double totalSeverity = 0;
        foreach (var headache in HeadacheOccurenceList)
        {
            totalSeverity += headache.Severity;
        }

        return totalSeverity / HeadacheOccurenceList.Count;
    }
}

public class PainMedicationReport : Report
{
    // public List<PainMedication> PainMedicationList { get; set; }

    public PainMedicationReport(DateTime minDate, DateTime maxDate, DataManager dataManager) : base(minDate, maxDate, dataManager)
    {
        // TODO: add this
        // PainMedicationList = painMedicationList;
    }

    public override string GetReportString()
    {
        // todo
        return "";
    }
}

public class DataManager
{

    FileSaver HeadachesFileSaver;
    public List<HeadacheOccurence> Headaches { get; }

    public DataManager()
    {
        HeadachesFileSaver = new FileSaver("headaches.txt");
        Headaches = new List<HeadacheOccurence>();
        foreach (var headache in HeadachesFileSaver.GetAllLines())
        {
            var parts = headache.Split(",");
            var date = DateTime.Parse(parts[0]);
            var severity = int.Parse(parts[1]);
            Headaches.Add(new HeadacheOccurence(date, severity));
        }
    }

    public void SynchronizeHeadaches()
    {
        File.Delete("headaches.txt");
        foreach (var headache in Headaches)
        {
            HeadachesFileSaver.AppendLine(headache.ToString());
        }
    }
    
    public void AddHeadache(HeadacheOccurence headache)
    {
        Headaches.Add(headache);
        SynchronizeHeadaches();
    }

    public List<HeadacheOccurence> GetHeadacheOccurrencesByDate(DateTime minDate, DateTime maxDate)
    {
        return Headaches.Where(h => h.Date >= minDate && h.Date <= maxDate).ToList();
    }
}