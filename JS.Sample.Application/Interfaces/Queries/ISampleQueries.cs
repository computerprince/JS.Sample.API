using JS.Sample.Application.Models;
using JS.Sample.Common.Models;
using System.Threading.Tasks;

namespace JS.Sample.Application.Interfaces.Queries
{
   public  interface ISampleQueries
    {
        /// <summary>
        ///  Get Details of Product
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<CommonResponse> GetProductDetail(long Id);
        /// <summary>
        /// Get List of Products
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<CommonResponse> GetList(ListFilterRequest request);
    }
}
