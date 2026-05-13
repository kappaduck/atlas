#!/usr/bin/env dotnet

// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

string json = await File.ReadAllTextAsync("src/Web.App/wwwroot/appsettings.production.json");

string now = $"{DateTime.Now:yyyy.MM.dd}";

Console.WriteLine($"Updating to {now}");
string updatedJson = json.Replace("{version}", now);

await File.WriteAllTextAsync("src/Web.App/wwwroot/appsettings.production.json", updatedJson);
