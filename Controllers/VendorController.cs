using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infinterest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Infinterest.Controllers
{
    public class VendorController : Controller
    {
        private Context _context;
        public VendorController(Context context)
        {
            _context = context;
        }

        [HttpGet("dashboardvendor")]
        public IActionResult DashboardVendor()
        {

            int? ID = HttpContext.Session.GetInt32("userid");           
            Vendor user = _context.users
                .OfType<Vendor>()
                .Where(vendor => vendor.UserId == ID)
                .FirstOrDefault();
            
            if (user == null)
            {
                return Redirect("/");
            }

            DashboardVendorView viewModel = new DashboardVendorView();
            
            viewModel.allListings = _context.listings
                                    .ToList();

            if(user.Events != null)
            {
                viewModel.usersEvents = user.Events.Select(s => s.Event).ToList();
            }

            return View ("DashboardVendor", viewModel);
        }

        [HttpGet("vendor-registration")]
        public IActionResult VendorRegistration()
        {

            return View();
        }
        

        [HttpPost("vendor-registration")]
        public IActionResult CreateVendor(User UserInput)
        {
            if (ModelState.IsValid)
            {
                System.Console.WriteLine("Model IS Valid");
                if(_context.users.Any(u => u.Email == UserInput.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                }
                else
                {
                    Vendor NewVendor = new Vendor(UserInput);
                    _context.users.Add(NewVendor);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("userid", NewVendor.UserId);
                    return Redirect("vendor-registration2");
                }
            }
            System.Console.WriteLine("Not valid");
            return View("VendorRegistration", UserInput);
        }

        [HttpGet("vendor-registration2")]
        public IActionResult VendorRegistration2()
        {

            return View();
        }
        [HttpPost("vendor-registration")]
        public IActionResult AddToVendor(Vendor UserInput)
        {
            int? ID = HttpContext.Session.GetInt32("userid");           
            Vendor user = _context.users
                .OfType<Vendor>()
                .Where(vendor => vendor.UserId == ID)
                .FirstOrDefault();
            
            if (user == null)
            {
                return Redirect("/");
            }

            user.AreaOfHouse = UserInput.AreaOfHouse;
            user.BusinessCategory = UserInput.BusinessCategory;
            _context.SaveChanges();
            return Redirect("dashboard");
        }

        
        [HttpPost("vendor")]
        public IActionResult NewVendor(Vendor NewVendor)
        {
            if(ModelState.IsValid)  
            {   
                int UserId = (int)HttpContext.Session.GetInt32("userid");
                User User =_context.users.SingleOrDefault(user => user.UserId == UserId);
                
                HttpContext.Session.SetInt32("UserId", UserId);
                @ViewBag.User = User;

                NewVendor.UserId = UserId;
                _context.users.Add(NewVendor);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Vendor");
            }
        }
        
    }
}
