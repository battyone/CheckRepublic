using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Knapcode.CheckRepublic.Logic.Entities;

namespace Knapcode.CheckRepublic.Logic.Business
{
    public interface ICheckService
    {
        Task<IEnumerable<Check>> GetChecksAsync(int skip, int take, bool asc, CancellationToken token);
        Task<Check> GetCheckByIdAsync(int id, CancellationToken token);
        Task<Check> GetCheckByNameAsync(string name, CancellationToken token);
    }
}