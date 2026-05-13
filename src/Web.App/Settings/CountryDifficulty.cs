// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Settings;

[Flags]
public enum CountryDifficulty
{
    None = 0,
    Rotated = 2,
    Mirrored = 4,
    Blur = 8
}
