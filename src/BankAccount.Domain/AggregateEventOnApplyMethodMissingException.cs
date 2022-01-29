using System;

namespace Bank.Domain
{
    public class AggregateEventOnApplyMethodMissingException : Exception
    {
        public AggregateEventOnApplyMethodMissingException(string msg) : base(msg)
        {
            
        }
    }
}