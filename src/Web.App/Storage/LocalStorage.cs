// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Web.App.Storage;

[ExcludeFromCodeCoverage]
internal sealed partial class LocalStorage : ILocalStorage
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public T? GetItem<T>(string key)
    {
        string? json = Get(key);

        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json, _options);
    }

    public void RemoveItem(string key) => Remove(key);

    public void SetItem<T>(string key, T value)
    {
        string json = JsonSerializer.Serialize(value, _options);

        Set(key, json);
    }

    [JSImport("globalThis.localStorage.getItem")]
    private static partial string? Get(string key);

    [JSImport("globalThis.localStorage.removeItem")]
    private static partial void Remove(string key);

    [JSImport("globalThis.localStorage.setItem")]
    private static partial void Set(string key, string value);
}
