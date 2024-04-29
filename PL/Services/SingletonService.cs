using System;

namespace PL.Services
{
    public class SingletonService : ISingeltonService
    {
        public Guid Guid { get; set; }

        public SingletonService()
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
