using Chapeau.Models;
using Chapeau.Models.VeiwModels;
using Chapeau.Repositories.Interface;
using Chapeau.Repositories.Interfaces;
using Chapeau.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class BillController : Controller
    {
        private readonly BillService _billService;

        public BillController(BillService billService)
        {
            _billService = billService;
        }

        [HttpGet]
        public IActionResult Overview(int boothId)
        {
            TableOrderViewModel viewModel = _billService.GetTableOrderOverview(boothId);

            if (viewModel == null)
            {
                return NotFound("There is no active order sitting at this table right now.");
            }

            return View(viewModel);
        }
        public IActionResult ProcessPayment(ProcessPaymentViewModel formData)
        {
            try
            {
                Bill completedBill = _billService.ProcessCheckout(formData);

               
                decimal grandTotal = completedBill.Totall_price + (decimal)completedBill.Tip_Amount;

               
                TempData["SuccessMessage"] = $"Order for Table {formData.BoothId} finished successfully! Total Paid: €{grandTotal.ToString("0.00")}";


                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
               
                TempData["ErrorMessage"] = "Something went wrong during payment: " + ex.Message;
                return RedirectToAction("Overview", new { boothId = formData.BoothId });
            }
        }

    }
}
    


