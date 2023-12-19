using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ExpenseTracker.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Auth
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Users'  is null.");
        }

        // GET: Auth/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Auth/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auth/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,UserEmail,Password,CreatedAt")] User user)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = hashedPassword;
                user.CreatedAt = DateTime.Now;

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Auth/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Auth/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserEmail,Password,CreatedAt")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: Auth/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Auth/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'AppDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLogin loggedUser)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserEmail == loggedUser.Email);

                if (user != null)
                {
                    bool passwordMatches = BCrypt.Net.BCrypt.Verify(loggedUser.Password, user.Password);

                    if (passwordMatches)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserEmail),
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) 
                        };

                        HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "/");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }

            
            return View(loggedUser);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "/");
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
