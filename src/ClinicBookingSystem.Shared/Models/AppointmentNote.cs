namespace ClinicBookingSystem.Shared.Models;

public class AppointmentNote
{
    [Key]
    public int NoteId { get; set; }

    [Required]
    [ForeignKey("Appointment")]
    public int AppointmentId { get; set; }

    [Required]
    [StringLength(2000)]
    public string Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Appointment Appointment { get; set; }
}
