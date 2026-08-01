#!/usr/bin/env dotnet

// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

const string directory = "countries";
const string shapeFile = "country.svg";

const int size = 1000;
const double padding = 0.02;
const double tolerance = 0.35;

const double minAreaRatio = 0.001;
const double maxDistance = 40;
const int scale = 50;

Uri source = new($"https://raw.githubusercontent.com/nvkelso/natural-earth-vector/master/geojson/ne_{scale}m_admin_0_map_units.geojson");
string cache = Path.Combine(Path.GetTempPath(), $"ne_{scale}m_admin_0_map_units.geojson");

Dictionary<string, string> aliases = new(StringComparer.OrdinalIgnoreCase)
{
    ["Kosovo"] = "xk",
    ["Northern Cyprus"] = "cy",
    ["Somaliland"] = "so",
};

HashSet<string> manual = [with(StringComparer.OrdinalIgnoreCase), "bv", "gi", "um"];

if (!File.Exists(cache))
{
    Console.WriteLine($"Downloading {source}");

    using HttpClient client = new();
    await using Stream download = await client.GetStreamAsync(source);
    await using FileStream target = File.Create(cache);

    await download.CopyToAsync(target);
}

HashSet<string> expected = Directory.EnumerateDirectories(directory)
                                    .Select<string, string>(Path.GetFileName)
                                    .Where(cca2 => !manual.Contains(cca2))
                                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

foreach (string skipped in manual.Order(StringComparer.Ordinal))
    Console.WriteLine($"{skipped}: skipped, keeping manual shape");

await using FileStream stream = File.OpenRead(cache);
FeatureCollection collection = (await JsonSerializer.DeserializeAsync(stream, JsonContext.Default.FeatureCollection))!;

Dictionary<string, List<Polygon>> shapes = [with(StringComparer.OrdinalIgnoreCase)];

foreach (Feature feature in collection.Features)
{
    if (feature.Geometry is null || Resolve(feature.Properties) is not string cca2 || !expected.Contains(cca2))
        continue;

    if (!shapes.TryGetValue(cca2, out List<Polygon>? polygons))
        shapes[cca2] = polygons = [];

    polygons.AddRange(ReadPolygons(feature.Geometry));
}

foreach ((string cca2, List<Polygon> polygons) in shapes.OrderBy(s => s.Key, StringComparer.Ordinal))
{
    Normalize(polygons);

    List<Polygon> kept = Filter(polygons, out int dropped);
    string path = BuildPath(kept);

    if (path.Length == 0)
    {
        Console.WriteLine($"{cca2}: no geometry left after filtering, skipped");
        continue;
    }

    string file = Path.Combine(directory, cca2, shapeFile);
    await File.WriteAllTextAsync(file, BuildSvg(path));

    Console.WriteLine($"{cca2}: {kept.Count} polygon(s), {dropped} dropped, {path.Length / 1024d:0.0} KB");
}

foreach (string missing in expected.Except(shapes.Keys, StringComparer.OrdinalIgnoreCase).Order(StringComparer.Ordinal))
    Console.WriteLine($"{missing}: not found in Natural Earth at 1:{scale}m, needs a manual shape");

string? Resolve(Properties properties)
{
    if (properties.IsoA2Eh is { Length: 2 } isoEh && isoEh != "-9")
        return isoEh.ToLowerInvariant();

    if (properties.IsoA2 is { Length: 2 } iso && iso != "-9")
        return iso.ToLowerInvariant();

    return properties.Name is string name && aliases.TryGetValue(name, out string? alias) ? alias : null;
}

static List<Polygon> ReadPolygons(Geometry geometry)
{
    return geometry.Type switch
    {
        "Polygon" => [ReadPolygon(geometry.Coordinates)],
        "MultiPolygon" => [.. geometry.Coordinates.EnumerateArray().Select(ReadPolygon)],
        _ => [],
    };
}

static Polygon ReadPolygon(JsonElement element)
{
    List<List<Point>> rings = [.. element.EnumerateArray().Select(ReadRing)];

    return new Polygon(rings[0], [.. rings.Skip(1)]);
}

static List<Point> ReadRing(JsonElement element)
{
    List<Point> points = [];

    foreach (JsonElement position in element.EnumerateArray())
    {
        double x = position[0].GetDouble();
        double y = position[1].GetDouble();

        points.Add(new Point(x, y));
    }

    if (points is [Point first, .., Point last] && first == last)
        points.RemoveAt(points.Count - 1);

    return points;
}

static void Normalize(List<Polygon> polygons)
{
    IEnumerable<Point> points = polygons.SelectMany(p => p.Exterior);

    if (points.Max(p => p.X) - points.Min(p => p.X) <= 180)
        return;

    foreach (Polygon polygon in polygons)
    {
        foreach (List<Point> ring in polygon.Rings)
        {
            for (int i = 0; i < ring.Count; i++)
            {
                if (ring[i].X < 0)
                    ring[i] = ring[i] with { X = ring[i].X + 360 };
            }
        }
    }
}

