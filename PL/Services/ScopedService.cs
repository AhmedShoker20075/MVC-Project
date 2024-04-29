using System;

namespace PL.Services
{
    public class ScopedService : IScopedService
    {
        public Guid Guid { get ; set ; }
        public ScopedService()
        {
            Guid = Guid.NewGuid(); //Generate New Guid 
        }
        public string GetGuid()
        {
            return Guid.ToString();
        }
        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}
