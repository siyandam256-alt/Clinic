namespace ClinicBookingSystem.Shared.Models;

public class Clinic
{
    [Key]
    public int ClinicId { get; set; }

    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Address { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(100)]
    public string State { get; set; }

    [Required]
    [StringLength(20)]
    public string ZipCode { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public TimeSpan WorkingHoursStart { get; set; }

    [Required]
    public TimeSpan WorkingHoursEnd { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Provider> Providers { get; set; } = new List<Provider>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
