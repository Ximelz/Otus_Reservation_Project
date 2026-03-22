namespace Hotels.Domain.Entities;

public class HotelPolicy
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid HotelId { get; set; } = Guid.Empty;
    public TimeOnly CheckInTime { get; set; } = new(14, 0);
    public TimeOnly CheckOutTime { get; set; } = new(12, 0);
    public string? EarlyCheckInNote { get; set; }
    public string? LateCheckOutNote { get; set; }
    public string? CancellationPolicyText { get; set; }
    public bool ConfirmationPendingEnabled { get; set; }
    public bool AutoConfirmRules { get; set; }
    public string? TermsAndConditionsText { get; set; }
    public string? ContactInstructions { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Hotel? Hotel { get; set; }
}
