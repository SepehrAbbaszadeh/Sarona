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
        public IQueryable<Link> Links => context.Links;
        public void AddExchange(Exchange exch)
        {
            exch.Abb = exch.Abb.ToUpper();
            exch.CreatedOn = DateTime.Now;
            exch.ModifiedOn = exch.CreatedOn;
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

        internal NetworkElement GetLinks(string name)
        {
            return context.NetworkElements.Where(x => x.Name == name)
                .Select(x => new NetworkElement()
                {
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    ExchangeId = x.ExchangeId,
                    Exchange = new Exchange() { Name = x.Exchange.Name, Abb = x.Exchange.Abb },
                    Customer = new Customer() { Name = x.Customer.Name, Abb = x.Customer.Name },
                    Manufacturer = x.Manufacturer,
                    Model = x.Model,
                    Name = x.Name,
                    Type = x.Type,
                    ParentId = x.ParentId,
                    LinksOnEnd1 = x.LinksOnEnd1.OrderBy(z => z.End2.Name).Select(y => new Link()
                    {
                        Channels = y.Channels,
                        Type = y.Type,
                        CreatedOn = y.CreatedOn,
                        Direction = y.Direction,
                        Remark = y.Remark,
                        End2Id = y.End2Id,
                        End2 = new NetworkElement() { Customer = y.End2.Customer, Name = y.End2.Name, NetworkType = y.End2.NetworkType, Exchange = new Exchange() { Area = y.End2.Exchange.Area, Abb = y.End2.Exchange.Abb, Name = y.End2.Exchange.Name } },
                        Id = y.Id
                    }),
                }).FirstOrDefault();
        }
        internal void AddCustomer(Customer newCustomer)
        {
            newCustomer.CreatedOn = DateTime.Now;
            newCustomer.ModifiedOn = newCustomer.CreatedOn;
            newCustomer.Abb = newCustomer.Abb.ToUpperInvariant();
            context.Customers.Add(newCustomer);
            context.SaveChanges();
        }

        internal void EditExchange(Exchange exch)
        {
            exch.ModifiedOn = DateTime.Now;
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
            newNe.CreatedOn = DateTime.Now;
            newNe.ModifiedOn = newNe.CreatedOn;
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
            ne.ModifiedOn = DateTime.Now;
            context.NetworkElements.Update(ne);
            context.SaveChanges();
        }

        internal void EditLink(Link link)
        {
            var originalLink = context.Links.Find(link.Id);
            originalLink.Remark = link.Remark;
            originalLink.Type = link.Type;
            originalLink.Channels = link.Channels;
            originalLink.ModifiedOn = DateTime.Now;
            context.SaveChanges();

        }
        internal void AddLink(Link newLink)
        {
            newLink.CreatedOn = DateTime.Now;
            newLink.ModifiedOn = newLink.ModifiedOn;
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
            context.Links.Add(otherLink);
            context.SaveChanges();
            newLink.OtherLinkId = otherLink.Id;
            context.Links.Update(newLink);
            context.SaveChanges();
            context.Database.CommitTransaction();
        }

        internal void DeleteLink(long id)
        {
            var link = context.Links.Find(id);
            var otherLink = context.Links.Find(link.OtherLinkId);
            context.Database.BeginTransaction();
            link.OtherLinkId = null;
            context.Links.Update(link);
            context.SaveChanges();
            context.Links.Remove(otherLink);
            context.SaveChanges();
            context.Links.Remove(link);
            context.SaveChanges();
            context.Database.CommitTransaction();
        }
    }
}
