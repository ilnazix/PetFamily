namespace PetFamily.Core.Dtos
{
    public class SocialMediaInfo
    {
        public SocialMediaInfo()
        {
        }

        public SocialMediaInfo(string link, string title)
        {
            Link = link;
            Title = title;
        }

        public string Link { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
