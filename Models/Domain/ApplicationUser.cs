using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Eden_Fn.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FrameLink { get; set; }
    }
}