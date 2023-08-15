namespace Ecommerce.Application.Features.Reviews.Queries.Vms;

public class ReviewVm
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public int ProductId { get; set; }
    public string? Nombre { get; set; }
    public string? Comentario { get; set; }
}
