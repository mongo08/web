using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private MobileContext db;
        public HomeController(MobileContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Phones.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Phone phone)
        {
            db.Phones.Add(phone);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                Phone phone = await db.Phones.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                    return View(phone);
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Phone phone = await db.Phones.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                    return View(phone);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Phone phone)
        {
            db.Phones.Update(phone);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Phone phone = await db.Phones.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                    return View(phone);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Phone phone = new Phone { Id = id.Value };
                db.Entry(phone).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
