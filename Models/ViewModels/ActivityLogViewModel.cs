namespace Follow_Up_Manager.Models.ViewModels;

public class ActivityLogViewModel
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long? FollowUpId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}