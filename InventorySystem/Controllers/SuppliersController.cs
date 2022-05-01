using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using InventorySystem.OtherClass;
using System.Globalization;

namespace InventorySystem.Controllers
{
    public class SuppliersController : Controller
    {

        private readonly Data.InventorySystemContext _context;

        public SuppliersController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Suppliers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "sup_name" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var supplier = from s in _context.Supplier
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                supplier = supplier.Where(s => s.Supplier_Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "sup_name":
                    supplier = supplier.OrderByDescending(s => s.Supplier_Name);
                    break;
                default:
                    supplier = supplier.OrderBy(s => s.Supplier_Name);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Supplier>.CreateAsync(supplier.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await categories.AsNoTracking().ToListAsync());
        }

        [Authorize]
        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.Supplier_ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [Authorize]
        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Supplier_ID,Supplier_Name,Supplier_Address,Supplier_Phone,Supplier_Fax,Supplier_Email")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        [Authorize]
        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Supplier_ID,Supplier_Name,Supplier_Address,Supplier_Phone,Supplier_Fax,Supplier_Email")] Supplier supplier)
        {
            if (id != supplier.Supplier_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Supplier_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        [Authorize]
        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .FirstOrDefaultAsync(m => m.Supplier_ID == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await _context.Supplier.FindAsync(id);
            _context.Supplier.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return _context.Supplier.Any(e => e.Supplier_ID == id);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Supplier_ID"),
                                        new DataColumn("Supplier_Name"),
                                        new DataColumn("Supplier_Address"),
                                        new DataColumn("Supplier_Phone"),
                                        new DataColumn("Supplier_Fax"),
                                        new DataColumn("Supplier_Email")});

            var suppliers = from supplier in this._context.Supplier.Take(10)
                             select supplier;

            foreach (var supplier in suppliers)
            {
                dt.Rows.Add(supplier.Supplier_ID, supplier.Supplier_Name, supplier.Supplier_Address, supplier.Supplier_Phone,supplier.Supplier_Fax,supplier.Supplier_Email);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Suppliers.xlsx");
                }
            }
        }
    }
}
