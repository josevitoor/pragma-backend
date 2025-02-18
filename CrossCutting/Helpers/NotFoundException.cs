using System;

namespace CrossCutting.Helpers
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {

        }
    }

}