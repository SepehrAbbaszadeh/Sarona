using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using Sarona.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sarona.Infrastructure
{
    public class ReportGenerator
    {
        private readonly string username;
        private readonly string path;
        public ReportGenerator(string user, string path)
        {
            username = user;
            this.path = path;
        }


        private void HeaderFooter(ExcelWorksheet ws,string text="")
        {
            ws.HeaderFooter.OddFooter.RightAlignedText = $"User: {username}";
            ws.HeaderFooter.OddHeader.LeftAlignedText = $"File: {ExcelHeaderFooter.FileName}";
            ws.HeaderFooter.OddHeader.CenteredText = text;
            ws.HeaderFooter.OddHeader.RightAlignedText = $"Sheet: {ExcelHeaderFooter.SheetName}";
            ws.HeaderFooter.OddFooter.LeftAlignedText = $"Report generated at: {DateTime.Now}";
            ws.HeaderFooter.OddFooter.CenteredText = $"{ExcelHeaderFooter.PageNumber}/{ExcelHeaderFooter.NumberOfPages}";
        }


        private void PrinterSettings(ExcelWorksheet ws, int? repeatedRow = null, eOrientation orientation = eOrientation.Portrait)
        {
            ws.PrinterSettings.PaperSize = ePaperSize.A4;
            ws.PrinterSettings.Orientation = orientation;
            //ws.PrinterSettings.VerticalCentered = true;
            ws.PrinterSettings.HorizontalCentered = true;
            ws.PrinterSettings.FitToPage = true;
            ws.PrinterSettings.FitToWidth = 1;
            ws.PrinterSettings.FitToHeight = 0;
            if (repeatedRow.HasValue)
                ws.PrinterSettings.RepeatRows = new ExcelAddress($"{repeatedRow}:{repeatedRow}");
        }


        private void AddLinkSheet(NetworkElement ne, ExcelPackage p, NeType type)
        {
            if (ne.LinksOnEnd1.Where(x => x.End2.NetworkType == type).Count() == 0)
                return;

            var ws = p.Workbook.Worksheets.Add($"{ne.Name} ({type})");
            int col = 1;
            int row = 6;

            ws.Cells[row, col++].Value = "Name";
            ws.Cells[row, col++].Value = "Exchange";
            ws.Cells[row, col++].Value = "Channels";
            ws.Cells[row, col++].Value = "[STM1,E1,Channel]";
            ws.Cells[row, col++].Value = "Direction";
            ws.Cells[row, col++].Value = "Type";
            ws.Cells[row, col++].Value = "Remark";

            row++;

            foreach (var link in ne.LinksOnEnd1.Where(x => x.End2.NetworkType == type))
            {
                col = 1;
                ws.Cells[row, col++].Value = link.End2.Name;
                ws.Cells[row, col++].Value = $"{link.End2.Exchange.Abb} ({link.End2.Exchange.Name})";
                ws.Cells[row, col++].Value = link.Channels;
                ws.Cells[row, col++].Value = link.GetStm1E1();
                ws.Cells[row, col++].Value = link.Direction;
                ws.Cells[row, col++].Value = link.Type;
                ws.Cells[row, col++].Value = link.Remark;
                row++;
            }

            var range = ws.Cells[6, 1, row - 1, 7];

            var tbl = ws.Tables.Add(range, $"{ne.Name}_{type}");
            tbl.TableStyle = TableStyles.Medium1;
            range.AutoFitColumns();

            row = 1;
            ws.Cells[row++, 1].Value = $"Name: {ne.Name}";
            ws.Cells[row++, 1].Value = $"Exchange: {ne.Exchange.Name} ({ne.Exchange.Abb})";
            if (ne.NetworkType == NeType.Core)
            {
                ws.Cells[row++, 1].Value = $"Model: {ne.Model}";
                ws.Cells[row++, 1].Value = $"Manufacturer: {ne.Manufacturer}";
            }
            else
            {
                //ws.Cells[row++, 1].Value = $"Customer: {ne.Customer.Name} ({ne.Customer.Abb})";
            }

            PrinterSettings(ws, 6);
            HeaderFooter(ws);
        }
        public void LinksReport(NetworkElement ne)
        {
            var file = new FileInfo(path);
            using (var p = new ExcelPackage(file))
            {
                var ws = p.Workbook.Worksheets.Add("Links");
                FillLinkWS(ws, ne.LinksOnEnd1);
                p.Save();
            }
        }


        public void RedFolder(IEnumerable<PRA_SIP_Sarona> records)
        {
            var fi = new FileInfo(path);
            using (var p = new ExcelPackage(fi))
            {
                int i = 1;
                var ws = p.Workbook.Worksheets.Add($"RedFolder");
                ws.Cells[1, i++].Value = "Subscriber Abbreviation";
                ws.Cells[1, i++].Value = "Subscriber Code";
                ws.Cells[1, i++].Value = "Switch Abbreviation";
                ws.Cells[1, i++].Value = "Switch Code";
                ws.Cells[1, i++].Value = "Link Type";
                ws.Cells[1, i++].Value = "Channels";
                ws.Cells[1, i++].Value = "Remark";
                i = 2;

                foreach (var r in records)
                {
                    int col = 1;
                    ws.Cells[i, col++].Value = r.SubscriberAbb;
                    ws.Cells[i, col++].Value = r.SubscriberCode;
                    ws.Cells[i, col++].Value = r.SwitchAbb;
                    ws.Cells[i, col++].Value = r.SwitchCode;
                    ws.Cells[i, col++].Value = r.LinkType;
                    ws.Cells[i, col++].Value = r.Channels;
                    ws.Cells[i++, col++].Value = r.Remark;
                }

                var range = ws.Cells[1, 1, i - 1, 7];

                var tbl = ws.Tables.Add(range, "RedFolder");
                tbl.TableStyle = TableStyles.Medium1;
                range.AutoFitColumns();
                HeaderFooter(ws);
                PrinterSettings(ws, 1, eOrientation.Portrait);
                p.Save();
            }


        }
        public void ShenasnameByArea(IEnumerable<Exchange> exchanges)
        {
            var fi = new FileInfo(path);
            using (var p = new ExcelPackage(fi))
            {
                foreach (var exchange in exchanges.OrderBy(x => x.Abb))
                {
                    int i = 1;
                    var ws = p.Workbook.Worksheets.Add($"{exchange.Name} ({exchange.Abb})");
                    ws.Cells[1, i++].Value = "Name";
                    ws.Cells[1, i++].Value = "Network Type";
                    ws.Cells[1, i++].Value = "Type";
                    ws.Cells[1, i++].Value = "Model";
                    ws.Cells[1, i++].Value = "Manufacturer";
                    ws.Cells[1, i++].Value = "Parent Name";
                    ws.Cells[1, i++].Value = "Installed Capacity";
                    ws.Cells[1, i++].Value = "Used Capacity";
                    ws.Cells[1, i++].Value = "Remark";
                    i = 2;
                    foreach (var ne in exchange.NetworkElements)
                    {
                        int col = 1;
                        ws.Cells[i, col++].Value = ne.Name;
                        ws.Cells[i, col++].Value = ne.NetworkType;
                        ws.Cells[i, col++].Value = ne.Type;
                        ws.Cells[i, col++].Value = ne.Model;
                        ws.Cells[i, col++].Value = ne.Manufacturer;

                        if (ne.Parent is null)
                            col++;
                        else
                            ws.Cells[i, col++].Value = ne.Parent.Name;

                        ws.Cells[i, col++].Value = ne.InstalledCapacity;
                        ws.Cells[i, col++].Value = ne.UsedCapacity;
                        ws.Cells[i, col++].Value = ne.Remark;
                        i++;
                    }
                    var range = ws.Cells[1, 1, i - 1, 9];

                    var tbl = ws.Tables.Add(range, exchange.Abb);
                    tbl.TableStyle = TableStyles.Medium1;
                    range.AutoFitColumns();
                    HeaderFooter(ws);
                    PrinterSettings(ws, 1, eOrientation.Landscape);
                }
                p.Save();
            }
        }

        internal void Numbering(IEnumerable<NumberingPool> nps)
        {
            var fi = new FileInfo(path);
            using (var p = new ExcelPackage(fi))
            {
                var ws = p.Workbook.Worksheets.Add($"Numberings");
                FillNumberingWS(ws, nps);
                p.Save();
            }
        }


        private void FillLinkWS(ExcelWorksheet ws, IEnumerable<Link> links)
        {
            if (links.Count() == 0)
                return;
            int col = 1;
            int row = 1;
            ws.Cells[row, col++].Value = "Name";
            ws.Cells[row, col++].Value = "Exchange";
            ws.Cells[row, col++].Value = "Channels";
            ws.Cells[row, col++].Value = "[STM1,E1,Channel]";
            ws.Cells[row, col++].Value = "Direction";
            ws.Cells[row, col++].Value = "Type";
            ws.Cells[row, col++].Value = "Remark";

            row++;

            foreach (var link in links)
            {
                col = 1;
                ws.Cells[row, col++].Value = link.End2.Name;
                ws.Cells[row, col++].Value = $"{link.End2.Exchange.Abb} ({link.End2.Exchange.Name})";
                ws.Cells[row, col++].Value = link.Channels;
                ws.Cells[row, col++].Value = link.GetStm1E1();
                ws.Cells[row, col++].Value = link.Direction;
                ws.Cells[row, col++].Value = link.Type;
                ws.Cells[row, col++].Value = link.Remark;
                row++;
            }
            var range = ws.Cells[1, 1, row - 1, 7];
            var tbl = ws.Tables.Add(range, "Links");
            tbl.TableStyle = TableStyles.Medium1;
            range.AutoFitColumns();
            HeaderFooter(ws);
            PrinterSettings(ws, 1, eOrientation.Portrait);
            
        }
        internal void NE(NetworkElement ne)
        {
            var fi = new FileInfo(path);
            using (var p = new ExcelPackage(fi))
            {
                var ws = p.Workbook.Worksheets.Add("Links");
                FillLinkWS(ws, ne.LinksOnEnd1);
                ws = p.Workbook.Worksheets.Add("Numberings");
                FillNumberingWS(ws, ne.NumberingPoolNetworkElements.Select(x=>x.Numbering));
                ws = p.Workbook.Worksheets.Add("Remotes");
                FillRemotesWs(ws, ne.NetworkElements.Where(x => x.NetworkType == NeType.Remote),"remotes");
                ws = p.Workbook.Worksheets.Add("Accesses");
                FillRemotesWs(ws, ne.NetworkElements.Where(x => x.NetworkType == NeType.Access),"accesses");
                p.Save();

                
            }
        }

        private void FillRemotesWs(ExcelWorksheet ws, IEnumerable<NetworkElement> remotes, string tableName)
        {
            if (remotes.Count() == 0)
                return;
            int i = 1;
            ws.Cells[1, i++].Value = "Name";
            ws.Cells[1, i++].Value = "Network Type";
            ws.Cells[1, i++].Value = "Type";
            ws.Cells[1, i++].Value = "Model";
            ws.Cells[1, i++].Value = "Manufacturer";
            ws.Cells[1, i++].Value = "Parent Name";
            ws.Cells[1, i++].Value = "Installed Capacity";
            ws.Cells[1, i++].Value = "Used Capacity";
            ws.Cells[1, i++].Value = "Numberings";
            ws.Cells[1, i++].Value = "Remark";


            i = 2;
            foreach (var ne in remotes)
            {
                int col = 1;
                ws.Cells[i, col++].Value = ne.Name;
                ws.Cells[i, col++].Value = ne.NetworkType;
                ws.Cells[i, col++].Value = ne.Type;
                ws.Cells[i, col++].Value = ne.Model;
                ws.Cells[i, col++].Value = ne.Manufacturer;

                if (ne.Parent is null)
                    col++;
                else
                    ws.Cells[i, col++].Value = ne.Parent.Name;

                ws.Cells[i, col++].Value = ne.InstalledCapacity;
                ws.Cells[i, col++].Value = ne.UsedCapacity;
                ws.Cells[i, col++].Value = ne.GetNumberings();
                ws.Cells[i, col++].Value = ne.Remark;
                i++;
            }
            var range = ws.Cells[1, 1, i - 1, 10];

            var tbl = ws.Tables.Add(range, tableName);
            tbl.TableStyle = TableStyles.Medium1;
            range.AutoFitColumns();
            HeaderFooter(ws);
            PrinterSettings(ws, 1, eOrientation.Landscape);
        }

        private void FillNumberingWS(ExcelWorksheet ws, IEnumerable<NumberingPool> nps)
        {
            if(nps.Count() == 0)
                return;
            int i = 1;
            ws.Cells[1, i++].Value = "Prefix";
            ws.Cells[1, i++].Value = "Min-Max";
            ws.Cells[1, i++].Value = "Min-Max (Sheet)";
            ws.Cells[1, i++].Value = "Area";
            ws.Cells[1, i++].Value = "Area (Sheet)";
            ws.Cells[1, i++].Value = "Rond";
            ws.Cells[1, i++].Value = "Number Type";
            ws.Cells[1, i++].Value = "Subscriber";
            ws.Cells[1, i++].Value = "Abbreviation";
            ws.Cells[1, i++].Value = "Link Type";
            ws.Cells[1, i++].Value = "Direction";
            ws.Cells[1, i++].Value = "Float";
            ws.Cells[1, i++].Value = "Keshvari";
            ws.Cells[1, i++].Value = "Status";
            ws.Cells[1, i++].Value = "Owner";
            ws.Cells[1, i++].Value = "NE";
            ws.Cells[1, i++].Value = "Remark";
            i = 2;

            foreach (var number in nps)
            {
                int col = 1;
                ws.Cells[i, col++].Value = number.Prefix;
                ws.Cells[i, col++].Value = $"{number.Min}-{number.Max}";
                ws.Cells[i, col++].Value = $"{number.SecondaryMin}-{number.SecondaryMax}";
                ws.Cells[i, col++].Value = number.Area;
                ws.Cells[i, col++].Value = number.SecondaryArea;
                ws.Cells[i, col++].Value = number.Rond;
                ws.Cells[i, col++].Value = number.NumberType;
                ws.Cells[i, col++].Value = number.SubscriberName;
                ws.Cells[i, col++].Value = number.Abb;
                ws.Cells[i, col++].Value = number.Link;
                ws.Cells[i, col++].Value = number.Direction;
                ws.Cells[i, col++].Value = number.IsFloat ? "true" : "false";
                ws.Cells[i, col++].Value = number.IsKeshvari ? "true" : "false";
                ws.Cells[i, col++].Value = number.Status;
                ws.Cells[i, col++].Value = number.Owner;
                ws.Cells[i, col++].Value = number.GetNetworkElementNames();
                ws.Cells[i, col++].Value = number.Remark;
                i++;
            }
            var range = ws.Cells[1, 1, i - 1, 17];

            var tbl = ws.Tables.Add(range, "Numbering");
            tbl.TableStyle = TableStyles.Medium1;
            range.AutoFitColumns();
            HeaderFooter(ws);
            PrinterSettings(ws, 1, eOrientation.Landscape);
        }
    }
}
