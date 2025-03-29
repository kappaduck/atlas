// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Atlas.Domain.Countries;

public readonly record struct Cca2(string Code)
{
    public static implicit operator string(Cca2 cca2) => cca2.Code;

    public bool Equals(Cca2 other) => Code.Equals(other.Code, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Code.GetHashCode(StringComparison.OrdinalIgnoreCase);

    public override string ToString() => Code;
}
