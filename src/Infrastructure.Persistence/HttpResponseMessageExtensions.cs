// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using System.Net;

namespace Infrastructure.Persistence;

internal static class HttpResponseMessageExtensions
{
    extension(HttpResponseMessage response)
    {
        internal void ThrowIfFailed()
        {
            if (response.IsSuccessStatusCode || response.StatusCode is HttpStatusCode.NotModified)
                return;

            throw new HttpRequestException(FormatMessage(response), null, response.StatusCode);
        }
    }

    private static string FormatMessage(HttpResponseMessage response)
    {
        const string message = "Response status code does not indicate success:";

        return string.IsNullOrWhiteSpace(response.ReasonPhrase)
            ? $"{message} {response.StatusCode}."
            : $"{message} {response.StatusCode} ({response.ReasonPhrase}).";
    }
}
