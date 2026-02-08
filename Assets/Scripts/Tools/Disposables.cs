using System;
using System.Collections.Generic;

namespace Idler.Tools
{
    public sealed class Disposables : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        public T Register<T>(T service) where T : IDisposable
        {
            _disposables.Add(service);
            return service;
        }

        public void Dispose()
        {
            for (int i = _disposables.Count - 1; i >= 0; i--)
                _disposables[i].Dispose();
            _disposables.Clear();
        }
    }
}
