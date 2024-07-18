namespace WomensWiki.Domain;

public class Revision : Entity {
    public Article Article { get; private set; }
    public string Content { get; private set; }

    public Revision() { }
    
    public Revision(Article article, string content) {
        Article = article;
        Content = content;
    }
}