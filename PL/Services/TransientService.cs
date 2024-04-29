using System;

namespace PL.Services
{
    public class TransientService : ITransientService
    {
        public Guid Guid { get; set; }

        public TransientService()
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
