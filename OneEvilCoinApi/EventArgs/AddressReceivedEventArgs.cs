using System;

namespace OneEvil.OneEvilCoinAPI
{
    public class AddressReceivedEventArgs : EventArgs
    {
        public string Address { get; private set; }

        internal AddressReceivedEventArgs(string address)
        {
            Address = address;
        }
    }
}
