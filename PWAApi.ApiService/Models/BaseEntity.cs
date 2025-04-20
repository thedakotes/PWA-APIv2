using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity 
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Primary key for the entity, typically used for database operations
    public DateTime CreatedAt { get; set; } // Timestamp for when the entity was created, useful for tracking creation time
    public DateTime UpdatedAt { get; set; } // Timestamp for when the entity was last updated, useful for tracking modifications
}