using ClosedXML.Excel;
using InventorySystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystem.Controllers
{
    public class ExportExcelController : Controller
    {
        private readonly Data.InventorySystemContext _context;

        public ExportExcelController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> categories = (from category in this._context.Category.Take(10)
                                        select category).ToList();
            return View(categories);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Category_ID"),
                                        new DataColumn("Category_Name"),
                                        new DataColumn("Category_Description") });

            var categories = from category in this._context.Category.Take(10)
                            select category;

            foreach (var category in categories)
            {
                dt.Rows.Add(category.Category_ID, category.Category_Name, category.Category_Description);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }
    }
}
