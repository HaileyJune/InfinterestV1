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
    public class HomeController : Controller
    {
        private Context _context;
        public HomeController(Context context)
        {
            _context = context;
        }
        // Signin and login page
        public IActionResult Index()
        {
            ViewBag.Error = TempData["Error"];
            ViewBag.ErrorReg = TempData["ErrorReg"];
            return View("Index");
        }
        
        [HttpGet("broker-profile/{id}")]
        public IActionResult BrokerProfile(String id)
        {
            ProfileDetailView ViewModel = new ProfileDetailView();
            int? ID = HttpContext.Session.GetInt32("userid");           
            User user = _context.users
                .Where(use => use.UserId == ID)
                .FirstOrDefault();
            if (user == null)
            {
                return Redirect("/fail");
            }
            ViewModel.CurrentUser = user;
            if(Int32.TryParse(id, out int userid))
            {
                Broker thisBroker = _context.users
                .OfType<Broker>()                
                    .Where(broker => broker.UserId == userid)
                    .Include(broker => broker.Listings) //redundent until I re-work the view
                    .ThenInclude(list => list.Address)
                    .Include(broker => broker.Events)
                        .ThenInclude(eve => eve.Listing)
                        .ThenInclude(list => list.Address)
                    .Include(broker => broker.Events)
                        .ThenInclude(eve => eve.EventVendors)
                        .ThenInclude(ev => ev.Vendor)
                    .FirstOrDefault();
                if (thisBroker == null)
                {
                    return Redirect("/");
                }
                ViewModel.SelectedUser = thisBroker;
                ViewModel.UpcomingEvents = thisBroker.Events
                                        .FindAll(eve => eve.Confirmed.Equals(true) && eve.OpenHouseDateTime > DateTime.Now)
                                        .ToList();
                ViewModel.PastEvents = thisBroker.Events
                                        .FindAll(eve => eve.OpenHouseDateTime < DateTime.Now)
                                        .ToList();
                return View("BrokerProfile", ViewModel);
            }
            return RedirectToAction ("Dashboard");
        }

        [HttpGet("vendor-profile/{id}")]
        public IActionResult VendorProfile(String id)
        {
            ProfileDetailView ViewModel = new ProfileDetailView();
            int? ID = HttpContext.Session.GetInt32("userid");           
            User user = _context.users
                .Where(use => use.UserId == ID)
                .FirstOrDefault();
            if (user == null)
            {
                return Redirect("/fail");
            }
            ViewModel.CurrentUser = user;
            if(Int32.TryParse(id, out int userid))
            {
                Vendor thisVendor = _context.users
                .OfType<Vendor>()
                    .Include(ve => ve.Events)
                        .ThenInclude(ve => ve.Event)
                    .Where(vendor => vendor.UserId == userid)
                    .FirstOrDefault();
                if (thisVendor == null)
                {
                    return Redirect("/fail");
                }
                ViewModel.SelectedUser = thisVendor;
                List<Event> ConfrimedEvents = thisVendor.Events
                                            .FindAll(eve => eve.RequestStatus == "Accepted")
                                            .Select(eve => eve.Event)
                                            .ToList();
                ViewModel.UpcomingEvents = ConfrimedEvents
                                        .FindAll(eve => eve.OpenHouseDateTime > DateTime.Now)
                                        .ToList();
                ViewModel.PastEvents = ConfrimedEvents
                                        .FindAll(eve => eve.OpenHouseDateTime < DateTime.Now)
                                        .ToList();
                return View("VendorProfile", ViewModel);
            }
            
            return RedirectToAction ("Dashboard");
        }

        [HttpGet("event-detail/{eventId}")]
        public IActionResult EventDetail(String eventId)
        {
            int? ID = HttpContext.Session.GetInt32("userid");           
            User user = _context.users
                .Where(use => use.UserId == ID)
                .FirstOrDefault();

            if (user == null)
            {
                return Redirect("/fail");
            }
            
            if(Int32.TryParse(eventId, out int id))
            {
                Event thisEvent = _context.events
                    .Include(ev => ev.Broker)
                    .Include(ev => ev.AreaOfHouse)
                    .Include(ev => ev.Listing)
                        .ThenInclude(li => li.Address)
                    .Include(ev => ev.EventVendors)
                        .ThenInclude(ev => ev.Vendor)  
                        .ThenInclude(bus => bus.BusinessCategory)
                    .FirstOrDefault(ev => ev.EventId == id);

                    EventDetailView ViewModel = new EventDetailView(user, thisEvent);

                    if(user.UserType == "Vendor")
                    {
                        if(thisEvent.EventVendors.Find(ev => ev.Vendor.UserId == user.UserId) == null)
                        {
                            ViewModel.ShowJoinButton = true;
                        }
                    }

                return View(ViewModel);
            }
            return RedirectToAction ("Dashboard");
        }

        [HttpGet("listing-details/{listingId}")]
        public IActionResult ListingDetail(String listingId)
        {
            int? ID = HttpContext.Session.GetInt32("userid");           
            User user = _context.users
                .Where(use => use.UserId == ID)
                .FirstOrDefault();

            if (user == null)
            {
                return Redirect("/fail");
            }

            if(Int32.TryParse(listingId, out int id))
            {
                Listing thisListing = _context.listings
                    .Include(list => list.Broker)
                    .Include(list => list.Address)
                    .Include(list => list.Events)
                    .ThenInclude(eve => eve.EventVendors)
                    .ThenInclude(ev => ev.Vendor)
                    .FirstOrDefault(lis => lis.ListingId == id);

                ListingDetailView ViewModel = new ListingDetailView(user, thisListing);
                return View(ViewModel);
            }
            return RedirectToAction ("Dashboard");
        }

        // POST Login user
        [HttpPost("existing-user")]
        public IActionResult ExistingUser(User userInput)
        {   
            if(userInput.Email != null && userInput.Password != null)
            {   
                User ThisUser =_context.users.SingleOrDefault(user => user.Email == userInput.Email);
                if(ThisUser != null)
                {
                    var Hasher = new PasswordHasher<User>();
                
                    if(0 != Hasher.VerifyHashedPassword(ThisUser, ThisUser.Password, userInput.Password))
                    {
                        HttpContext.Session.SetString("Login", "True");
                        HttpContext.Session.SetInt32("userid", ThisUser.UserId);
                        return Redirect("/dashboard");
                    }
                }
                TempData["Error"] = "Your email or password are not correct";      
            }
            else 
            {   
                TempData["Error"] = "An email and password are required";
            }
            //temp while sorting out error return
            return Redirect("/fail");
            // return RedirectToAction("Index");
        }
        //GET Logoff
        [HttpGet("logoff")]
        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }  

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? ID = HttpContext.Session.GetInt32("userid");

            User CurrentUser = _context.users
            .Where(user => user.UserId == ID)
            .FirstOrDefault();

            if(CurrentUser != null)
            {
                if(CurrentUser.UserType == "Vendor")
                {
                    return RedirectToAction("DashboardVendor", "Vendor");
                }
                else if(CurrentUser.UserType == "Broker")
                {
                    return RedirectToAction("DashboardBroker", "Broker");
                }
            }
                
                return Redirect("/");
        }
        
        // [HttpGet("messaging")]
        // public IActionResult Messaging()
        // {
        //     return View();
        // }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return View();
        }
        [HttpGet("profile-changes")]
        public IActionResult ProfileChanges()
        {
            return View();
        }

        [HttpGet("search")]
        public IActionResult AfterSearch()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("browse")]
        public IActionResult Browse()
        {
            BrowseView viewModel = new BrowseView();

            viewModel.UpcomingEvents = _context.events
                                    .Include(eve => eve.Listing)
                                        .ThenInclude(lis => lis.Address)
                                    .Include (eve => eve.Broker)
                                    .Include (eve => eve.EventVendors)
                                        .ThenInclude(ev => ev.Vendor)
                                    .Include (eve => eve.AreaOfHouse)
                                    .Where (eve => eve.OpenHouseDateTime > DateTime.Now)
                                    .Where (eve => eve.Confirmed == true)
                                    .ToList();
            viewModel.PastEvents = _context.events
                                    .Include(eve => eve.Listing)
                                        .ThenInclude(lis => lis.Address)
                                    .Include (eve => eve.Broker)
                                    .Include (eve => eve.EventVendors)
                                        .ThenInclude(ev => ev.Vendor)
                                    .Include (eve => eve.AreaOfHouse)
                                    .Where (eve => eve.OpenHouseDateTime < DateTime.Now)
                                    .Where (eve => eve.Confirmed == true)
                                    .ToList();
            return View(viewModel);
        }   
        
        [HttpGet("fail")]
        public IActionResult Fail()
        {
            return View("Fail");
        }
    }
}
