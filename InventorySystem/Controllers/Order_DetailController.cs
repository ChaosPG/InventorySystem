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
    public class Order_DetailController : Controller
    {
        private readonly Data.InventorySystemContext _context;

        public Order_DetailController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Order_Detail
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var order_detail = from s in _context.Order_Detail.Include(p => p.Product).Include(p => p.Orders).Include(p => p.Payment)
                               select s;

            switch (sortOrder)
            {
                case "name_desc":
                    order_detail = order_detail.OrderByDescending(s => s.Product);
                    break;
                case "date_desc":
                    order_detail = order_detail.OrderByDescending(s => s.Order_Detail_Date);
                    break;
                default:
                    order_detail = order_detail.OrderBy(s => s.Product);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Order_Detail>.CreateAsync(order_detail.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize]
        // GET: Order_Detail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Detail = await _context.Order_Detail
                .Include(o => o.Orders)
                .Include(o => o.Payment)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Order_Detail_ID == id);
            if (order_Detail == null)
            {
                return NotFound();
            }

            return View(order_Detail);
        }

        [Authorize]
        // GET: Order_Detail/Create
        public IActionResult Create()
        {
            ViewData["Bill_Number"] = new SelectList(_context.Payment, "Bill_Number", "Payment_Type");
            ViewData["Order_ID"] = new SelectList(_context.Orders, "Order_ID", "Order_Details");
            ViewData["Product_ID"] = new SelectList(_context.Product, "Product_ID", "Product_Name");
            return View();
        }

        // POST: Order_Detail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Order_Detail_ID,Unit_Price,Size,Quantity,Discount,Total,Order_Detail_Date,Product_ID,Order_ID,Bill_Number")] Order_Detail order_Detail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_Detail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Bill_Number"] = new SelectList(_context.Payment, "Bill_Number", "Bill_Number",order_Detail.Bill_Number);
            ViewData["Order_ID"] = new SelectList(_context.Orders, "Order_ID", "Order_ID", order_Detail.Order_ID);
            ViewData["Product_ID"] = new SelectList(_context.Product, "Product_ID", "Product_Name",order_Detail.Product_ID);
            return View(order_Detail);
        }

        [Authorize]
        // GET: Order_Detail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Detail = await _context.Order_Detail.FindAsync(id);
            if (order_Detail == null)
            {
                return NotFound();
            }
            ViewData["Bill_Number"] = new SelectList(_context.Payment, "Bill_Number", "Payment_Type");
            ViewData["Order_ID"] = new SelectList(_context.Orders, "Order_ID", "Order_Details");
            ViewData["Product_ID"] = new SelectList(_context.Product, "Product_ID", "Product_Name");

            return View(order_Detail);
        }

        // POST: Order_Detail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Order_Detail_ID,Unit_Price,Size,Quantity,Discount,Total,Order_Detail_Date,Product_ID,Order_ID,Bill_Number")] Order_Detail order_Detail)
        {
            if (id != order_Detail.Order_Detail_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_Detail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_DetailExists(order_Detail.Order_Detail_ID))
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
            ViewData["Bill_Number"] = new SelectList(_context.Payment, "Bill_Number", "Bill_Number", order_Detail.Bill_Number);
            ViewData["Order_ID"] = new SelectList(_context.Orders, "Order_ID", "Order_ID", order_Detail.Order_ID);
            ViewData["Product_ID"] = new SelectList(_context.Product, "Product_ID", "Product_Name", order_Detail.Product_ID);
            return View(order_Detail);
        }

        [Authorize]
        // GET: Order_Detail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order_Detail = await _context.Order_Detail
                .Include(o => o.Orders)
                .Include(o => o.Payment)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Order_Detail_ID == id);
            if (order_Detail == null)
            {
                return NotFound();
            }

            return View(order_Detail);
        }

        // POST: Order_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order_Detail = await _context.Order_Detail.FindAsync(id);
            _context.Order_Detail.Remove(order_Detail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_DetailExists(int id)
        {
            return _context.Order_Detail.Any(e => e.Order_Detail_ID == id);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[10] { new DataColumn("Order_Detail_ID"),
                                        new DataColumn("Unit_Price"),
                                        new DataColumn("Size"),
                                        new DataColumn("Quantity"),
                                        new DataColumn("Discount"),
                                        new DataColumn("Total"),
                                        new DataColumn("Order_Detail_Date"),
                                        new DataColumn("Product_ID"),
                                        new DataColumn("Order_ID"),
                                        new DataColumn("Bill_Number")});

            var order_detail = from orderd in this._context.Order_Detail.Take(10)
                             select orderd;

            foreach (var orderd in order_detail)
            {
                dt.Rows.Add(orderd.Order_Detail_ID,orderd.Unit_Price, orderd.Size, orderd.Quantity, orderd.Discount, orderd.Total, orderd.Order_Detail_Date, orderd.Product_ID, orderd.Order_ID, orderd.Bill_Number);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrderDetail.xlsx");
                }
            }
        }
    }
}
