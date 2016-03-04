﻿using System.Diagnostics;
using System.Threading;

namespace CETech
{
    public class QueueMpmc<T>
    {
        private readonly int _capacityMask;
        private readonly T[] _data;
        private readonly int[] _sequences;

        private int _capacity;
        private int _dequeuePos;
        private int _enqueuePos;


        public QueueMpmc(int capacity)
        {
            _capacityMask = capacity - 1;
            Debug.Assert(0 == (capacity & _capacityMask), "capacity must be power of two");

            _capacity = capacity;
            _data = new T[capacity];

            _sequences = new int[capacity];
            for (var i = 0; i < capacity; ++i)
            {
                _sequences[i] = i;
            }

            _enqueuePos = 0;
            _dequeuePos = 0;
        }

        public int Size
        {
            get
            {
                var e = _enqueuePos & _capacityMask;
                var d = _dequeuePos & _capacityMask;
                return e > d ? e - d : d - e;
            }
        }

        public bool Push(T value)
        {
            var pos = _enqueuePos;

            for (;;)
            {
                var seq = _sequences[pos & _capacityMask];

                var dif = seq - pos;

                if (dif == 0)
                {
                    if (Interlocked.CompareExchange(ref _enqueuePos, pos + 1, pos) == pos)
                    {
                        break;
                    }
                }
                else if (dif < 0)
                {
                    return false;
                }
                else
                {
                    pos = _enqueuePos;
                }
            }

            _data[pos & _capacityMask] = value;
            _sequences[pos & _capacityMask] = pos + 1;

            return true;
        }

        public bool Pop(out T value)
        {
            var pos = _dequeuePos;

            for (;;)
            {
                var seq = _sequences[pos & _capacityMask];

                var dif = seq - (pos + 1);

                if (dif == 0)
                {
                    if (Interlocked.CompareExchange(ref _dequeuePos, pos + 1, pos) == pos)
                    {
                        break;
                    }
                }
                else if (dif < 0)
                {
                    value = default(T);
                    return false;
                }
                else
                {
                    pos = _dequeuePos;
                }
            }

            value = _data[pos & _capacityMask];
            _sequences[pos & _capacityMask] = pos + _capacityMask + 1;

            return true;
        }
    }
}