static List<Polygon> Filter(List<Polygon> polygons, out int dropped)
{
    Polygon largest = polygons.MaxBy(Area)!;

    double threshold = Area(largest) * minAreaRatio;
    Point origin = Center(largest);

    List<Polygon> kept = [.. polygons.Where(p => Area(p) >= threshold && Distance(Center(p), origin) <= maxDistance)];
    dropped = polygons.Count - kept.Count;

    return kept;

    static double Area(Polygon polygon)
    {
        List<Point> ring = polygon.Exterior;
        double area = 0;

        for (int i = 0, j = ring.Count - 1; i < ring.Count; j = i++)
            area += (ring[j].X * ring[i].Y) - (ring[i].X * ring[j].Y);

        // Longitude degrees shrink towards the poles, so weight by the latitude.
        return Math.Abs(area / 2) * Math.Cos(double.DegreesToRadians(Center(polygon).Y));
    }

    static Point Center(Polygon polygon)
    {
        List<Point> ring = polygon.Exterior;

        return new Point((ring.Min(p => p.X) + ring.Max(p => p.X)) / 2, (ring.Min(p => p.Y) + ring.Max(p => p.Y)) / 2);
    }

    static double Distance(Point left, Point right)
    {
        double x = (left.X - right.X) * Math.Cos(double.DegreesToRadians((left.Y + right.Y) / 2));
        double y = left.Y - right.Y;

        return Math.Sqrt((x * x) + (y * y));
    }
}

static string BuildPath(List<Polygon> polygons)
{
    List<List<Point>> rings = [.. polygons.SelectMany(p => p.Rings).Select(r => r.ConvertAll(Project))];

    double minX = rings.SelectMany(r => r).Min(p => p.X);
    double maxX = rings.SelectMany(r => r).Max(p => p.X);
    double minY = rings.SelectMany(r => r).Min(p => p.Y);
    double maxY = rings.SelectMany(r => r).Max(p => p.Y);

    double inner = size * (1 - (padding * 2));
    double factor = inner / Math.Max(Math.Max(maxX - minX, maxY - minY), double.Epsilon);

    double offsetX = ((size - ((maxX - minX) * factor)) / 2) - (minX * factor);
    double offsetY = ((size - ((maxY - minY) * factor)) / 2) + (maxY * factor);

    StringBuilder builder = new();

    foreach (List<Point> ring in rings)
    {
        List<Point> fitted = ring.ConvertAll(p => new Point((p.X * factor) + offsetX, offsetY - (p.Y * factor)));
        List<Point> simplified = Simplify(fitted, tolerance);

        if (simplified.Count < 3)
            continue;

        for (int i = 0; i < simplified.Count; i++)
            builder.Append(i == 0 ? 'M' : 'L').Append(Format(simplified[i].X)).Append(' ').Append(Format(simplified[i].Y));

        builder.Append('Z');
    }

    return builder.ToString();

    static Point Project(Point point)
    {
        double latitude = double.DegreesToRadians(Math.Clamp(point.Y, -85, 85));

        return new Point(double.DegreesToRadians(point.X), Math.Log(Math.Tan((Math.PI / 4) + (latitude / 2))));
    }

    static string Format(double value) => Math.Round(value, 2).ToString("0.##", CultureInfo.InvariantCulture);
}

static List<Point> Simplify(List<Point> points, double tolerance)
{
    if (points.Count < 3)
        return points;

    bool[] keep = new bool[points.Count];
    keep[0] = keep[^1] = true;

    Stack<(int First, int Last)> segments = new();
    segments.Push((0, points.Count - 1));

    while (segments.TryPop(out (int First, int Last) segment))
    {
        double furthest = 0;
        int index = -1;

        for (int i = segment.First + 1; i < segment.Last; i++)
        {
            double distance = Perpendicular(points[i], points[segment.First], points[segment.Last]);

            if (distance <= furthest)
                continue;

            furthest = distance;
            index = i;
        }

        if (index < 0 || furthest <= tolerance)
            continue;

        keep[index] = true;
        segments.Push((segment.First, index));
        segments.Push((index, segment.Last));
    }

    return [.. points.Where((_, i) => keep[i])];

    static double Perpendicular(Point point, Point start, Point end)
    {
        double dx = end.X - start.X;
        double dy = end.Y - start.Y;
        double length = (dx * dx) + (dy * dy);

        if (length == 0)
            return Math.Sqrt(((point.X - start.X) * (point.X - start.X)) + ((point.Y - start.Y) * (point.Y - start.Y)));

        double ratio = Math.Clamp((((point.X - start.X) * dx) + ((point.Y - start.Y) * dy)) / length, 0, 1);

        double x = point.X - (start.X + (ratio * dx));
        double y = point.Y - (start.Y + (ratio * dy));

        return Math.Sqrt((x * x) + (y * y));
    }
}

static string BuildSvg(string path)
{
    return $"""
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {size} {size}" fill="currentColor" fill-rule="evenodd">
          <path d="{path}" />
        </svg>

        """;
}

internal readonly record struct Point(double X, double Y);

internal sealed record Polygon(List<Point> Exterior, List<List<Point>> Holes)
{
    public IEnumerable<List<Point>> Rings => [Exterior, .. Holes];
}

[JsonSerializable(typeof(FeatureCollection))]
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class JsonContext : JsonSerializerContext;

internal sealed record FeatureCollection
{
    public required IEnumerable<Feature> Features { get; init; }
}

internal sealed record Feature
{
    public required Properties Properties { get; init; }

    public Geometry? Geometry { get; init; }
}

internal sealed record Properties
{
    [JsonPropertyName("ISO_A2_EH")]
    public string? IsoA2Eh { get; init; }

    [JsonPropertyName("ISO_A2")]
    public string? IsoA2 { get; init; }

    [JsonPropertyName("NAME")]
    public string? Name { get; init; }
}

internal sealed record Geometry
{
    public required string Type { get; init; }

    public required JsonElement Coordinates { get; init; }
}
