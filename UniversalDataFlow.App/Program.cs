using System.Security.Cryptography;
using System.Text;
using UniversalDataFlow.Core.Data;
using UniversalDataFlow.Core.Fields;
using UniversalDataFlow.Core.Pipeline;
using UniversalDataFlow.Core.Runtime;
using UniversalDataFlow.Core.Transformations;
using UniversalDataFlow.Core.Validation.Actions;
using UniversalDataFlow.Core.Validation.Field;
using UniversalDataFlow.Core.Validation.Row;
using UniversalDataFlow.Core.Validation.Set;
using UniversalDataFlow.IO.Csv;
using UniversalDataFlow.IO.Encoding;
using UniversalDataFlow.Job.Factory;


/* verze 3.0  */
using UniversalDataFlow.Job.Runtime;

// ⬇⬇⬇ TADY ⬇⬇⬇
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

if (args.Length != 1)
{
    Console.WriteLine("Usage: UniversalDataFlow <job.json>");
    return;
}

new JobRunner().Run(args[0]);



/* verze 2.0 - 

// STEP 1 - Schema + FieldRegistry (per CSV)
// --- ORG schema ---
var orgFields = new FieldRegistry();
var orgCode = orgFields.Register("OrgCode", typeof(int));
var orgManager = orgFields.Register("OrgManagerID", typeof(int));
var orgParent = orgFields.Register("SuperiorOrgID", typeof(int));

// --- PERS schema ---
var persFields = new FieldRegistry();
var persNum = persFields.Register("PersNum", typeof(int));
var persOrg = persFields.Register("OrgNumber", typeof(int));


// STEP 2 - PipelineSpec + PipelineRuntime (per dataset)
// --- ORG pipeline ---
var orgSpec = new PipelineSpec(
    orgFields,
    new TransformationRegistry(),
    fieldValidations: Array.Empty<FieldValidationStep>(),
    rowValidations: Array.Empty<RowValidationStep>(),
    dataSetRules: Array.Empty<IDataSetRule>() // zde NE
);

var orgMetadata = new PipelineValidator().Validate(orgSpec);
var orgPipelineRuntime = new PipelineRuntime(orgMetadata);

// --- PERS pipeline ---
var persSpec = new PipelineSpec(
    persFields,
    new TransformationRegistry(),
    fieldValidations: Array.Empty<FieldValidationStep>(),
    rowValidations: Array.Empty<RowValidationStep>(),
    dataSetRules: Array.Empty<IDataSetRule>() // zde NE
);

var persMetadata = new PipelineValidator().Validate(persSpec);
var persPipelineRuntime = new PipelineRuntime(persMetadata);


// STEP 3 - CSV vstupy (DataRow kolekce)
var encoding = new TextEncoding(System.Text.Encoding.UTF8);
var csvSource = new CsvSource(encoding);

// načtení CSV (byte[] → DataRow)
var orgRows = csvSource.Read(
    File.ReadAllBytes("OrgData.csv"),
    orgFields);

var persRows = csvSource.Read(
    File.ReadAllBytes("PersData.csv"),
    persFields);


// STEP 4 - Dataset-level rules (VZTAHY)
var dataSetRules = new IDataSetRule[]
{
    new PersonOrgExistsRule(
        persDataset: "pers",
        orgDataset: "org",
        persId: persNum,
        orgRef: persOrg,
        orgId: orgCode),

    new OrgManagerExistsRule(
        orgDataset: "org",
        persDataset: "pers",
        orgId: orgCode,
        manager: orgManager,
        persId: persNum),

    new OrgHierarchyRule(
        orgDataset: "org",
        id: orgCode,
        parent: orgParent)
};

// STEP 5 - JobRuntime (místo registrace rules)
var job = new DataFlowJobRuntime(
    pipelines: new Dictionary<string, PipelineRuntime>
    {
        ["org"] = orgPipelineRuntime,
        ["pers"] = persPipelineRuntime
    },
    dataSetRules: dataSetRules
);

// STEP 6 - Spuštění jobu
job.Execute(
    new Dictionary<string, IEnumerable<DataRow>>
    {
        ["org"] = orgRows,
        ["pers"] = persRows
    }
);
*/

/* verze 1.0
// --- CSV input ---
var csv = """
Name,Age
Alice,30
Bob,-5
,20
""";

var encoding = new TextEncoding(Encoding.UTF8);
var source = new CsvSource(encoding);

// --- schema ---
var fields = new FieldRegistry();
var name = fields.Register("Name", typeof(string));
var age = fields.Register("Age", typeof(int));

// --- validations ---
var fieldValidations = new[]
{
    new FieldValidationStep(
        new AgeNonNegativeRule(age),
        new SetIntFieldToZeroAction(age))
};

var rowValidations = new[]
{
    new RowValidationStep(
        new NameRequiredRule(name),
        new SkipRowAction())
};

// --- pipeline ---
var spec = new PipelineSpec(
    fields,
    new TransformationRegistry(),
    fieldValidations,
    rowValidations);

var validator = new PipelineValidator();
var metadata = validator.Validate(spec);
var runtime = new PipelineRuntime(metadata);

// --- read ---
var rows = source.Read(
    Encoding.UTF8.GetBytes(csv),
    fields);

// --- run ---
var result = runtime.Execute(rows);

// --- write ---
var writer = new CsvWriter(encoding);
var output = writer.Write(
    result.AcceptedRows,
    new[] { name, age });

Console.WriteLine(Encoding.UTF8.GetString(output));
*/