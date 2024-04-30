using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class PaymentAccountAddRequest
    {
        [Required]
        public int VenueId { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public int PaymentTypeId { get; set; }
    }
}
