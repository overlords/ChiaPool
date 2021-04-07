using ChiaPool.Models;
using System.Threading.Tasks;

namespace ChiaPool.Services.Abstraction
{
    public interface IPlotOfferHandler
    {
        ValueTask HandlePlotOfferAsync(RemotePlot plot, long plotterId);
    }
}
