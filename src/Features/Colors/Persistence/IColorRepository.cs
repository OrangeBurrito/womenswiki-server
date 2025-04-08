using WomensWiki.Common.Interfaces;
using WomensWiki.Domain.Colors;

namespace WomensWiki.Features.Colors.Persistence;

public interface IColorRepository: IRepository {
    Task<Color?> GetColor(string name);
    Task<Color?> CreateColor(string name, string value);
}