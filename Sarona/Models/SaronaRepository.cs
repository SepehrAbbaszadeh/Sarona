using Microsoft.EntityFrameworkCore;
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
        public IQueryable<Misc> Miscs => context.Miscs;
        public IQueryable<NetworkElement> NetworkElements => context.NetworkElements;
        public IQueryable<Link> Links => context.Links;
        public IQueryable<LinkHistory> LinkHistories => context.LinkHistories;
        public IQueryable<NumberingPool> NumberingPools => context.NumberingPools;
        public IQueryable<CrmCode> CrmCodes => context.CrmCodes;
        public IQueryable<NumberingPoolNetworkElement> NumberingPoolNetworkElements => context.NumberingPoolNetworkElements;
        public IQueryable<RmsMapping> RmsMappings => context.RmsMappings;
        internal bool InsertFreeCodes(InsertNumberingPool freeCode, string user, out string error, out int addNo)
        {
            List<string> prefixesToAdd = new List<string>();
            List<string> prefixesToChange = new List<string>();

            addNo = 0;
            for (int i = freeCode.From; i < freeCode.To + 1; i++)
            {
                var overlaps = PrefixValidationSearch(i.ToString());
                if (overlaps.Count() == 0)
                {
                    prefixesToAdd.Add(i.ToString());
                }
                else if (overlaps.Count() == 1 && overlaps.First().Prefix == i.ToString() && freeCode.NotDefined)
                {
                    prefixesToChange.Add(i.ToString());
                }
                else if (overlaps.Count() > 0 && freeCode.NotDefined)
                {
                    continue;
                }
                else
                {
                    error = $"Overlap: {string.Join(", ", overlaps.Select(x => x.Prefix))}";
                    return false;
                }
            }
            var crmCodes = CrmCodes
                .Where(x => prefixesToAdd.Contains(x.Code) || prefixesToChange.Contains(x.Code)).ToArray();
            List<NumberingPool> npToAdd = new List<NumberingPool>();
            addNo = prefixesToAdd.Count;
            foreach (var prefix in prefixesToAdd)
            {
                var newPrefix = new NumberingPool()
                {
                    Abb = null,
                    IsFloat = false,
                    SecondaryArea = freeCode.Area,
                    Remark = "",
                    Area = freeCode.Area,
                    SubscriberName = null,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ExpireDate = new DateTime(2300, 1, 1),
                    IsKeshvari = false,
                    Min = freeCode.Min,
                    Max = freeCode.Max,
                    SecondaryMax = freeCode.Max,
                    SecondaryMin = freeCode.Min,
                    Owner = "TCT",
                    NumberType = freeCode.NumberType,
                    Status = freeCode.IsReserved ? NumberingStatus.Reserved : NumberingStatus.Free,
                    Username = user,
                    ChargingCase = freeCode.ChargingCase,
                    Prefix = prefix,
                    Link = freeCode.Link,
                    Direction = null,
                };
                //newPrefix.Rond = newPrefix.GetRondType();
                npToAdd.Add(newPrefix);
            }
            var npToChange = context.NumberingPools
                .Where(x => prefixesToChange.Contains(x.Prefix)).ToArray();
            foreach (var p in npToChange)
            {
                p.SecondaryMin = freeCode.Min;
                p.SecondaryMax = freeCode.Max;
                p.SecondaryArea = freeCode.Area;
                p.Username = user;
            }

            if (freeCode.NotDefined)
            {
                foreach (var np in npToAdd)
                {
                    var q = crmCodes.Where(x => x.Code == np.Prefix).ToArray();
                    if (q.Count() == 1)
                    {
                        np.Status = NumberingStatus.Reserved;
                        np.Area = q.First().Area;
                    }
                }
                foreach (var np in npToChange)
                {
                    var q = crmCodes.Where(x => x.Code == np.Prefix).ToArray();
                    if (q.Count() == 1)
                    {
                        if (np.Status == NumberingStatus.Free)
                        {
                            np.Status = NumberingStatus.Reserved;
                        }
                        np.Area = q.First().Area;
                    }
                }
            }
            context.NumberingPools.AddRange(npToAdd);
            context.SaveChanges();
            error = "";
            return true;

        }
        public IQueryable<NumberingPool> GetNps()
        {
            return context.NumberingPools
                            .Include(x => x.NumberingPoolNetworkElements)
                            .ThenInclude(x => x.Element)
                            .ThenInclude(x => x.Parent)
                            .Include(x => x.NumberingPoolNetworkElements)
                            .ThenInclude(x => x.Element)
                            .ThenInclude(x => x.Exchange)
                            .OrderBy(x => x.Prefix)
                            .AsNoTracking();
        }

        internal IEnumerable<NumberingPool> GetNumberingPool(string prefix)
        {
            var q = context.NumberingPools
                .Where(x => x.Prefix.StartsWith(prefix))
                .Select(x => new NumberingPool()
                {
                    Id = x.Id,
                    Prefix = x.Prefix,
                    ChargingCase = x.ChargingCase,
                    CreatedOn = x.CreatedOn,

                    Max = x.Max,
                    Min = x.Min,
                    ModifiedOn = x.ModifiedOn,
                    Owner = x.Owner,
                    Remark = x.Remark,
                    Status = x.Status,
                    Username = x.Username,
                }
                );
            return q;
        }

        internal void DeleteNumberingPool(params string[] prefixes)
        {
            context.Database.BeginTransaction();
            var nps = context.NumberingPools.Where(x => prefixes.Contains(x.Prefix)).ToArray();
            context.NumberingPools.RemoveRange(nps);
            context.SaveChanges();
            foreach (var np in nps)
            {
                if (!(np.Abb is null))
                {
                    DeleteAbbreviation(np.Abb);
                }
            }
            context.Database.CommitTransaction();
        }

        public void AddExchange(Exchange exch)
        {
            exch.Abb = exch.Abb.ToUpper();
            if (!context.Abbreviations.Contains(new Abbreviation() { Abb = exch.Abb }))
            {
                context.Abbreviations.Add(new Abbreviation() { Abb = exch.Abb });
            }
            exch.CreatedOn = DateTime.Now;
            exch.ModifiedOn = exch.CreatedOn;
            context.Exchanges.Add(exch);
            context.SaveChanges();
        }

        internal void EditNumberingPool(NumberingPool np)
        {
            var originalNp = context.NumberingPools.Find(np.Id);
            originalNp.Prefix = np.Prefix;
            originalNp.Area = np.Area;
            originalNp.ChargingCase = np.ChargingCase;
            originalNp.Direction = np.Direction;
            originalNp.IsFloat = np.IsFloat;
            originalNp.IsKeshvari = np.IsKeshvari;
            originalNp.Link = np.Link;
            originalNp.Max = np.Max;
            originalNp.Min = np.Min;
            originalNp.ModifiedOn = DateTime.Now;
            originalNp.NumberType = np.NumberType;
            originalNp.Owner = np.Owner;
            originalNp.Remark = np.Remark;
            originalNp.SecondaryArea = np.SecondaryArea;
            originalNp.SecondaryMax = np.SecondaryMax;
            originalNp.SecondaryMin = np.SecondaryMin;
            originalNp.Username = np.Username;
            //track.Property(x => x.CreatedOn).IsModified = false;
            context.SaveChanges();
        }

        internal void AddNumberingPool(NumberingPool np, bool isReserved)
        {
            np.CreatedOn = DateTime.Now;
            np.ModifiedOn = np.CreatedOn;
            if (isReserved)
            {
                np.Status = NumberingStatus.Reserved;
            }
            else
            {
                np.Status = NumberingStatus.Free;
            }

            //np.Rond = np.GetRondType();
            context.NumberingPools.Add(np);
            context.SaveChanges();
        }

        public Exchange RemoveExchange(string abb)
        {
            var exch = context.Exchanges.Where(x => x.Abb == abb).FirstOrDefault();
            context.Exchanges.Remove(exch);
            context.SaveChanges();
            DeleteAbbreviation(abb);
            return exch;
        }

        internal void ReserveCodes(string[] prefixes, Area? area, LinkType? link, byte min, byte max, bool changeMinMax, string name)
        {
            var selectedprefixes = context.NumberingPools.Where(x => prefixes.Contains(x.Prefix) && x.Status == NumberingStatus.Free);
            foreach (var prefix in selectedprefixes)
            {
                if (changeMinMax)
                {
                    prefix.Min = min;
                    prefix.Max = max;
                }

                prefix.Link = link;
                prefix.Status = NumberingStatus.Reserved;
                prefix.ModifiedOn = DateTime.Now;
                prefix.Username = name;
                prefix.Area = area;
            }
            context.SaveChanges();
        }

        internal IEnumerable<NumberingPool> PrefixValidationSearch(string prefix)
        {
            string query = "";
            List<NumberingPool> result = new List<NumberingPool>();
            IEnumerable<NumberingPool> q;
            foreach (var ch in prefix)
            {
                query = String.Concat(query, ch);
                q = context.NumberingPools.Where(x => x.Prefix == query).ToList();
                result.AddRange(q);
            }
            q = context.NumberingPools.Where(x => x.Prefix.StartsWith(prefix)).ToList();
            result.AddRange(q);
            return result.Distinct();
        }

        internal void AssignPrefix(string[] prefixes, string abb, string name, string user, LinkType link, LinkDirection dir)
        {

            name = name.Replace('ي', 'ی').Replace('ك', 'ک');
            var q = context.NumberingPools.Where(x => prefixes.Contains(x.Prefix) && x.Status == NumberingStatus.Reserved).ToArray();
            foreach (var prefix in q)
            {
                prefix.Direction = dir;
                prefix.SubscriberName = name;
                prefix.NormalizedSubscriberName = name.Replace(" ", "");
                prefix.Abb = abb.ToUpper();
                prefix.Status = NumberingStatus.Used;
                prefix.ModifiedOn = DateTime.Now;
                prefix.Username = user;
                prefix.Link = link;
                prefix.Abbreviation = new Abbreviation() { Abb = abb.ToUpper() };
            }
            context.SaveChanges();


        }

        public void RemovePrefix(string user, params string[] prefixes)
        {

            var prefix = context.NumberingPools.Where(x => prefixes.Contains(x.Prefix) && x.Status == NumberingStatus.Used).Include(x => x.NumberingPoolNetworkElements).First();
            string abbTemp = prefix.Abb?.ToUpper();
            context.NumberingPoolNetworkElements.RemoveRange(prefix.NumberingPoolNetworkElements);
            prefix.Abb = null;
            prefix.SubscriberName = null;
            prefix.Status = NumberingStatus.Reserved;
            prefix.Link = null;
            prefix.Direction = null;
            prefix.Username = user;
            prefix.ModifiedOn = DateTime.Now;
            context.SaveChanges();
            DeleteAbbreviation(abbTemp);
        }
        /// <summary>
        /// Assign an available customer to a prefix.
        /// </summary>
        /// <param name="selectedPrefix">Prefix that availble customer will assign.</param>
        /// <param name="customerId">Selected prefix which customer data exists.</param>
        /// <param name="user">User that modifies this prefix.</param>
        /// <param name="link">Link type of prefix.</param>
        internal void AssignPrefix(string[] prefixes, long customerId, string user, LinkType link, LinkDirection dir)
        {
            var sub = context.NumberingPools.Find(customerId);
            var q = context.NumberingPools.Where(x => prefixes.Contains(x.Prefix) && x.Status == NumberingStatus.Reserved).ToArray();
            foreach (var prefix in q)
            {
                prefix.Abb = sub.Abb.ToUpper();
                prefix.Direction = dir;
                prefix.NormalizedSubscriberName = sub.NormalizedSubscriberName;
                prefix.SubscriberName = sub.SubscriberName;
                prefix.Status = NumberingStatus.Used;
                prefix.ModifiedOn = DateTime.Now;
                prefix.Username = user;
                prefix.Link = link;
            }
            context.SaveChanges();
        }

        public Exchange DeleteExchange(int id)
        {
            var exch = context.Exchanges.Find(id);
            context.Exchanges.Remove(exch);
            context.SaveChanges();

            return exch;
        }

        internal void DeleteAbbreviation(string abb)
        {
            if (abb is null)
            {
                return;
            }

            int count = 0;
            count = context.Exchanges.Where(x => x.Abb == abb).Count();
            count += context.NumberingPools.Where(x => x.Abb == abb).Count();
            if (count == 0)
            {
                context.Abbreviations.Remove(new Abbreviation() { Abb = abb });
                context.SaveChanges();
            }

        }

        internal NetworkElement GetLinks(string name)
        {
            return context.NetworkElements
                .Where(x => x.Name == name)
                .Select(x => new NetworkElement()
                {
                    Id = x.Id,
                    //CustomerId = x.CustomerId,
                    ExchangeId = x.ExchangeId,
                    Exchange = new Exchange() { Name = x.Exchange.Name, Abb = x.Exchange.Abb },
                    //Customer = new Customer() { Name = x.Customer.Name, Abb = x.Customer.Name },
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
                        ModifiedOn = y.ModifiedOn,
                        Direction = y.Direction,
                        Remark = y.Remark,
                        End2Id = y.End2Id,
                        Username = y.Username,
                        End2 = new NetworkElement() { /*Customer = y.End2.Customer,*/ Name = y.End2.Name, NetworkType = y.End2.NetworkType, Exchange = new Exchange() { Area = y.End2.Exchange.Area, Abb = y.End2.Exchange.Abb, Name = y.End2.Exchange.Name } },
                        Id = y.Id
                    }),
                }).FirstOrDefault();
        }

        internal void EditExchange(Exchange exch)
        {
            var originalExchange = context.Exchanges.Where(x => x.Id == exch.Id).Include(x => x.Abbreviation).First();

            originalExchange.ModifiedOn = DateTime.Now;
            originalExchange.Name = exch.Name;
            originalExchange.Abb = exch.Abb.ToUpper();
            originalExchange.Abbreviation.Abb = exch.Abb.ToUpper();
            originalExchange.Providence = exch.Providence;
            originalExchange.Area = exch.Area;
            originalExchange.IsSite = exch.IsSite;
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
                    newNe.Name = $"ACCESS{context.GetNextAccessSequenceValue()}";
                    break;
                case NeType.Remote:
                    newNe.Name = $"REMOTE{context.GetNextRemoteSequenceValue()}";
                    break;
                //case NeType.PBX:
                //    newNe.Name = $"PBX{context.GetNextPbxSequenceValue()}";
                //    break;
                //case NeType.IP_PBX:
                //    newNe.Name = $"IP-PBX{context.GetNextPbxSequenceValue()}";
                //    break;
                default:
                    break;
            }
            newNe.CreatedOn = DateTime.Now;
            newNe.ModifiedOn = newNe.CreatedOn;
            context.NetworkElements.Add(newNe);
            context.SaveChanges();
        }

        internal NetworkElement DeleteNetworkElement(long id)
        {
            var ne = context.NetworkElements.Find(id);
            context.NetworkElements.Remove(ne);
            context.SaveChanges();
            return ne;
        }

        internal void EditNetworkElement(NetworkElement ne)
        {
            ne.ModifiedOn = DateTime.Now;
            context.NetworkElements.Update(ne);
            context.SaveChanges();
        }

        internal void EditLink(Link link)
        {
            var originalLink = context.Links.Where(x => x.Id == link.Id).Include(x => x.OtherLink).First();

            var linkHistory = new LinkHistory()
            {
                Channels = originalLink.Channels,
                LinkId = originalLink.Id,
                ModifiedOn = originalLink.ModifiedOn,
                Remark = originalLink.Remark,
                Type = originalLink.Type,
                Username = originalLink.Username,
                Direction = originalLink.Direction,
            };

            var otherLink = originalLink.OtherLink;

            var otherLinkHistory = new LinkHistory()
            {
                Channels = otherLink.Channels,
                LinkId = otherLink.Id,
                ModifiedOn = otherLink.ModifiedOn,
                Remark = otherLink.Remark,
                Type = otherLink.Type,
                Username = otherLink.Username,
                Direction = otherLink.Direction
            };

            originalLink.Remark = link.Remark;
            originalLink.Type = link.Type;
            originalLink.Channels = link.Channels;
            originalLink.ModifiedOn = DateTime.Now;
            originalLink.Direction = link.Direction;

            otherLink.Remark = link.Remark;
            otherLink.Type = link.Type;
            otherLink.Channels = link.Channels;
            otherLink.ModifiedOn = originalLink.ModifiedOn;
            switch (link.Direction)
            {
                case LinkDirection.Incoming:
                    otherLink.Direction = LinkDirection.Outgoing;
                    break;
                case LinkDirection.Outgoing:
                    otherLink.Direction = LinkDirection.Incoming;
                    break;
                case LinkDirection.Bothway:
                    otherLink.Direction = LinkDirection.Bothway;
                    break;
            }
            context.LinkHistories.AddRange(linkHistory, otherLinkHistory);
            context.Database.BeginTransaction();
            context.SaveChanges();
            context.Database.CommitTransaction();

        }
        internal void AddLink(Link newLink)
        {
            newLink.CreatedOn = DateTime.Now;
            newLink.ModifiedOn = newLink.CreatedOn;
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
                ModifiedOn = newLink.ModifiedOn,
                Direction = direction,
                End1Id = newLink.End2Id,
                End2Id = newLink.End1Id,
                OtherLinkId = newLink.Id,
                Remark = newLink.Remark,
                Type = newLink.Type,
                Username = newLink.Username

            };
            context.Links.Add(otherLink);
            context.SaveChanges();
            newLink.OtherLinkId = otherLink.Id;
            context.Links.Update(newLink);
            context.SaveChanges();
            context.Database.CommitTransaction();
        }

        internal void DetachPrefix(long neId, long prefixId)
        {
            var junction = new NumberingPoolNetworkElement() { NetworkElementId = neId, NumberingId = prefixId };
            context.NumberingPoolNetworkElements.Attach(junction);
            var np = context.NumberingPools.Find(prefixId);
            np.Status = NumberingStatus.Free;
            context.NumberingPoolNetworkElements.Remove(junction);
            context.SaveChanges();
        }

        internal void DeleteLink(long id)
        {
            var link = context.Links.Where(x => x.Id == id)
                .Include(x => x.OtherLink)
                .First();
            var otherLink = link.OtherLink;
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

        internal void AttachNumberingPool(long neId, string user, params string[] prefixes)
        {
            var ne = context.NetworkElements.Find(neId);
            var numberings = context.NumberingPools.Where(x => prefixes.Contains(x.Prefix) && (x.Status == NumberingStatus.Free || x.Status == NumberingStatus.Reserved || x.IsFloat));
            foreach (var number in numberings)
            {
                number.Status = NumberingStatus.Used;
                number.Direction = null;
                number.ModifiedOn = DateTime.Now;
                number.SubscriberName = null;
                number.Abb = null;
                number.Link = null;
                number.Username = user;
            }

            ne.NumberingPoolNetworkElements = numberings.Select(x => new NumberingPoolNetworkElement()
            {
                NetworkElementId = ne.Id,
                NumberingId = x.Id
            }).ToList();
            context.SaveChanges();
        }

        internal void AttachNumberingPool(string neName, string user, params long[] ids)
        {
            var ne = context.NetworkElements.Where(x => x.Name == neName).First();
            var numberings = context.NumberingPools.Where(x => ids.Contains(x.Id));
            foreach (var number in numberings)
            {
                number.Status = NumberingStatus.Used;
                number.Username = user;
                number.ModifiedOn = DateTime.Now;
            }

            ne.NumberingPoolNetworkElements = ids.Select(x => new NumberingPoolNetworkElement()
            {
                NetworkElementId = ne.Id,
                NumberingId = x
            }).ToList();
            context.SaveChanges();
        }
    }
}
