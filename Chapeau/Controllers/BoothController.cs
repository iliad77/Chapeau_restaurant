using Chapeau.Models;
using Chapeau.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class BoothController : Controller
    {
        private readonly IBoothService _boothService;

        public BoothController(IBoothService boothService)
        {
            _boothService = boothService;
        }

        // GET: /Booth/Index
        public IActionResult Index()
        {
            List<Booth> booths = _boothService.GetAllBooth();
            return View(booths);
        }

        // GET: /Booth/Details/5
        public IActionResult detail(int id)
        {
            Booth booth = _boothService.GetBooth(id);

            if (booth == null)
            {
                return RedirectToAction("Index");
            }

            return View(booth);
        }

        // GET: /Booth/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Booth/Create
        [HttpPost]
        public IActionResult Create(Booth booth)
        {
            _boothService.createBooth(booth);
            return RedirectToAction("Index");
        }

        // GET: /Booth/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Booth booth = _boothService.GetBooth(id);

            if (booth == null)
            {
                return RedirectToAction("Index");
            }

            return View(booth);
        }

        // POST: /Booth/Edit
        [HttpPost]
        public IActionResult Edit(Booth booth)
        {
            _boothService.updateBooth(booth);
            return RedirectToAction("Index");
        }

        // GET: /Booth/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Booth booth = _boothService.GetBooth(id);

            if (booth == null)
            {
                return RedirectToAction("Index");
            }

            return View(booth);
        }

        // POST: /Booth/DeleteConfirmed
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _boothService.deleteBooth(id);
            return RedirectToAction("Index");
        }
    }
}
