﻿using System;

namespace OneEvil.OneEvilCoinAPI
{
    public class ValueChangingEventArgs<T> : EventArgs
    {
        public T NewValue { get; private set; }

        internal ValueChangingEventArgs(T newValue)
        {
            NewValue = newValue;
        }
    }
}
