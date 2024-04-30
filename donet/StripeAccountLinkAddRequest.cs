using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class StripeAccountLinkAddRequest
    {
        [Required]
        public string Account { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string RefreshUrl { get; set; }
        [Required]
        public string ReturnUrl { get; set; }


    }
}
