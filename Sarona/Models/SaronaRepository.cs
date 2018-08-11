using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sarona.Models;

namespace Sarona.Models
{
    public class SaronaRepository
    {
        private SaronaContext context;
        public SaronaRepository(SaronaContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Abbreviation> Abbreviations => context.Abbreviations;
        

        public void AddAbbreviation(Abbreviation abb)
        {
            abb.Abb = abb.Abb.ToUpper();
            abb.CreatedOn = DateTime.Now;
            context.Abbreviations.Add(abb);
            context.SaveChanges();
        }

        public void RemoveAbbreviation(long id)
        {
            context.Abbreviations.Remove(new Abbreviation() { Id = id });
            context.SaveChanges();
        }
    }
}
