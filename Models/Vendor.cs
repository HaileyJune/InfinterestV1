using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infinterest.Models
{
public class Vendor : UserProfile
    {
        public List<Area> AreaOfHouse {get; set;}
        public List<Business> BusinessCategory {get; set;}
        public virtual List<VendorToEvent> Events {get; set;}

    public Vendor()
        {

        }

        public Vendor(UserProfile input)
        {
            UserType = "Vendor";
            FirstName = input.FirstName;
            LastName = input.LastName;
            Email = input.Email;
            PasswordHasher<Vendor> Hasher = new PasswordHasher<Vendor>();
            Password = Hasher.HashPassword(this, input.Password);
            ImgUrl = input.ImgUrl;
            Contact = input.Contact;
            Bio = input.Bio;
            Company = input.Company;
            Website = input.Website;

            //area of house and business catagory
            AreaOfHouse = new List<Area>();
            BusinessCategory = new List<Business>();
            
            Events = new List<VendorToEvent>();
        }
    }
}