using System;
using System.Collections.Generic;

namespace Follow_Up_Manager.Models;

public partial class ActivityLog
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long? FollowUpId { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
