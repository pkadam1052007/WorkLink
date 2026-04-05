using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkLink.Models;

namespace WorkLink.Models
{
    public class WorkerVerification
    {
        [Key]
        public int VerificationId { get; set; }

        public int WorkerProfileId { get; set; }

        [ForeignKey("WorkerProfileId")]
        public WorkerProfile Worker { get; set; } = null!;

        public DateTime VerifiedOn { get; set; }

        public string VerifiedByAdminId { get; set; } = null!;
    }
}
