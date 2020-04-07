using KAndJCore.Data;
using KAndJCore.Enums;
using KAndJCore.Models;
using KAndJCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAndJCore.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClaimsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Claims
        public async Task<IActionResult> Index(string key = "")
        {
            if (String.IsNullOrEmpty(key))
            {
                var applicationDbContext = _context.Claim.OrderByDescending(c => c.Created).Take(100).Include(c => c.Buro).Include(c => c.Client).Include(c => c.Template).Include(c => c.Disputes).ThenInclude(d => d.Account);
                return View(await applicationDbContext.ToListAsync());
            }
            var dbContext = _context.Claim.Where(c => c.Client.LastName.ToLower().Contains(key.ToLower()) || c.Client.Name.ToLower().Contains(key.ToLower())).OrderByDescending(c => c.Created).Include(c => c.Buro).Include(c => c.Client).Include(c => c.Template).Include(c => c.Disputes).ThenInclude(d => d.Account);
            return View(await dbContext.ToListAsync());
        }

        public async Task<IActionResult> Pending(string key = "")
        {
            if (String.IsNullOrEmpty(key))
            {
                var applicationDbContext = _context.Claim.Where(c => c.NextRevision <= DateTime.Now.Date).OrderBy(c => c.NextRevision).Take(100).Include(c => c.Buro).Include(c => c.Client).Include(c => c.Template).Include(c => c.Disputes).ThenInclude(d => d.Account);
                return View(await applicationDbContext.ToListAsync());
            }
            var dbContext = _context.Claim.Where(c => c.NextRevision <= DateTime.Now.Date && (c.Client.LastName.ToLower().Contains(key.ToLower()) || c.Client.Name.ToLower().Contains(key.ToLower()))).OrderByDescending(c => c.Created).Include(c => c.Buro).Include(c => c.Client).Include(c => c.Template).Include(c => c.Disputes).ThenInclude(d => d.Account);
            return View(await dbContext.ToListAsync());
        }

        // GET: Claims/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim
                .Include(c => c.Buro)
                .Include(c => c.Client)
                .Include(c => c.Template)
                .Include(c => c.Disputes)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // GET: Claims/Create
        public IActionResult Create(string key = "")
        {
            ViewData["Buroes"] = _context.Buro.ToList();
            ViewData["TemplateId"] = new SelectList(_context.Template.OrderBy(t => t.Name), "Id", "Name");

            if (String.IsNullOrEmpty(key))
                ViewData["ClientId"] = new SelectList(_context.Client.Take(100).OrderByDescending(c => c.Created), "Id", "Name");
            else
                ViewData["ClientId"] = new SelectList(_context.Client.Where(c => c.LastName.ToLower().Contains(key.ToLower()) || c.Name.ToLower().Contains(key.ToLower())).OrderByDescending(c => c.Created), "Id", "Name");

            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,TemplateId,IncludedBuros,IncludedAccounts,Notes")] ClaimVM claimVM)
        {
            if (ModelState.IsValid)
            {
                if (claimVM.IncludedBuros.EndsWith(";"))
                    claimVM.IncludedBuros = claimVM.IncludedBuros.Remove(claimVM.IncludedBuros.Length - 1,  1);
                List<string> buros = claimVM.IncludedBuros.Split(';').ToList();
                int index = _context.Claim.Count() + 1;
                foreach (var buro in buros)
                {
                    Claim claim = new Claim
                    {
                        Id = Guid.NewGuid(),
                        ClientId = claimVM.ClientId,
                        TemplateId = claimVM.TemplateId,
                        BuroId = Guid.Parse(buro),
                        ClaimNumber = $"{DateTime.Now.ToString("yyyyMMdd")}-{index}",
                        Created = DateTime.Now,
                        NextRevision = DateTime.Now.AddDays(30),
                        Notes = claimVM.Notes,
                        Status = ClaimStatusEnum.Open,
                        CurrentIteration = 1,
                    };
                    _context.Add(claim);

                    claimVM.DisputeAccounts = (List<DisputeVM>)JsonConvert.DeserializeObject(claimVM.IncludedAccounts, typeof(List<DisputeVM>));
                    foreach (var account in claimVM.DisputeAccounts)
                    {
                        Dispute dispute = new Dispute
                        {
                            Id = Guid.NewGuid(),
                            ClaimId = claim.Id,
                            AccountId = account.AccountId,
                            CompleteReason = account.CompleteReason
                        };
                        _context.Add(dispute);
                    }
                    index++;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Buroes"] = _context.Buro.ToList();
            ViewData["ClientId"] = new SelectList(_context.Client, "Id", "Name", claimVM.ClientId);
            ViewData["TemplateId"] = new SelectList(_context.Template, "Id", "Name", claimVM.TemplateId);
            return View(claimVM);
        }

        // GET: Claims/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim
                .Include(c => c.Buro)
                .Include(c => c.Client)
                .Include(c => c.Template)
                .Include(c => c.Disputes)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }
            ViewData["TemplateId"] = new SelectList(_context.Template.OrderBy(t => t.Name), "Id", "Name", claim.TemplateId);
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TemplateId,Status,NextRevision,Notes")] Claim claim)
        {
            if (id != claim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dbclaim = await _context.Claim.FirstOrDefaultAsync(m => m.Id == id);
                    dbclaim.TemplateId = claim.TemplateId;
                    dbclaim.Status = claim.Status;
                    dbclaim.NextRevision = claim.NextRevision;
                    dbclaim.Notes = claim.Notes;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClaimExists(claim.Id))
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
            ViewData["TemplateId"] = new SelectList(_context.Template, "Id", "Name", claim.TemplateId);
            return View(claim);
        }

        // GET: Claims/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim
                .Include(c => c.Buro)
                .Include(c => c.Client)
                .Include(c => c.Template)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }

            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var claim = await _context.Claim.FindAsync(id);
            _context.Claim.Remove(claim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditDisputes(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim
                .Include(c => c.Disputes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (claim == null)
            {
                return NotFound();
            }
            return View(new ClaimVM(claim));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDisputes(Guid id, [Bind("Id, IncludedAccounts")] ClaimVM claimVM)
        {
            if (id != claimVM.Id)
            {
                return NotFound();
            }
            var claim = await _context.Claim
                        .Include(c => c.Disputes)
                        .FirstOrDefaultAsync(c => c.Id == id);
            if (claim == null)
            {
                return NotFound();
            }
            if (claim.Disputes.Count > 0)
            {
                Dictionary<Guid, DisputeVM> disputes = ((List<DisputeVM>)JsonConvert.DeserializeObject(claimVM.IncludedAccounts, typeof(List<DisputeVM>))).ToDictionary(d => d.Id, d => d);
                foreach (var dispute in claim.Disputes)
                {
                    if (disputes.ContainsKey(dispute.Id))
                        dispute.CompleteReason = disputes[dispute.Id].CompleteReason;
                    else
                        _context.Dispute.Remove(dispute);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> NextRevision(Guid id)
        {
            if (id == null)
                return Json(new { Success = "False", responseText = "Invalid Claim Id" });

            try
            {
                var dbclaim = await _context.Claim.FirstOrDefaultAsync(m => m.Id == id);
                if (dbclaim.CurrentIteration < 3)
                {
                    dbclaim.CurrentIteration++;
                    dbclaim.NextRevision = dbclaim.NextRevision.AddDays(30);
                    await _context.SaveChangesAsync();
                    return Json(new { newDate = dbclaim.NextRevision.ToString("MM/dd/yyyy"), revision = dbclaim.CurrentIteration });
                }
                else
                    return Json(new { Success = "False", responseText = "The claim is at its last revision" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
                    return Json(new { Success = "False", responseText = "Claim Not Found" });
                else
                    throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> CloseClaim(Guid id)
        {
            if (id == null)
                return Json(new { Success = "False", responseText = "Invalid Claim Id" });

            try
            {
                var dbclaim = await _context.Claim.FirstOrDefaultAsync(m => m.Id == id);
                if (dbclaim.CurrentIteration >= 3)
                {
                    dbclaim.Status = ClaimStatusEnum.Closed;
                    await _context.SaveChangesAsync();
                    return Json(new { Success = "True" });
                }
                else
                    return Json(new { Success = "False", responseText = "The claim still has some pending revisions" });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
                    return Json(new { Success = "False", responseText = "Claim Not Found" });
                else
                    throw;
            }
        }

        [HttpPost]
        public async Task<JsonResult> ReopenClaim(Guid id)
        {
            if (id == null)
                return Json(new { Success = "False", responseText = "Invalid Claim Id" });

            try
            {
                var dbclaim = await _context.Claim.FirstOrDefaultAsync(m => m.Id == id);
                dbclaim.Status = ClaimStatusEnum.Open;
                dbclaim.CurrentIteration = 1;
                dbclaim.NextRevision = dbclaim.Created.AddDays(30);
                await _context.SaveChangesAsync();
                return Json(new { newDate = dbclaim.NextRevision.ToString("MM/dd/yyyy")});
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id))
                    return Json(new { Success = "False", responseText = "Claim Not Found" });
                else
                    throw;
            }
        }

        public async Task<IActionResult> Claim(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var claim = await _context.Claim.Include(c => c.Buro).Include(c => c.Client).Include(c => c.Template).Include(c => c.Disputes).FirstOrDefaultAsync(c => c.Id == id);
            return View(claim);
        }

        public async Task<IActionResult> ClaimFile(Guid? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var dbclaim = await _context.Claim.Include(c => c.Buro).Include(c => c.Client).FirstOrDefaultAsync(c => c.Id == id);
                if (dbclaim == null)
                    return NotFound();

                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertUrl(Url.Action("Claim", "Claims", new { id = id }, Request.Scheme));
                string path = String.Format("{0}\\documents\\{1}.pdf", this._hostingEnvironment.WebRootPath, id);
                doc.Save(path);
                doc.Close();

                string name = $"Dispute_{dbclaim.ClaimNumber}_{dbclaim.Client.FullName.Replace(" ", "_")}_{dbclaim.Buro.Name}.pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                System.IO.File.Delete(path);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id.Value))
                    return Json(new { Success = "False", responseText = "Claim Not Found" });
                else
                    throw;
            }
        }

        public async Task<IActionResult> ClaimClone(Guid? id)
        {
            if (id == null)
                return NotFound();

            try
            {
                var dbclaim = await _context.Claim.Include(c => c.Disputes).FirstOrDefaultAsync(c => c.Id == id);
                if (dbclaim == null)
                    return NotFound();

                int index = _context.Claim.Count() + 1;
                Claim newClaim = new Claim() {
                    Id = Guid.NewGuid(),
                    ClientId = dbclaim.ClientId,
                    BuroId = dbclaim.BuroId,
                    TemplateId = dbclaim.TemplateId,
                    ClaimNumber = $"{DateTime.Now.ToString("yyyyMMdd")}-{index}",
                    Created = DateTime.Now,
                    NextRevision = DateTime.Now.AddDays(30),
                    CurrentIteration = 1,
                    Status = ClaimStatusEnum.Open,
                    Notes = $"Created From: {dbclaim.ClaimNumber}\nImported Notes: \n {dbclaim.Notes}",
                };
                _context.Claim.Add(newClaim);

                foreach (var dispute in dbclaim.Disputes)
                {
                    Dispute newDispute = new Dispute()
                    {
                        Id = Guid.NewGuid(),
                        ClaimId = newClaim.Id,
                        AccountId = dispute.AccountId,
                        CompleteReason = dispute.CompleteReason,
                    };
                    _context.Dispute.Add(newDispute);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClaimExists(id.Value))
                    return Json(new { Success = "False", responseText = "Claim Not Found" });
                else
                    throw;
            }
        }

        private bool ClaimExists(Guid id)
        {
            return _context.Claim.Any(e => e.Id == id);
        }
    }
}
