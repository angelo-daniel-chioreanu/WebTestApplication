using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTestApplication.Models;
using WebTestApplication.Data;

namespace WebTestApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly TestApplicationContext _context;

        public ContactsController(TestApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }
            
            return await _context.Contacts.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(long id)
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
            return NotFound();
            }

            return contact;
        }

        // GET: api/Contacts/name=John%20Smith
        [HttpGet("name={name}")]
        public async Task<ActionResult<IEnumerable<Contact>>> SearchContactByName(string name)
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }

            return await _context.Contacts.Where(contact => contact.Name == name).ToListAsync();
        }

        // GET: api/Contacts/birthDateMin=1970-01-01/birthDateMax=1972-02-02
        [HttpGet("birthDateMin={birthDateMin}/birthDateMax={birthDateMax}")]
        public async Task<ActionResult<IEnumerable<Contact>>> SearchContactByDate(string birthDateMin, string birthDateMax)
        {
            DateOnly birthDateMinDate =DateOnly.Parse(birthDateMin);
            DateOnly birthDateMaxDate = DateOnly.Parse(birthDateMax);

            if (_context.Contacts == null)
            {
                return NotFound();
            }

            return await _context.Contacts.Where(contact => contact.BirthDate >= birthDateMinDate && contact.BirthDate <= birthDateMaxDate).ToListAsync();
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(long id, Contact contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }
            
            if (contact.Emails.Count > 0 && contact.Emails.Where(e => e.IsPrimary).Count() != 1)
            {
                return ValidationProblem("When a contact has any emails, one and only one should be set to IsPrimary true");
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'TestApplicationContext.Contacts'  is null.");
            }
            
            if (contact.Emails.Count > 0 && contact.Emails.Where(e => e.IsPrimary).Count() != 1)
            {
                return ValidationProblem("When a contact has any emails, one and only one should be set to IsPrimary true");
            }

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(long id)
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactExists(long id)
        {
            return (_context.Contacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
