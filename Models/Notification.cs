using System;
using System.Collections.Generic;

namespace Follow_Up_Manager.Models;

public partial class Notification
{
    public long Id { get; set; }

    public string ClientId { get; set; } = null!;

    public string UserIdentifier { get; set; } = null!;

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public string? Payload { get; set; }

    public long? Status { get; set; }
}
