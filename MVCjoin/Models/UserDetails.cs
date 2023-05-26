using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MVCjoin.Models
{
    public class UserDetails
    {
       
        public int UserId { get; set; }      
        public string UserName { get; set; }        
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}