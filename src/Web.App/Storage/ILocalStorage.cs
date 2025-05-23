// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

namespace Web.App.Storage;

public interface ILocalStorage
{
    T? GetItem<T>(string key);

    void RemoveItem(string key);

    void SetItem<T>(string key, T value);
}
