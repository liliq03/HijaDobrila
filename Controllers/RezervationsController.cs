using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HijaDobrila2.Data;
using Microsoft.AspNetCore.Authorization;
using HijaDobrila2.Models;
using Microsoft.AspNetCore.Identity;
using System;

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
       /* public async Task<IActionResult> Test()
        {
            var userLoged = await _userManager.GetUserAsync(User);
            var result = await _userManager.AddToRoleAsync(userLoged, Roles.Admin.ToString());   //"Admin");
            var roles = _userManager.GetRolesAsync(userLoged);
            return Content("OK !!!");
        }
       */

        // GET: Rezervations
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))//ako e admin vijda vsichko
            {
                var applicationDbContext = _context.Rezervations
                .Include(o => o.Rooms)
                .Include(o => o.Users);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var currentUser = _userManager.GetUserId(User);
                var myOrders = _context.Rezervations
                               .Include(o => o.Rooms)
                               .Include(u => u.Users)
                               .Where(x => x.IdUser == currentUser.ToString())
                               .ToListAsync();

                return View(await myOrders);
            }

        }

        // GET: Rezervations
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index1()
        {
            var applicationDbContext = _context.Rezervations
                .Include(o => o.Rooms)
                .Include(o => o.Users);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rezervations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervation = await _context.Rezervations
                 .Include(o => o.Rooms)
                .Include(u => u.Users) //ako go nqma nqmam dostyp do poletata na User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervation == null)
            {
                return NotFound();
            }

            return View(rezervation);
        }

        // GET: Rezervations/Create  
        [Authorize(Roles = "User, Admin")]
        public IActionResult Create()
        {
            RezervationsVM model = new RezervationsVM();

           // model.IdUser = _userManager.GetUserId(User);
            model.Rooms = _context.Rooms.Select(x => new SelectListItem
            {

                Text = x.RoomNum.ToString(),
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
        public async Task<IActionResult> Create([Bind("Id,IdRoom,IdUser,AdultsNum,ChildrensNum,DateArrived,DateLeft,DateRezervation")]
                                                                                            RezervationsVM rezervation)
                                                                                                                        
        {
            if (ModelState.IsValid)
            {
                RezervationsVM model = new RezervationsVM();
                model.Rooms = _context.Rooms.Select(x => new SelectListItem
                {
                    Text = x.RoomNum.ToString(),
                    Value = x.Id.ToString(),
                    Selected = (x.Id == model.IdRoom)
                }
                ).ToList();
                return View(model);
            }

            Rezervation modelToDB = new Rezervation
            {
                IdRoom = rezervation.IdRoom,
                IdUser = _userManager.GetUserId(User),
                DateRezervation = DateTime.Now
            };
            _context.Add(rezervation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            RezervationsVM model = new RezervationsVM();
            //  зареждаме падащ списък с всички стаи от БД
            model.Rooms = _context.Rooms.Select(pr => new SelectListItem
            {
                Value = pr.Id.ToString(),
                Text = pr.Rezervations.ToString(),
                Selected = pr.Id == model.IdRoom
            }
            ).ToList();
            return View(model);
        }

        // POST: Rezervations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRoom,IdUser,AdultsNum,ChildrensNum,DateArrived," +
                                                                 "DateLeft,DateRezervation")] RezervationsVM rezervation)
        {
            if (id != rezervation.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) //ако моделът не е ОК
            {
                //презареждаме страницата
                return View(rezervation);
            }

            Rezervation modeFromDB = new Rezervation
            {
                Id = id,
                IdUser = _userManager.GetUserId(User),
                IdRoom = rezervation.IdRoom,
                DateRezervation = DateTime.Now
            };
            try
            {
                _context.Update(modeFromDB);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RezervationExists(modeFromDB.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //4. Извикваме Details на актуализирания запис
            return RedirectToAction(" Details ", new { id = id });
        }
        // GET: Rezervations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rezervation = await _context.Rezervations
                    .Include(o => o.Rooms)
                    .Include(u => u.Users)
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
            var rezervation = await _context.Rezervations
                .Include(u => u.Users)
                .FirstOrDefaultAsync(x => x.Id == id);
            // .FindAsync(id);
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


   

   


