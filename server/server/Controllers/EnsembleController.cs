using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    public class EnsembleController : Controller
    {

        private readonly PluggedContext _context;

        public EnsembleController(PluggedContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEnsemble([Bind("Ensemble_Name, Formed_Date, Disbanded_Date, Type, Genre, Bio")] Ensemble ensemble)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ensemble);
                await _context.SaveChangesAsync();
            }

                return View();
        }
    }
}