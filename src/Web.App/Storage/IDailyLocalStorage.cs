// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Web.App.Games;

namespace Web.App.Storage;

public interface IDailyLocalStorage
{
    DailyGame Get();

    void Set(DailyGame game);
}
