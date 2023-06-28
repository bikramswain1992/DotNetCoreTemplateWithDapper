namespace TemplateDapper.Domain.Entities;

public class AuditableEntity : BaseEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
}
