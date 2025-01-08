using RDF.GL.Common;

namespace RDF.GL.Features.Report;

public class ImportReportsErrors
{
    public static Error DuplicateReport() => new("Import.DuplicateReports", "Duplicate Report");
    public static Error DebitCreditNotMatch() => new("Import.DebitCreditNotMatch", "Debit/Credit not match");
}
