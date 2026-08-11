using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

string? token = Environment.GetEnvironmentVariable("GITHUB_TOKEN") ?? throw new InvalidOperationException("GITHUB_TOKEN is not set.");
string? repository = Environment.GetEnvironmentVariable("GITHUB_REPOSITORY") ?? throw new InvalidOperationException("GITHUB_REPOSITORY is not set.");

string today = DateTime.UtcNow.ToString("yyyy.MM.dd");

using HttpClient http = new() { BaseAddress = new Uri("https://api.github.com/") };
http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("atlas-milestone-bot", "1.0"));
http.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2026-03-10");

Milestone[] milestones = await http.GetFromJsonAsync($"repos/{repository}/milestones?state=open&per_page=100", GitHubJson.Default.MilestoneArray) ?? [];

Milestone? current = Array.Find(milestones, m => m.Title == "next");

if (current is null)
{
    Console.WriteLine("No open \"next\" milestone found; nothing to close.");
}
else
{
    using HttpResponseMessage patch = await http.PatchAsJsonAsync($"repos/{repository}/milestones/{current.Number}", new MilestoneUpdate(today, "closed"), GitHubJson.Default.MilestoneUpdate);
    patch.EnsureSuccessStatusCode();

    Console.WriteLine($"Closed milestone #{current.Number} as \"{today}\"");
}

using HttpResponseMessage create = await http.PostAsJsonAsync($"repos/{repository}/milestones", new MilestoneCreate("next"), GitHubJson.Default.MilestoneCreate);

if (create.StatusCode == HttpStatusCode.UnprocessableEntity)
{
    Console.WriteLine("A \"next\" milestone already exists; skipped creation.");
}
else
{
    create.EnsureSuccessStatusCode();
    Console.WriteLine("Opened a new \"next\" milestone.");
}

internal sealed record Milestone(int Number, string Title);

internal sealed record MilestoneUpdate(string Title, string State);

internal sealed record MilestoneCreate(string Title);

[JsonSerializable(typeof(Milestone[]))]
[JsonSerializable(typeof(MilestoneUpdate))]
[JsonSerializable(typeof(MilestoneCreate))]
[JsonSourceGenerationOptions(
    GenerationMode = JsonSourceGenerationMode.Default,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class GitHubJson : JsonSerializerContext;
