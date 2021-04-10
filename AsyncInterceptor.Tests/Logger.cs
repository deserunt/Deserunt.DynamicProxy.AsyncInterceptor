using System;
using System.Collections.Generic;

namespace AsyncInterceptor.Tests
{
    public sealed class Logger
    {
        private readonly List<string> _log = new List<string>();
        private readonly object _lock = new object();

        public bool Frozen { get; private set; }

        public string this[int index]
        {
            get
            {
                lock (_lock)
                {
                    if (index >= 0 && index < _log.Count)
                    {
                        return _log[index];
                    }

                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        public int Count 
        {
            get
            {
                lock (_lock)
                {
                    return _log.Count;
                }
            }
        }

        public void Freeze()
        {
            lock (_lock)
            {
                Frozen = true;
            }
        }

        public void Add(string message) 
        {
            lock (_lock)
            {
                if (Frozen == false)
                {
                    _log.Add(message);
                }
            }
        }
    }
}