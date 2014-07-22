using System;

namespace OneEvil.OneEvilCoinAPI
{
    public class TransactionReceivedEventArgs : EventArgs
    {
        public Transaction Transaction { get; private set; }

        internal TransactionReceivedEventArgs(Transaction transaction)
        {
            Transaction = transaction;
        }
    }
}
