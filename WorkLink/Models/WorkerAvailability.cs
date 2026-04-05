using System;
using System.ComponentModel.DataAnnotations;

namespace WorkLink.Models
{
    public class WorkerAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }

        public int WorkerProfileId { get; set; }

        public DateTime AvailableDate { get; set; }

        public bool IsAvailable { get; set; } = true;

        // Navigation
        public WorkerProfile WorkerProfile { get; set; } = null!;
    }
}
