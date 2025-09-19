using System;

namespace Code.Infrastructure
{
    public sealed class FastEvent<T1>
    {
        private Action<T1>[] _snapshot = Array.Empty<Action<T1>>();
        private Action<T1>[] _buffer = Array.Empty<Action<T1>>();
        private int _count;

        public int SubscribersCount => _count;

        public void Clear()
        {
            _snapshot = Array.Empty<Action<T1>>();
            _buffer = Array.Empty<Action<T1>>();
            _count = 0;
        }

        public void Add(Action<T1> handler)
        {
            if (handler == null)
                return;

            EnsureCapacity(_count + 1);
            _buffer[_count++] = handler;
            RebuildSnapshot();
        }

        public bool Remove(Action<T1> handler)
        {
            if (handler == null || _count == 0)
                return false;

            for (int i = 0; i < _count; i++)
            {
                if (_buffer[i] != handler)
                    continue;

                _buffer[i] = _buffer[--_count];
                _buffer[_count] = null;
                RebuildSnapshot();
                return true;
            }

            return false;
        }

        public void Invoke(T1 arg)
        {
            Action<T1>[] snap = _snapshot;
            for (int i = 0; i < snap.Length; i++)
                snap[i]?.Invoke(arg);
        }

        private void EnsureCapacity(int needed)
        {
            if (_buffer.Length >= needed) 
                return;
            
            int newCap = Math.Max(needed, _buffer.Length == 0 ? 4 : _buffer.Length << 1);
            Array.Resize(ref _buffer, newCap);
        }

        private void RebuildSnapshot()
        {
            Action<T1>[] snap = new Action<T1>[_count];
            Array.Copy(_buffer, 0, snap, 0, _count);
            _snapshot = snap;
        }
    }
}