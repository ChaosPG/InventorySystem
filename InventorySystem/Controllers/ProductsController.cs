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

namespace InventorySystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Data.InventorySystemContext _context;

        public ProductsController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Products
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "productName" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var product = from s in _context.Product.Include(p => p.Category).Include(p => p.Supplier)
                          select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(s => s.Product_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "productName":
                    product = product.OrderByDescending(s => s.Product_Name);
                    break;
                default:
                    product = product.OrderBy(s => s.Product_Name);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Product>.CreateAsync(product.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await categories.AsNoTracking().ToListAsync());
        }

        [Authorize]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Category_ID"] = new SelectList(_context.Category, "Category_ID", "Category_Name");
            ViewData["Supplier_ID"] = new SelectList(_context.Supplier, "Supplier_ID", "Supplier_Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product_ID,Product_Name,Product_Description,Product_Unit,Product_Price,Product_Quantity,Other_Details,Supplier_ID,Category_ID")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Category_ID"] = new SelectList(_context.Category, "Category_ID", "Category_ID", product.Category_ID);
            ViewData["Supplier_ID"] = new SelectList(_context.Supplier, "Supplier_ID", "Supplier_ID", product.Supplier_ID);
            return View(product);
        }

        [Authorize]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Category_ID"] = new SelectList(_context.Category, "Category_ID", "Category_Name");
            ViewData["Supplier_ID"] = new SelectList(_context.Supplier, "Supplier_ID", "Supplier_Name");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Product_ID,Product_Name,Product_Description,Product_Unit,Product_Price,Product_Quantity,Other_Details,Supplier_ID,Category_ID")] Product product)
        {
            if (id != product.Product_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Product_ID))
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
            ViewData["Category_ID"] = new SelectList(_context.Category, "Category_ID", "Category_ID", product.Category_ID);
            ViewData["Supplier_ID"] = new SelectList(_context.Supplier, "Supplier_ID", "Supplier_ID", product.Supplier_ID);
            return View(product);
        }

        [Authorize]
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Product_ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Product_ID == id);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[9] { new DataColumn("Product_ID"),
                                        new DataColumn("Product_Name"),
                                        new DataColumn("Product_Description"),
                                        new DataColumn("Product_Unit"),
                                        new DataColumn("Product_Price"),
                                        new DataColumn("Product_Quantity"),
                                        new DataColumn("Other_Details"),
                                        new DataColumn("Supplier_ID"),
                                        new DataColumn("Category_ID") });

            var products = from product in this._context.Product.Take(10)
                             select product;

            foreach (var product in products)
            {
                dt.Rows.Add(product.Product_ID, product.Product_Name, product.Product_Description,product.Product_Unit,product.Product_Price,product.Product_Quantity,product.Other_Details,product.Supplier_ID,product.Category_ID);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Products.xlsx");
                }
            }
        }
    }
}
