using System;
using System.Collections.Generic;
using System.Linq;

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
        public IQueryable<Exchange> Exchanges => context.Exchanges;
        public IQueryable<Customer> Customers => context.Customers;
        public IQueryable<Misc> Miscs => context.Miscs;
        public IQueryable<NetworkElement> NetworkElements => context.NetworkElements;

        public void AddExchange(Exchange exch)
        {
            exch.Abb = exch.Abb.ToUpper();
            exch.CreatedOn = DateTime.Now;
            context.Exchanges.Add(exch);
            context.SaveChanges();
        }

        public Exchange RemoveExchange(string abb)
        {
            var exch = context.Exchanges.Where(x => x.Abb == abb).FirstOrDefault();
            context.Exchanges.Remove(exch);
            context.SaveChanges();
            return exch;
        }

        public Exchange RemoveExchange(int id)
        {
            var exch = context.Exchanges.Find(id);
            context.Exchanges.Remove(exch);
            context.SaveChanges();
            return exch;
        }

        internal void AddCustomer(Customer newCustomer)
        {
            newCustomer.CreatedOn = DateTime.Now;
            newCustomer.Abb = newCustomer.Abb.ToUpperInvariant();
            context.Customers.Add(newCustomer);
            context.SaveChanges();
        }

        internal void EditExchange(Exchange exch)
        {
            context.Abbreviations.Update(exch);
            context.SaveChanges();
        }

        internal void AddRangeNetworkElement(IEnumerable<NetworkElement> range)
        {
            context.NetworkElements.AddRange(range);
            context.SaveChanges();
        }

        internal void AddNetworkElement(NetworkElement newNe)
        {
            switch (newNe.NetworkType)
            {
                case NeType.Core:
                    newNe.Name = newNe.Name.ToUpperInvariant();
                    break;
                case NeType.Access:
                    newNe.Name = $"ACCESS{context.GetNexAccessSequenceValue()}";
                    break;
                case NeType.Remote:
                    newNe.Name = $"REMOTE{context.GetNextRemoteSequenceValue()}";
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

        internal void DeleteNetworkElement(NetworkElement ne)
        {
            context.NetworkElements.Remove(ne);
            context.SaveChanges();
        }

        internal void EditNetworkElement(NetworkElement ne)
        {
            context.NetworkElements.Update(ne);
            context.SaveChanges();
        }

        internal void AddLink(Link newLink)
        {
            newLink.CreatedOn = DateTime.Now;
            LinkDirection direction = LinkDirection.Bothway;
            switch (newLink.Direction)
            {
                case LinkDirection.Incoming:
                    direction = LinkDirection.Outgoing;
                    break;
                case LinkDirection.Outgoing:
                    direction = LinkDirection.Incoming;
                    break;
                case LinkDirection.Bothway:
                    direction = LinkDirection.Bothway;
                    break;
            }
            context.Database.BeginTransaction();
            context.Links.Add(newLink);
            context.SaveChanges();

            var otherLink = new Link()
            {
                Channels = newLink.Channels,
                CreatedOn = newLink.CreatedOn,
                Direction = direction,
                End1Id = newLink.End2Id,
                End2Id = newLink.End1Id,
                OtherLinkId = newLink.Id,
                Remark = newLink.Remark,
                Type = newLink.Type
            };

            newLink.OtherLinkId = otherLink.Id;
            context.Links.Add(otherLink);
            context.Links.Update(newLink);
            context.SaveChanges();
            context.Database.CommitTransaction();
        }
    }
}
