public class SeedService {
    private WikiContext _context;

    public SeedService(WikiContext wikiContext) {
        _context = wikiContext;
    }

    public async Task Seed() {
        await _context.Database.EnsureDeletedAsync();
        await  _context.Database.EnsureCreatedAsync();

        var articleService = new ArticleService(_context);

        var articles = new List<Article> {
            new Article {
                Title = "First Article",
                Revisions = new List<Revision> { new Revision { Content = "# To bee or not to bee" } }
            },
            new Article {
                Title = "Second Article",
                Revisions = new List<Revision> { new Revision { Content = "# The Second Article" } }
            },
            new Article {
                Title = "Third Article",
                Revisions = new List<Revision> { new Revision { Content = "# How to chew grass like a real cow" } }
            },
            new Article {
                Title = "HackMD Stuff",
                Revisions = new List<Revision> { new Revision { Content = @"
Lorem ipsum dolor sit amet, *adipiscing elit*.
## Second Header
wow some more information, see the lists below.
- Item 1
  - Sub-item 1
  - Sub-item 2
- Item 2
  - Sub-item 1
                "}
                }}
        };

        foreach(var article in articles) {
            await articleService.CreateArticleAsync(article);
        }

        await _context.SaveChangesAsync();
    }
}