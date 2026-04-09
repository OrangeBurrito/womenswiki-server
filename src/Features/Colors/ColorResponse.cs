using WomensWiki.Domain.Colors;

namespace WomensWiki.Features.Colors;

public record ColorResponse(string Name, string Value) {
    public static ColorResponse FromColor(Color color) {
        return new ColorResponse(color.Name, color.Value);
    }
}