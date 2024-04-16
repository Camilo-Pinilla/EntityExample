using System.ComponentModel.DataAnnotations;

namespace EntityExample.Models
{
	public class Contacts
	{
        public int Id { get; set; }

        [StringLength(56, ErrorMessage = "The field accept only a max of 56 characters long ")]
        public  string Address { get; set; }

		[StringLength(56, ErrorMessage = "The field accept only a max of 56 characters long ")]
		public string City { get; set; }

        [EmailAddress]
        public  string EmailAddress { get; set; }
		[StringLength(56, ErrorMessage = "The field accept only a max of 56 characters long ")]
		public string Name { get; set; }

        public Status Status { get; set; }
	}


    public enum Status
    {
        Approved,
        Submitted,
        Rejected
    }
}
