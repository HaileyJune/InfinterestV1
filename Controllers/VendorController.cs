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
                .Include(vend => vend.Events)
                .FirstOrDefault();
            
            if (user == null)
            {
                return Redirect("/");
            }

            DashboardVendorView viewModel = new DashboardVendorView();
            
            viewModel.currentUser = user;

            viewModel.allEvents = _context.events
                                    .Include(eve => eve.Listing)
                                    .ThenInclude(lis => lis.Address)
                                    .Include (eve => eve.Broker)
                                    .Where (eve => eve.OpenHouseDateTime > DateTime.Now)
                                    .Where (eve => eve.Confirmed == false)
                                    .ToList();

            viewModel.usersEvents = user.Events
                                    .Select(ve => ve.Event)
                                    .ToList();

            // viewModel.usersEvents = _context.users
            //                         .OfType<Vendor>()
            //                         .Where (use => use.UserId == user.UserId)
            //                         .Select(use => use.Events)
            //                         .ToList();

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
                    return View("VendorRegistration");
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
        
        [HttpPost("vendor-registration2")]
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

        
        [HttpGet("event-detail/{eventId}/request")]
        public IActionResult EventRequest(string eventId)
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

            if(Int32.TryParse(eventId, out int id))
            {
                Event thisEvent = _context.events
                    .FirstOrDefault(ev => ev.EventId == id);

                if (thisEvent == null)
                {
                    return Redirect("/no-event");
                }
                
                VendorToEvent thisRequest = new VendorToEvent(user, thisEvent);

                _context.eventvendors.Add(thisRequest);
                thisEvent.EventVendors.Add(thisRequest);
                user.Events.Add(thisRequest);
                _context.SaveChanges();
            }
            return Redirect("/event-detail/" + eventId);
        }
        // [HttpPost("vendor")]
        // public IActionResult NewVendor(Vendor NewVendor)
        // {
        //     if(ModelState.IsValid)  
        //     {   
        //         int UserId = (int)HttpContext.Session.GetInt32("userid");
        //         User User =_context.users.SingleOrDefault(user => user.UserId == UserId);
                
        //         HttpContext.Session.SetInt32("UserId", UserId);
        //         @ViewBag.User = User;

        //         NewVendor.UserId = UserId;
        //         _context.users.Add(NewVendor);
        //         _context.SaveChanges();
        //         return RedirectToAction("Dashboard");
        //     }
        //     else
        //     {
        //         return View("Vendor");
        //     }
        // }
        
    }
}
