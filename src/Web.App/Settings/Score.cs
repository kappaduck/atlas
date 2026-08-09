// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Settings;

public sealed record Score(string Key)
{
    public int Streak { get; private set; }
    public int Best { get; private set; }

    public void Increment()
    {
        Streak++;

        if (Streak > Best)
            Best++;
    }

    public void Reset() => Streak = 0;
}
