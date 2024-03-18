using System;
using System.Collections.Generic;

namespace Follow_Up_Manager.Models;

public partial class FollowUp
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string? Name { get; set; }

    public string? Project { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? FollowUpDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Task { get; set; } = null!;

    public long? Status { get; set; }
}
