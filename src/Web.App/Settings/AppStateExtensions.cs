// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Text;

namespace Web.App.Settings;

internal static class AppStateExtensions
{
    private const int Pixels = 30;
    private const int Step = 5;

    extension(FlagDifficulty difficulty)
    {
        internal string Css(int guesses)
        {
            return difficulty switch
            {
                FlagDifficulty.Blur => $"filter: blur({CalculatePixels(guesses)}px)",
                FlagDifficulty.Invert => "filter: invert(1)",
                FlagDifficulty.Shift => "filter: hue-rotate(90deg)",
                FlagDifficulty.Grayscale => "filter: grayscale(1)",
                _ => string.Empty
            };
        }
    }

    extension(CountryDifficulty difficulty)
    {
        internal string Css(int guesses)
        {
            if (difficulty == CountryDifficulty.None)
                return string.Empty;

            StringBuilder builder = new();

            if ((difficulty & CountryDifficulty.Blur) != 0)
                builder.Append($"filter: blur({CalculatePixels(guesses)}px);");

            List<string> transforms = [];

            if ((difficulty & CountryDifficulty.Mirrored) != 0)
                transforms.Add("scaleX(-1)");

            if ((difficulty & CountryDifficulty.Rotated) != 0)
                transforms.Add($"rotate({Random.Shared.Next(0, 360)})");

            if (transforms.Count > 0)
                builder.Append($"transform: {string.Join(' ', transforms)};");

            return builder.ToString();
        }
    }

    private static int CalculatePixels(int guesses) => Pixels - (guesses * Step);
}
