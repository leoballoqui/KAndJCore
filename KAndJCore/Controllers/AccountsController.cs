using KAndJCore.Data;
using KAndJCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("clientId", id.ToString());

            var applicationDbContext = _context.Account.Where(a => a.ClientId == id).Include(a => a.AccountType).Include(a => a.Reason);
            ViewData["Client"] = client;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.AccountType)
                .Include(a => a.Client)
                .Include(a => a.Reason)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["Client"] = client;
            ViewData["AccountTypeId"] = new SelectList(_context.AccountType, "Id", "Name");
            ViewData["ReasonId"] = new SelectList(_context.Reason, "Id", "Value");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,AccountTypeId,ReasonId,Alias,Owner,Notes")] Account account)
        {
            if (ModelState.IsValid)
            {
                account.Id = Guid.NewGuid();
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = account.ClientId });
            }
            ViewData["Client"] = await _context.Client.FirstOrDefaultAsync(c => c.Id == account.ClientId);
            ViewData["AccountTypeId"] = new SelectList(_context.AccountType, "Id", "Name");
            ViewData["ReasonId"] = new SelectList(_context.Reason, "Id", "Value", account.ReasonId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["Client"] = await _context.Client.FirstOrDefaultAsync(c => c.Id == account.ClientId);
            ViewData["AccountTypeId"] = new SelectList(_context.AccountType, "Id", "Name", account.AccountTypeId);
            ViewData["ReasonId"] = new SelectList(_context.Reason, "Id", "Value", account.ReasonId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClientId,AccountTypeId,Alias,Owner,ReasonId,Notes")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = account.ClientId });
            }
            ViewData["Client"] = await _context.Client.FirstOrDefaultAsync(c => c.Id == account.ClientId);
            ViewData["AccountTypeId"] = new SelectList(_context.AccountType, "Id", "Name", account.AccountTypeId);
            ViewData["ReasonId"] = new SelectList(_context.Reason, "Id", "Value", account.ReasonId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.AccountType)
                .Include(a => a.Client)
                .Include(a => a.Reason)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var account = await _context.Account.FindAsync(id);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = account.ClientId });
        }

        public async Task<JsonResult> AccountsByClient(Guid? id)
        {
            if (id == null)
            {
                return Json(new { Success = "False", responseText = "Invalid Id" });
            }
            var client = await _context.Client.FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return Json(new { Success = "False", responseText = "Client Not Found" });
            }

            var accounts = await _context.Account
                .Include(a => a.Reason)
                .Where( a=> a.ClientId == id && a.Status == 0)
                .ToListAsync();

            return Json(new { Success = "True", accounts = accounts, address = client.FullAddress });
        }

        public IActionResult Back()
        {
            Guid clientId;
            if (Guid.TryParse(HttpContext.Session.GetString("clientId"), out clientId))
                return RedirectToAction(nameof(Index), new { id = clientId });
            else
                return RedirectToAction("Index", "Clients");
        }

        private bool AccountExists(Guid id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
    }
}
