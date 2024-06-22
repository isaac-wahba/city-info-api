namespace CityInfo.API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // one to many relationship
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

        // calculated field.
        public int NumberOfPointsOfInterest 
        { 
            get 
            { 
                return PointsOfInterest.Count;
            } 
        }
    }
}
