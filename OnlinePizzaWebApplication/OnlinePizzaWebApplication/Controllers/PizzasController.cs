using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlinePizzaWebApplication.Models;
using OnlinePizzaWebApplication.Repository;

namespace OnlinePizzaWebApplication.Controllers
{
    public class PizzasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPizzaRepository _pizzaRepo;

        public PizzasController(AppDbContext context, IPizzaRepository pizzaRepo)
        {
            _context = context;
            _pizzaRepo = pizzaRepo;
        }

        // GET: Pizzas
        public async Task<IActionResult> Index()
        {
            return View(await _pizzaRepo.GetAllAsync());
        }

        // GET: Pizzas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _pizzaRepo.GetPizzaByIdAsync(id);

            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        // GET: Pizzas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pizzas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,Image,IsPizzaOfTheWeek")] Pizza pizza)
        {
            if (ModelState.IsValid)
            {
                _pizzaRepo.Add(pizza);
                await _pizzaRepo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pizza);
        }

        // GET: Pizzas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _pizzaRepo.GetPizzaByIdAsync(id);
            if (pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }

        // POST: Pizzas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,Image,IsPizzaOfTheWeek")] Pizza pizza)
        {
            if (id != pizza.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _pizzaRepo.Update(pizza);
                    await _pizzaRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaExists(pizza.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(pizza);
        }

        // GET: Pizzas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizza = await _pizzaRepo.GetPizzaByIdAsync(id);

            if (pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizza = await _pizzaRepo.GetPizzaByIdAsync(id);
            _pizzaRepo.Remove(pizza);
            await _pizzaRepo.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PizzaExists(int id)
        {
            return _pizzaRepo.PizzaExists(id);
        }
    }
}