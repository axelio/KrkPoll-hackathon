using System.Threading.Tasks;

namespace KrkPoll.Utils.Result
{
    public interface IQuery<T> { }
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}
