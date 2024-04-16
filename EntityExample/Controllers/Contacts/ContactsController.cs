using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntityExample.Data;
using EntityExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;


namespace EntityExample.Controllers.Contacts
{
	[Authorize]

	public class ContactsController : Controller
    {
        private readonly ApplicationContext _context;

       
        public ContactsController(ApplicationContext context)
        {
            _context = context;
            
        }

        // GET: Contacts
        
        public async Task<IActionResult> Index()
        {

            return View(await _context.Contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,City,EmailAddress,Name,Status")] Models.Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                contacts.Status = Status.Submitted;
                _context.Add(contacts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contacts);
        }

		// GET: Contacts/Edit/5
		[Authorize(Policy = "EditingAndDeletion")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts == null)
            {
                return NotFound();
            }

			return View(contacts);
        }

        // POST: Contacts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,City,EmailAddress,Name")] Models.Contacts contacts)
        {
            if (id != contacts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //contacts.Status = Status.Submitted;
                    var record = _context.Contacts.AsNoTracking().FirstOrDefault(c => c.Id == contacts.Id);
                    contacts.Status = record.Status;
                    _context.Update(contacts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactsExists(contacts.Id))
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
            return View(contacts);
        }

		// GET: Contacts/Delete/5
		[Authorize(Policy = "EditingAndDeletion")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacts = await _context.Contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contacts == null)
            {
                return NotFound();
            }

            return View(contacts);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contacts = await _context.Contacts.FindAsync(id);
            if (contacts != null)
            {
                _context.Contacts.Remove(contacts);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactsExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id) ?? throw new Exception("Contact not found");
				contact.Status = Status.Approved;
                _context.Update(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return RedirectToAction(nameof(Index));
			}
			
		}

        public async Task<IActionResult> Reject(int id)
        {
			try
            {
				var contact = await _context.Contacts.FindAsync(id) ?? throw new Exception("Contact not found");
				contact.Status = Status.Rejected;
				_context.Update(contact);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				return RedirectToAction(nameof(Index));
			}

		}
    }
}
