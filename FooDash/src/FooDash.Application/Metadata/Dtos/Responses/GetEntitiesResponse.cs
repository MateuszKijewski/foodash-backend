namespace FooDash.Application.Metadata.Dtos.Responses
{
    public class GetEntitiesResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsTranslated { get; set; }
    }
}