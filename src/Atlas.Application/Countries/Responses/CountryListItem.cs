// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Application.Countries.Responses;

public sealed record CountryListItem(string Name, string Continent, Uri Map, Uri Flag);
