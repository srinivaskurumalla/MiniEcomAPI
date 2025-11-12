using System;
using System.Collections.Generic;

namespace MiniEcom.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[]? PasswordHash { get; set; }

    public byte[]? PasswordSalt { get; set; }

    public bool IsEmailConfirmed { get; set; }

    public string? Phone { get; set; }

    public string? DisplayName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
