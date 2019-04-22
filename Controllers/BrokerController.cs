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
    public class BrokerController : Controller
    {
        private Context _context;
        public BrokerController(Context context)
        {
            _context = context;
        }

        [HttpGet("broker-registration")]
        public IActionResult BrokerRegistration()
        {
            return View("BrokerRegistration");
        }

        [HttpPost("broker-registration")]
        public IActionResult DoesBrokerRegistration(User UserInput)
        {
            if(ModelState.IsValid)
            {
                if(_context.users.Any(u => u.Email == UserInput.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("BrokerRegistration");
                }
                else
                {
                    Broker NewBroker = new Broker(UserInput);
                    // only need one?
                    // _context.brokers.Add(NewBroker);
                    
                    _context.users.Add(NewBroker);
                    
                    _context.SaveChanges();
                    HttpContext.Session.SetString("name", NewBroker.FirstName);
                    HttpContext.Session.SetInt32("userid", NewBroker.UserId);

                    return RedirectToAction("DashboardBroker");
                }
            }
            else
            {
                return View("BrokerRegistration");
            }
        }

        

        [HttpGet("brokerdashboard")]
        public IActionResult DashboardBroker()
        {
            
            
            int? ID = HttpContext.Session.GetInt32("userid");           
            Broker user = _context.users
                .OfType<Broker>()
                .Where(broker => broker.UserId == ID)
                .FirstOrDefault();

            if (user == null)
            {
                return Redirect("/");
            }

            DashboardBrokerView DisplayModel = new DashboardBrokerView();
            DisplayModel.CurrentUser = user;

            DisplayModel.UsersListings = _context.listings
                .Where(lis => lis.BrokerId == user.UserId)
                .ToList();

            DisplayModel.PendingEvents = _context.events
                .Where(eve => eve.BrokerId == user.UserId)
                .Where(thisEvent => thisEvent.Confirmed == false)
                .ToList();

            DisplayModel.FinalizedEvents = _context.events
                .Where(eve => eve.BrokerId == user.UserId)
                .Where(thisEvent => thisEvent.Confirmed == true)
                .ToList();

            DisplayModel.AvailableVendors = _context.users
            .OfType<Vendor>()
            .ToList();

            // probably needs to account for being in a different controlelr
            return View("DashboardBroker", DisplayModel);
        
        }

        //temporary to show add event page
        [HttpGet("add-event")]
        public IActionResult AddEventTemp(String listingId)
        {
            return View ("AddEvents");
        }

        // [HttpGet("add-event/{ListingId}")]
        // public IActionResult AddEvent(String listingId)
        // {
        //     return View ("AddEvents");
        // }

        [HttpPost("add-event/{ListingId}")]
        public IActionResult CreateEvent(Event NewEvent, String ListingId)
        {
            if (ModelState.IsValid)
            {
                if(Int32.TryParse(ListingId, out int id))
                {
                    NewEvent.ListingId = id;

                    NewEvent.Listing = _context.listings
                    .FirstOrDefault(l => l.ListingId == id);
                    
                    int? ID = HttpContext.Session.GetInt32("userid");           
                    Broker user = _context.users
                        .OfType<Broker>()
                        .Where(broker => broker.UserId == ID)
                        .FirstOrDefault();

                    NewEvent.Broker = user;
                    NewEvent.BrokerId = user.UserId;


                    _context.events.Add(NewEvent);
                    return RedirectToAction ("Dashboard");                
                }
            }
            return View ("AddEvents");
        }

        //temporary to show add listing page
        [HttpGet("add-listings")]
        public IActionResult AddListingTemp()
        {
            return View ("AddListing");
        }

        [HttpPost("add-listings")]
        public IActionResult CreateListing(Listing NewListing)
        {
            if (ModelState.IsValid)
            {
                int? ID = HttpContext.Session.GetInt32("userid");           

                Broker user = _context.users
                    .OfType<Broker>()
                    .Where(broker => broker.UserId == ID)
                    .FirstOrDefault();
                
                //Add listing to db
                NewListing.Broker = user;
                NewListing.BrokerId = user.UserId;
                NewListing.Events = new List<Event>();
                _context.listings.Add(NewListing);

                // add to broker's listings
                user.Listings.Add(NewListing);
                _context.SaveChanges();

                return Redirect ("/dashboard");
            }
            else
            {
                return View ("AddListing");
            }
        }
        
    }
}
