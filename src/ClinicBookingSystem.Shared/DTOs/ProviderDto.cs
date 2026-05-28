namespace ClinicBookingSystem.Shared.DTOs;

public class ProviderDto
{
    public int ProviderId { get; set; }
    public int ClinicId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string LicenseNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}

public class CreateProviderDto
{
    [Required]
    public int ClinicId { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [StringLength(200)]
    public string Specialization { get; set; }

    [Required]
    [StringLength(100)]
    public string LicenseNumber { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}
