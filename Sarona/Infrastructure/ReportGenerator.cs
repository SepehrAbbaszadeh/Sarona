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
        private string username;
        public ReportGenerator(string user) => username = user;

        private void HeaderFooter(ExcelWorksheet ws)
        {
            ws.HeaderFooter.OddFooter.RightAlignedText = $"User: {username}";
            ws.HeaderFooter.OddHeader.CenteredText = $"{ExcelHeaderFooter.SheetName}";
            ws.HeaderFooter.OddFooter.LeftAlignedText = $"Print Date: {DateTime.Now}";
            ws.HeaderFooter.OddFooter.CenteredText = $"{ExcelHeaderFooter.PageNumber}/{ExcelHeaderFooter.NumberOfPages}";
        }


        private void PrinterSettings(ExcelWorksheet ws,int? repeatedRow = null, eOrientation orientation = eOrientation.Portrait)
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
            ws.Cells[row, col++].Value = "STM1/E1 equivalence";
            ws.Cells[row, col++].Value = "Direction";
            if(type == NeType.IP_PBX || type == NeType.PBX)
            {
                ws.Cells[row, col++].Value = "Customer Name";
                ws.Cells[row, col++].Value = "Customer Abb";
            }
            ws.Cells[row, col++].Value = "Type";
            ws.Cells[row, col++].Value = "Remark";

            row++;

            foreach (var link in ne.LinksOnEnd1.Where(x=>x.End2.NetworkType == type))
            {
                col = 1;
                ws.Cells[row, col++].Value = link.End2.Name;
                ws.Cells[row, col++].Value = $"{link.End2.Exchange.Abb} ({link.End2.Exchange.Name})";
                ws.Cells[row, col++].Value = link.Channels;
                ws.Cells[row, col++].Value = link.GetStm1E1();
                ws.Cells[row, col++].Value = link.Direction;
                if (type == NeType.IP_PBX || type == NeType.PBX)
                {
                    ws.Cells[row, col++].Value = link.End2.Customer.Name;
                    ws.Cells[row, col++].Value = link.End2.Customer.Abb;
                }
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
                ws.Cells[row++, 1].Value = $"Customer: {ne.Customer.Name} ({ne.Customer.Abb})";
            }

            PrinterSettings(ws,6);
            HeaderFooter(ws);
        }
        public void LinksReport(NetworkElement ne, string path)
        {
            var file = new FileInfo(path);
            using (var p = new ExcelPackage(file))
            {
                AddLinkSheet(ne, p, NeType.Core);
                AddLinkSheet(ne, p, NeType.PBX);
                AddLinkSheet(ne, p, NeType.IP_PBX);
                p.Save();
            }
        }

        public void ShenasnameByArea(IEnumerable<Exchange> exchanges, string path)
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
                    PrinterSettings(ws,1,eOrientation.Landscape);
                }
                
                p.Save();
            }

        }


    }
}
