#!/usr/bin/env dotnet

// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

string json = await File.ReadAllTextAsync("src/Web.App/wwwroot/appsettings.production.json");

string updatedJson = json.Replace("{version}", $"{DateTime.Now:yyyy.MM.dd}");

await File.WriteAllTextAsync("src/Web.App/wwwroot/appsettings.production.json", updatedJson);
