public class SeedService {
    private WikiContext _context;

    public SeedService(WikiContext wikiContext) {
        _context = wikiContext;
    }

    public async Task Seed() {
        await _context.Database.EnsureDeletedAsync();
        await  _context.Database.EnsureCreatedAsync();

        var articleService = new ArticleService(_context);

        var articles = new List<ArticleDTO> {
            new ArticleDTO {
                Title = "Lorem Ipsum",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            },
            new ArticleDTO {
                Title = "Bee Movie",
                Content = "According to all known laws of aviation, there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the ground. The bee, of course, flies anyway because bees don't care what humans think is impossible."
            },
            new ArticleDTO {
                Title = "About Copypastas",
                Content = "A copypasta is a block of text that is copied and pasted across the Internet by individuals through online forums and social networking websites."
            },
            new ArticleDTO {
                Title = "HackMD Stuff",
                Content = @"
Lorem ipsum dolor sit amet, *adipiscing elit*.
## Second Header
wow some more information, see the lists below.
- Item 1
  - Sub-item 1
  - Sub-item 2
- Item 2
  - Sub-item 1
                "}
        };

        foreach(var article in articles) {
            await articleService.CreateArticleAsync(article);
        }

        await _context.SaveChangesAsync();
    }
}