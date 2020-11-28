using System.Linq;
using KrkPoll.Data.Models;

namespace KrkPoll.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Poll> TryApplyLastIdCondition(this IQueryable<Poll> @this, int? lastId)
            => lastId.HasValue ? @this.Where(p => p.Id > lastId.Value) : @this;

        public static IQueryable<DiscussionPost> TryApplyLastIdCondition(this IQueryable<DiscussionPost> @this, int? lastId)
            => lastId.HasValue ? @this.Where(p => p.Id > lastId.Value) : @this;
    }
}
