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
        public RezervationsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            if (User.IsInRole("Admin"))
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
                               .Where(x => x.UserId == currentUser.ToString())
                               .ToListAsync();

                return View(await myOrders);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index1()
        {
            var applicationDbContext = _context.Rezervations
                .Include(o => o.Rooms)
                .Include(o => o.Users);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
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

        [Authorize(Roles = "User, Admin")]
        public IActionResult Create()
        {
            RezervationsVM model = new RezervationsVM();
            //model.Rooms = _context.Rooms.Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.Description.ToString(),
            //    Selected = (x.Id == model.RoomId)
            //}
            //).ToList();
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "RoomNum");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRoom,AdultsNum,ChildrensNum,DateArrived,DateLeft,DateRezervation")]
                                                                                            RezervationsVM rezervation)

        {
            if (!ModelState.IsValid)
            {
                RezervationsVM model = new RezervationsVM();
                model.Rooms = _context.Rooms.Select(x => new SelectListItem
                {
                    Text = x.RoomNum.ToString(),
                    Value = x.Id.ToString(),
                    Selected = (x.Id == model.RoomId)
                }
                ).ToList();
                return View(model);
            }

            Rezervation modelToDB = new Rezervation
            {
                RoomId = rezervation.RoomId,
                UserId = _userManager.GetUserId(User),
                DateRezervation = DateTime.Now,
                AdultsNum = rezervation.AdultsNum,
                DateArrived = rezervation.DateArrived,
                DateLeft = rezervation.DateLeft,
                ChildrensNum = rezervation.ChildrensNum
            };

            _context.Add(modelToDB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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
            model.Rooms = _context.Rooms.Select(pr => new SelectListItem
            {
                Value = pr.Id.ToString(),
                Text = pr.RoomNum.ToString(),
                Selected = pr.Id == model.RoomId

            }).ToList();
            model.AdultsNum = rezervation.AdultsNum;
            model.ChildrensNum = rezervation.ChildrensNum;
            model.DateArrived = rezervation.DateArrived;
            model.DateLeft = rezervation.DateLeft;
            model.RoomId = rezervation.RoomId;
            model.UserId = rezervation.UserId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRoom,AdultsNum,ChildrensNum,DateArrived," +
                                                                 "DateLeft,DateRezervation")] RezervationsVM rezervation)
        {
            if (id != rezervation.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(rezervation);
            }
            Rezervation modeFromDB = new Rezervation
            {
                Id = id,
                RoomId = rezervation.RoomId,
                DateRezervation = DateTime.Now,
                DateArrived = rezervation.DateArrived.Date,
                DateLeft = rezervation.DateLeft.Date,
                AdultsNum = rezervation.AdultsNum,
                ChildrensNum = rezervation.ChildrensNum
            };
            try
            {
                _context.Rezervations.Update(modeFromDB);
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
            return RedirectToAction("Details", new { id = id });
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervation = await _context.Rezervations
                .Include(u => u.Users)
                .FirstOrDefaultAsync(x => x.Id == id);
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







