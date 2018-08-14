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
        public IQueryable<Abbreviation> Abbreviations => context.Abbreviations;
        public IQueryable<Misc> Miscs => context.Miscs;

        public void AddAbbreviation(Abbreviation abb)
        {
            abb.Abb = abb.Abb.ToUpper();
            abb.CreatedOn = DateTime.Now;
            context.Abbreviations.Add(abb);
            context.SaveChanges();
        }

        public void RemoveAbbreviation(string abb)
        {
            var del = context.Abbreviations.Where(x => x.Abb == abb).First();
            context.Abbreviations.Remove(del);
            context.SaveChanges();
        }

        internal void EditAbbreviation(Abbreviation editAbb)
        {
            //var abb = context.Abbreviations.Find(editAbb.Id);
            //abb.Name = editAbb.Name;
            //abb.Type = editAbb.Type;
            //abb.Area = editAbb.Area;
            context.Abbreviations.Update(editAbb);
            context.SaveChanges();

        }

        internal void AddNetworkElement(NetworkElement newNe)
        {
            switch (newNe.NetworkType)
            {
                case NeType.Core:
                    break;
                case NeType.Access:
                    newNe.Name = $"Access{context.GetNexAccessSequenceValue()}";
                    break;
                case NeType.Remote:
                    newNe.Name = $"Remote{context.GetNextRemoteSequenceValue()}";
                    break;
                case NeType.PBX:
                    newNe.Name = $"PBX{context.GetNextPbxSequenceValue()}";
                    break;
                case NeType.IP_PBX:
                    newNe.Name = $"IP-PBX{context.GetNextPbxSequenceValue()}";
                    break;
                default:
                    break;
            }
            context.NetworkElements.Add(newNe);
            context.SaveChanges();
        }
    }
}
