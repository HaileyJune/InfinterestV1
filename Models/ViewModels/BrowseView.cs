using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infinterest.Models
{
    public class BrowseView
    {
        public List<Event> UpcomingEvents {get;set;}
        public List<Event> PastEvents {get;set;}
    }
}