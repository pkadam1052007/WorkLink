using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkLink.Models;

namespace WorkLink.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        // Navigation
        public WorkerProfile? WorkerProfile { get; set; }
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
