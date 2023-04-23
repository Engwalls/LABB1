using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LABB1.Data;
using LABB1.Models;
using System.Globalization;

namespace LABB1.Controllers
{
    public class LeaveController : Controller
    {
        private readonly Labb1Context _context;

        public LeaveController(Labb1Context context)
        {
            _context = context;
        }

        // GET: Leave
        public async Task<IActionResult> Index()
        {
            var labb1Context = _context.Leaves.Include(l => l.Employees);
            return View(await labb1Context.ToListAsync());
        }

        // GET: Leave/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leaves == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employees)
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // GET: Leave/Create
        public IActionResult Create()
        {
            ViewData["FK_EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            return View();
        }

        // POST: Leave/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveId,LeaveType,LeaveStart,LeaveStop,LeaveApply,FK_EmployeeId")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", leave.FK_EmployeeId);
            return View(leave);
        }

        // GET: Leave/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leaves == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            ViewData["FK_EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", leave.FK_EmployeeId);
            return View(leave);
        }

        // POST: Leave/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveId,LeaveType,LeaveStart,LeaveStop,LeaveApply,FK_EmployeeId")] Leave leave)
        {
            if (id != leave.LeaveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.LeaveId))
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
            ViewData["FK_EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", leave.FK_EmployeeId);
            return View(leave);
        }

        // GET: Leave/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leaves == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employees)
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // POST: Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leaves == null)
            {
                return Problem("Entity set 'Labb1Context.Leaves'  is null.");
            }
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Leaves
        public async Task<IActionResult> SearchForLeave()
        {
            var leaveDbContext = _context.Leaves.Include(v => v.Employees);
            return View(await leaveDbContext.ToListAsync());
        }

        // Post: Leaves/ShowLeaves
        public async Task<IActionResult> ShowLeaves(string LeaveSearchFirstName, string LeaveSearchLastName)
        {
            var leaveDbContext = _context.Leaves.Include(v => v.Employees);
            var results = await leaveDbContext.Where(v =>
                v.Employees.FirstName.Contains(LeaveSearchFirstName) &&
                v.Employees.LastName.Contains(LeaveSearchLastName)
            ).ToListAsync();

            return View("Index", results);
        }


        // GET: Leaves/LeaveAdmin
        public async Task<IActionResult> LeaveAdmin()
        {
            var leaveDbContext = _context.Leaves.Include(v => v.Employees);
            return View(await leaveDbContext.ToListAsync());
        }

        // Post: Leaves/ShowLeaveResults
        [HttpPost]
        public async Task<IActionResult> ShowLeaveResults(int MonthSelect)
        {
            var vacationDbContext = _context.Leaves.Include(v => v.Employees);
            return View("Index", await vacationDbContext.Where(v => v.LeaveApply.Month == MonthSelect).ToListAsync());
        }



        private bool LeaveExists(int id)
        {
          return (_context.Leaves?.Any(e => e.LeaveId == id)).GetValueOrDefault();
        }
    }
}
