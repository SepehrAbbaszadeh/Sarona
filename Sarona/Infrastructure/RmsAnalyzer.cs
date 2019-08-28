using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sarona.Infrastructure
{
    public class RmsAnalyzer
    {
        public class RmsData
        {
            public string MarkazName { get; set; }
            public string RouteName { get; set; }
            public int Hour { get; set; }
            public string Date { get; set; }
            public int Available { get; set; }
            public int Bid { get; set; }
            public int SeizeIn { get; set; }
            public int AnsIn { get; set; }
            public int SeizeOut { get; set; }
            public int AnsOut { get; set; }
            public double ErlangIn { get; set; }
            public double ErlangOut { get; set; }
            public int Week { get; set; }
            public int Nameroz { get; set; }
            public double AnsErlIn { get; set; }
            public double AnsErlOut { get; set; }
            public double ErlangTotal { get; set; }
            public long MarkazId { get; set; }
            public long OperatorId { get; set; }
        }


        private List<RmsData> rmsDatas = new List<RmsData>();
        private readonly Stream stream;
        private readonly IEnumerable<Models.RmsMapping> rmsMappings;
        public RmsAnalyzer(Stream sr, IEnumerable<Models.RmsMapping> mappings)
        {
            this.stream = sr;
            rmsMappings = mappings;
        }
        public string Analyze()
        {
            using (var sr = new StreamReader(stream))
            {
                var header = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(',');
                    var data = new RmsData();
                    data.MarkazName = line[0];
                    data.RouteName = line[1];
                    var map = rmsMappings.Where(x => x.Markaz == data.MarkazName && x.RouteName == data.RouteName).FirstOrDefault();
                    if (map is null)
                        continue;
                    data.OperatorId = map.OperatorId;
                    data.MarkazId = map.MarkazId;
                    data.Hour = Convert.ToInt32(line[2]);
                    data.Available = (int)Convert.ToDouble(line[3]);
                    data.Bid = (int)Convert.ToDouble(line[4]);
                    data.SeizeOut = (int)Convert.ToDouble(line[5]);
                    data.AnsOut = (int)Convert.ToDouble(line[6]);
                    data.SeizeIn = (int)Convert.ToDouble(line[7]);
                    data.ErlangIn = Convert.ToDouble(line[8]);
                    data.ErlangOut = Convert.ToDouble(line[9]);
                    data.ErlangTotal = data.ErlangIn + data.ErlangOut;
                    data.AnsIn = (int)Convert.ToDouble(line[10]);
                    data.Date = line[11];
                    data.Week = Convert.ToInt32(line[12].Replace("هفته ", ""));
                    data.Nameroz = Convert.ToInt32(line[13].Substring(1, 1));
                    data.AnsErlIn = Convert.ToDouble(line[14])*data.AnsIn/3600;
                    data.AnsErlOut = Convert.ToDouble(line[15])*data.AnsOut/3600;
                    rmsDatas.Add(data);
                }
            }

            var q = from d in rmsDatas
                    group d by new { d.MarkazId, d.OperatorId, d.Hour, d.Date } into grp
                    select new RmsData
                    {
                        MarkazId = grp.Key.MarkazId,
                        OperatorId = grp.Key.OperatorId,
                        MarkazName = grp.First().MarkazName,
                        RouteName = grp.First().RouteName,
                        Hour = grp.Key.Hour,
                        Date = grp.Key.Date,
                        AnsErlIn = grp.Sum(x => x.AnsErlIn),
                        AnsErlOut = grp.Sum(x => x.AnsErlOut),
                        AnsIn = grp.Sum(x => x.AnsIn),
                        AnsOut = grp.Sum(x => x.AnsOut),
                        Available = grp.Sum(x => x.Available),
                        Bid = grp.Sum(x => x.Bid),
                        ErlangIn = grp.Sum(x => x.ErlangIn),
                        ErlangOut = grp.Sum(x => x.ErlangOut),
                        ErlangTotal = grp.Sum(x => x.ErlangTotal),
                        Nameroz = grp.First().Nameroz,
                        SeizeIn = grp.Sum(x => x.SeizeIn),
                        SeizeOut = grp.Sum(x => x.SeizeOut),
                        Week = grp.First().Week
                    };

            var max = from r in q
                      group r by new { r.MarkazId, r.OperatorId, r.Week} into grp
                      select new {grp.Key,Max = grp.Max(x=>x.ErlangTotal)};

            var res = (from r in q
                      join m in max 
                      on new { r.MarkazId, r.OperatorId,r.Week, Erl = r.ErlangTotal } equals new { m.Key.MarkazId, m.Key.OperatorId, m.Key.Week, Erl = m.Max }
                      select r).GroupBy(x=>new { x.MarkazId,x.OperatorId,x.Week,x.ErlangTotal}).Select(x=>x.FirstOrDefault()).ToArray();

            var sb = new StringBuilder();
            foreach (var item in res)
            {
                var row = string.Join(
                    ';',
                    item.MarkazId,
                    item.OperatorId,
                    item.Week,
                    item.Nameroz,
                    string.Format("{0:00}00",item.Hour),
                    item.ErlangTotal,
                    item.Available,
                    item.Bid,
                    item.SeizeOut,
                    item.AnsOut,
                    item.AnsOut != 0 ? item.AnsErlOut * 3600 / item.AnsOut : 0,
                    item.SeizeOut !=0 ? item.ErlangOut * 3600 / item.SeizeOut : 0,
                    item.SeizeIn,
                    item.AnsIn,
                    item.AnsIn != 0 ? item.AnsErlIn * 3600 / item.AnsIn : 0,
                    item.SeizeIn != 0 ? item.ErlangIn * 3600 / item.SeizeIn : 0
                    );
                sb.AppendLine(row);
            }

            return sb.ToString();
        }
    }
}
