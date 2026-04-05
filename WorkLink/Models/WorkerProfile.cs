using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkLink.Models
{
    public class WorkerProfile
    {
        [Key]
        public int WorkerProfileId { get; set; }

        // 🔑 FK to Identity user

        [ValidateNever]
        public string UserId { get; set; } = null!;


        [ForeignKey("UserId")]
        [ValidateNever] // ✅ ignore during form validation
        public ApplicationUser? User { get; set; }

        // 🔹 PERSONAL INFO
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string City { get; set; } = null!;   // ✅ ADDED CITY

        // 🔹 Professional Info
        [Required, MaxLength(100)]
        public string Profession { get; set; } = null!;

        [MaxLength(200)]
        public string? Experience { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        // 🔹 Pricing
        public decimal? HourlyRate { get; set; }
        public decimal? RatePerKm { get; set; }


        // 🔹 Location
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // 🔹 Status
        public bool IsVerified { get; set; } = false;

        [Range(0, 5)]
        public double Rating { get; set; } = 0;
        // 🔹 PROFILE IMAGE
        // Stores the path for display: "/uploads/xyz.png"
        [MaxLength(500)]
        public string? ProfileImageUrl { get; set; }

        // ⚡ Transient property for file upload
        [NotMapped]
        public IFormFile? ProfileImageFile { get; set; }


        public ICollection<Job> Jobs { get; set; } = new List<Job>();
        public ICollection<WorkerAvailability> Availabilities { get; set; } = new List<WorkerAvailability>();

        // ✅ THIS IS WHAT YOUR ERROR IS ABOUT
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
