namespace Infrustructure.DTOs.Barbers;

public class BarberWithReviewsDTO : BarberDTO
{
    public List<ReviewDTO>? Reviews { get; set; }
}