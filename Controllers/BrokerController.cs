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
                    
                    _context.users.Add(NewBroker);
                    
                    _context.SaveChanges();
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
                .Include(List => List.Address)
                .Include(lis => lis.Events)
                .ThenInclude(eve => eve.EventVendors)
                // .ThenInclude(ev => ev.Vendor)
                // for posting venor img or name, for now we're just counting
                .ToList();

            DisplayModel.AvailableVendors = _context.users
            .OfType<Vendor>()
                .Include(ven => ven.BusinessCategory)
                .Include(ven => ven.AreaOfHouse)
            .ToList();

            return View("DashboardBroker", DisplayModel);
        
        }

        //Events and Listings
        [HttpGet("add-event/{ListingId}")]
        public IActionResult AddEventTemp(String ListingId)
        {
            ViewBag.listingId = ListingId;
                return View ("AddEvents");

        }

        [HttpPost("add-event/{ListingId}")]
        public IActionResult CreateEvent(EventForm Input, String ListingId)
        {
            if (ModelState.IsValid)
            {
                if(Int32.TryParse(ListingId, out int id))
                {
                    Listing thisListing = _context.listings
                    .FirstOrDefault(l => l.ListingId == id);
                    
                    int? ID = HttpContext.Session.GetInt32("userid");           
                    Broker user = _context.users
                        .OfType<Broker>()
                        .Where(broker => broker.UserId == ID)
                        .FirstOrDefault();

                    if(user == null)
                    {
                        return Redirect("/");
                    }
                    if(thisListing.BrokerId != user.UserId)
                    {
                        return Redirect("/fail");
                    }

                    Event NewEvent = new Event(Input, user, thisListing);


                    _context.events.Add(NewEvent);
                    user.Events.Add(NewEvent);
                    thisListing.Events.Add(NewEvent);

                    _context.SaveChanges();
                    return Redirect ("/listing-details/" + id);                
                }
            }
            return View ("AddEvents");
        }

        [HttpGet("add-listings")]
        public IActionResult AddListingTemp()
        {
            return View ("AddListing");
        }

        [HttpPost("add-listings")]
        public IActionResult CreateListing(ListingForm UserInput)
        {
            if (ModelState.IsValid)
            {
                int? ID = HttpContext.Session.GetInt32("userid");           
                Broker user = _context.users
                    .OfType<Broker>()
                    .Where(use => use.UserId == ID)
                    .FirstOrDefault();
                if(user == null)
                {
                    //fake code
                    return Redirect("/fail");
                }

                //Add address to db
                Address NewAddress = new Address(UserInput);
                _context.address.Add(NewAddress);
                _context.SaveChanges();

                //Add listing to db
                Listing NewListing = new Listing(UserInput, NewAddress);
                NewListing.Broker = user;
                NewListing.BrokerId = user.UserId;
                _context.listings.Add(NewListing);

                // add to broker's listings
                user.Listings.Add(NewListing);
                _context.SaveChanges();

                return Redirect ("/listing-details/" + NewListing.ListingId);
            }
            else
            {
                return View ("AddListing");
            }
        }
        
        [HttpGet("remove/listing/{ListingId}")]
        public IActionResult RemoveListing (string ListingId)
        {
            if(Int32.TryParse(ListingId, out int id))
            {
                Listing listingToDelete = _context.listings
                    .FirstOrDefault(listing => listing.ListingId == id);

                if(listingToDelete.BrokerId == HttpContext.Session.GetInt32("userid"))
                {
                    if(listingToDelete.Availible == false)
                    {
                        _context.listings.Remove(listingToDelete);
                        _context.SaveChanges();
                    }
                }
            }
            return Redirect("/dashboard");
        }
        [HttpGet("listing/{ListingId}/archive")]
        public IActionResult ArchiveListing (string ListingId)
        {
            if(Int32.TryParse(ListingId, out int id))
            {
                Listing listingToDelete = _context.listings
                    .FirstOrDefault(listing => listing.ListingId == id);

                if(listingToDelete.BrokerId == HttpContext.Session.GetInt32("userid"))
                {
                    listingToDelete.Availible = false;
                    _context.SaveChanges();
                }
            }
            return Redirect("/dashboard");
        }
        [HttpGet("remove/event/{EventId}")]
        public IActionResult RemoveEvent (string EventId)
        {
            if(Int32.TryParse(EventId, out int id))
            {
                Event eventToDelete = _context.events
                    .FirstOrDefault(eve => eve.EventId == id);

                if(eventToDelete.BrokerId == HttpContext.Session.GetInt32("userid"))
                {
                    _context.events.Remove(eventToDelete);
                    _context.SaveChanges();
                }
            }
            return Redirect("/dashboard");
        }
        [HttpGet("event-detail/{EventId}/confirm")]
        public IActionResult ConfirmEvent (string EventId)
        {
            if(Int32.TryParse(EventId, out int id))
            {
                Event eventToConfirm = _context.events
                    .FirstOrDefault(eve => eve.EventId == id);

                if(eventToConfirm.BrokerId == HttpContext.Session.GetInt32("userid"))
                {
                    eventToConfirm.Confirmed = true;
                    _context.SaveChanges();
                }
            }
            return Redirect("/dashboard");
        }
        [HttpGet("event-detail/{EventId}/{VendorId}/confirm")]
        public IActionResult ConfirmVendor (string EventId, string VendorId)
        {
            int? ID = HttpContext.Session.GetInt32("userid");           
            Broker user = _context.users
                .OfType<Broker>()
                .Where(use => use.UserId == ID)
                .FirstOrDefault();
            if(user == null)
            {
                return Redirect("/fail");
            }   

            if(Int32.TryParse(EventId, out int id))
            {
                
                Event eventToConfirm = _context.events
                    .Include(eve => eve.EventVendors)
                    .ThenInclude(ve => ve.Vendor)
                    .FirstOrDefault(eve => eve.EventId == id);

                if(Int32.TryParse(VendorId, out int vid))
                {
                    VendorToEvent Request = eventToConfirm.EventVendors.Find(re => re.Vendor.UserId == vid);

                    if(Request == null)
                    {
                        return Redirect("/fail");
                    }
                    
                    if(Request.Event.Broker == user)
                    {
                        Request.RequestStatus = "Accepted";
                        _context.SaveChanges();
                    }
                    
                }
            }
            return Redirect("/event-detail/" + EventId);
        }
        
        [HttpGet("event-detail/{EventId}/{VendorId}/deny")]
        public IActionResult DenyVendor (string EventId, string VendorId)
        {
            int? ID = HttpContext.Session.GetInt32("userid");           
            Broker user = _context.users
                .OfType<Broker>()
                .Where(use => use.UserId == ID)
                .FirstOrDefault();
            if(user == null)
            {
                return Redirect("/fail");
            }   

            if(Int32.TryParse(EventId, out int id))
            {
                
                Event eventToConfirm = _context.events
                    .Include(eve => eve.EventVendors)
                    .ThenInclude(ve => ve.Vendor)
                    .FirstOrDefault(eve => eve.EventId == id);

                if(Int32.TryParse(VendorId, out int vid))
                {
                    VendorToEvent Request = eventToConfirm.EventVendors.Find(re => re.Vendor.UserId == vid);


                    if(Request == null)
                    {
                        return Redirect("/fail");
                    }
                    
                    if(Request.Event.Broker == user)
                    {
                        Request.RequestStatus = "Denied";
                        _context.SaveChanges();
                    }
                    
                }
            }
            return Redirect("/event-detail/" + EventId);
        }
        
    }
}
