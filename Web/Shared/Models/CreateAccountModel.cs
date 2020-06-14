using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Shared.Models
{
    public class CreateAccountModel : LoginModel
    {
        public string Email { get; set; }
    }
}
