using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HijaDobrila2.Data;
using Microsoft.AspNetCore.Authorization;
using HijaDobrila2.Models;
using Microsoft.AspNetCore.Identity;

namespace HijaDobrila2.Controllers
{
      [Authorize]
    public class RezervationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
       // private readonly SignInManager<User> _signInManager;
       // private readonly RoleManager<IdentityRole> _roleManager;
        public RezervationsController(ApplicationDbContext context, UserManager<User> userManager)
        //RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        // GET: Rezervations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rezervations.ToListAsync());
        }

        // GET: Rezervations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervation == null)
            {
                return NotFound();
            }

            return View(rezervation);
        }

        // GET: Rezervations/Create  
        [Authorize(Roles ="User, Admin")]
        public IActionResult Create()
        {
            RezervationsVM model = new RezervationsVM();
            
            model.IdUser= _userManager.GetUserId(User);
            model.Rooms = _context.Rooms.Select(x => new SelectListItem
            {
                
                Text = x.Description,
                 Value = x.Id.ToString(),
                Selected = (x.Id == model.IdRoom)
            }
            ).ToList();
            return View(model);
        }
       
        // POST: Rezervations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRoom,IdUser,AdultsNum,ChildrensNum,DateArrived,DateLeft,DateRezervation")] Rezervation rezervation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rezervation);
        }

        // GET: Rezervations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations.FindAsync(id);
            if (rezervation == null)
            {
                return NotFound();
            }
            return View(rezervation);
        }

        // POST: Rezervations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRoom,IdUser,AdultsNum,ChildrensNum,DateArrived,DateLeft,DateRezervation")] Rezervation rezervation)
        {
            if (id != rezervation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervationExists(rezervation.Id))
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
            return View(rezervation);
        }

        // GET: Rezervations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervation == null)
            {
                return NotFound();
            }

            return View(rezervation);
        }

        // POST: Rezervations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervation = await _context.Rezervations.FindAsync(id);
            _context.Rezervations.Remove(rezervation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervationExists(int id)
        {
            return _context.Rezervations.Any(e => e.Id == id);
        }
    }
}
