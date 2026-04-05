using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkLink.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        public int WorkerProfileId { get; set; }

        public DateTime JobDate { get; set; }

        public int Hours { get; set; }

        public double DistanceKm { get; set; }

        // ✅ ADD THESE TWO LINES ONLY
        public double EmployerLatitude { get; set; }
        public double EmployerLongitude { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        public JobStatus Status { get; set; } = JobStatus.Requested;

        public DateTime? CompletedAt { get; set; }

        // Navigation
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public WorkerProfile WorkerProfile { get; set; } = null!;
    }
}
