using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerWebAPI.Models
{
	public class Customer
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is required")]
		[StringLength(100, ErrorMessage = "Maximum Name length is 100 characters")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Email Id is required")]
		[EmailAddress(ErrorMessage = "Invalid Email address")]
		public string Email	{ get; set; }
		[Required(ErrorMessage = "Phone number is required")]
		[Phone(ErrorMessage = "Invalid phone number")]
		public string PhoneNumber { get; set; }
	}
}