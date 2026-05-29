namespace ClinicBookingSystem.Shared.Models;

public enum AppointmentStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3,
    NoShow = 4
}

public class Appointment
{
    [Key]
    public int AppointmentId { get; set; }

    [Required]
    [ForeignKey("Patient")]
    public int PatientId { get; set; }

    [Required]
    [ForeignKey("Clinic")]
    public int ClinicId { get; set; }

    [Required]
    [ForeignKey("Provider")]
    public int ProviderId { get; set; }

    [Required]
    [ForeignKey("TimeSlot")]
    public int TimeSlotId { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [Required]
    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Patient Patient { get; set; }
    public Clinic Clinic { get; set; }
    public Provider Provider { get; set; }
    public TimeSlot TimeSlot { get; set; }
    public ICollection<AppointmentNote> Notes { get; set; } = new List<AppointmentNote>();
}
