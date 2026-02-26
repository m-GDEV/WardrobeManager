using System.Collections;
using System.Linq.Expressions;

namespace WardrobeManager.Api.Tests.Helpers;

// Wraps a plain IEnumerable<T> so that EF Core's ToListAsync() can consume it
// without a real database provider.
internal class AsyncQueryHelper<T>(IEnumerable<T> data) : IQueryable<T>, IAsyncEnumerable<T>
{
    private readonly IQueryable<T> _queryable = data.AsQueryable();

    public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public Type ElementType => _queryable.ElementType;
    public Expression Expression => _queryable.Expression;
    public IQueryProvider Provider => _queryable.Provider;

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new AsyncEnumeratorWrapper<T>(data.GetEnumerator());
}

internal class AsyncEnumeratorWrapper<T>(IEnumerator<T> inner) : IAsyncEnumerator<T>
{
    public T Current => inner.Current;
    public ValueTask DisposeAsync() { inner.Dispose(); return ValueTask.CompletedTask; }
    public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(inner.MoveNext());
}
