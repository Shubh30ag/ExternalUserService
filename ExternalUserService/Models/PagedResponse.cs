namespace ExternalUserService.Models
{
    public class PagedResponse
    {
        public int page { get; set; }
        public int total_pages { get; set; }
        public IEnumerable<User> data { get; set; }
    }
}
