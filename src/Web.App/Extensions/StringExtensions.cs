// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Globalization;
using System.Text;

namespace Web.App.Extensions;

public static class StringExtensions
{
    extension(string)
    {
        internal static string RemoveDiacritics(string value)
        {
            ReadOnlySpan<char> normalized = value.Normalize(NormalizationForm.FormD);

            return string.Create(value.Length, normalized, (buffer, content) =>
            {
                int i = 0;
                foreach (char c in content)
                {
                    if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        buffer[i++] = c;
                }
            });
        }

        internal static int CreateInitials(ReadOnlySpan<char> value, Span<char> initials)
        {
            int i = 0;
            foreach (Range range in value.Split(' '))
            {
                ReadOnlySpan<char> word = value[range];
                initials[i++] = word[0];
            }

            return i;
        }

        internal static bool Lookup(string name, ReadOnlySpan<char> input)
        {
            ReadOnlySpan<char> normalized = RemoveDiacritics(name);

            if (normalized.Contains(input, StringComparison.OrdinalIgnoreCase))
                return true;

            Span<char> initials = stackalloc char[7];
            int length = CreateInitials(normalized, initials);

            return initials[..length].Contains(input, StringComparison.OrdinalIgnoreCase);
        }
    }
}
