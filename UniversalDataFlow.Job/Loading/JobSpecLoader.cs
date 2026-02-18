using System.Text.Json;
using UniversalDataFlow.Job.Spec;

namespace UniversalDataFlow.Job.Loading;

public static class JobSpecLoader
{
    public static JobSpec Load(string path)
    {
        var json = File.ReadAllText(path);

        return JsonSerializer.Deserialize<JobSpec>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Invalid job.json");
    }
}
