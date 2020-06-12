using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Web.Shared.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        // @Derek any other object properties you need
    }

}
