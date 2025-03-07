using ArtShop.Models;
using System;
using System.Collections.Generic;

namespace ArtShopAPI.Models;

public partial class UserDTO
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public double Balance { get; set; }

    public virtual ICollection<Art> Arts { get; set; } = new List<Art>();
}
