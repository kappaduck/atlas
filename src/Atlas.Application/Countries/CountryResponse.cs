// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Countries;

public sealed record CountryResponse(string Cca2, string Name, ResourcesResponse Resources);
