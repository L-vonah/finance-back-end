namespace Infrastructure.Data
{
    public abstract class SoftableDeleted
    {
        public DateTime? DeletedAt { get; set; }
    }

    public abstract class BaseEntity : SoftableDeleted
    {
        public int Id { get; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
