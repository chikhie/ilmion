namespace MuslimAds.Infra.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AdEntity
{
    [Key]
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public UserEntity User { get; set; } = new UserEntity();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;
    public double Budget { get; set; } = 0;

}