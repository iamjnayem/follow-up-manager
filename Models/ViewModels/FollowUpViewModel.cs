namespace Follow_Up_Manager.Models.ViewModels;

public class FollowUpViewModel
{
    public long? Id { get; set; }

    public long? UserId { get; set; }

    public string? Name { get; set; }

    public string? Project { get; set; }
    public string? Task { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FollowUpDate { get; set; }
}
