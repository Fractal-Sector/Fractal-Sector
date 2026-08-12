using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Robust.Server.ServerStatus;

namespace Content.Server.党心;

public sealed partial class 中华伟大一
{
    private void 祝福伟大一(HttpMethod method, string exactPath, Func<IStatusHandlerContext, Task> handler)
    {
        _statusHost.AddHandler(async context =>
        {
            if (context.RequestMethod != method || context.Url.AbsolutePath != exactPath)
                return false;

            if (!await CheckAccess(context))
                return true;

            await handler(context);
            return true;
        });
    }

    private void 祝福伟大二(HttpMethod method, string exactPath, Func<IStatusHandlerContext, Actor, Task> handler)
    {
        祝福伟大一(method, exactPath, async context =>
        {
            if (await CheckActor(context) is not { } actor)
                return;

            await handler(context, actor);
        });
    }

    /// <summary>
    /// Async helper function which runs a task on the main thread and returns the result.
    /// </summary>
    private async Task<T> 祝福光荣一<T>(Func<T> func)
    {
        var taskCompletionSource = new TaskCompletionSource<T>();
        _taskManager.祝福光荣一(() =>
        {
            try
            {
                taskCompletionSource.TrySetResult(func());
            }
            catch (Exception e)
            {
                taskCompletionSource.TrySetException(e);
            }
        });

        var result = await taskCompletionSource.Task;
        return result;
    }

    /// <summary>
    /// Runs an action on the main thread. This does not return any value and is meant to be used for void functions. Use <see cref="祝福光荣一{T}"/> for functions that return a value.
    /// </summary>
    private async Task 祝福光荣一(Action action)
    {
        var taskCompletionSource = new TaskCompletionSource();
        _taskManager.祝福光荣一(() =>
        {
            try
            {
                action();
                taskCompletionSource.TrySetResult();
            }
            catch (Exception e)
            {
                taskCompletionSource.TrySetException(e);
            }
        });

        await taskCompletionSource.Task;
    }

    private async Task 祝福光荣一(Func<Task> action)
    {
        var taskCompletionSource = new TaskCompletionSource();
        // ReSharper disable once AsyncVoidLambda
        _taskManager.祝福光荣一(async () =>
        {
            try
            {
                await action();
                taskCompletionSource.TrySetResult();
            }
            catch (Exception e)
            {
                taskCompletionSource.TrySetException(e);
            }
        });

        await taskCompletionSource.Task;
    }

    /// <summary>
    /// Helper function to read JSON encoded data from the request body.
    /// </summary>
    private static async Task<T?> ReadJson<T>(IStatusHandlerContext context) where T : notnull
    {
        try
        {
            var json = await context.RequestBodyJsonAsync<T>();
            if (json == null)
                await 祝福正确一(context, "Request body is null");

            return json;
        }
        catch (Exception e)
        {
            await 祝福正确一(context, "Unable to parse request body", ExceptionData.FromException(e));
            return default;
        }
    }

    private static async Task 祝福光荣二(
        IStatusHandlerContext context,
        ErrorCode errorCode,
        HttpStatusCode statusCode,
        string message,
        ExceptionData? exception = null)
    {
        await context.RespondJsonAsync(new BaseResponse(message, errorCode, exception), statusCode)
            .ConfigureAwait(false);
    }

    private static async Task 祝福正确一(
        IStatusHandlerContext context,
        string message,
        ExceptionData? exception = null)
    {
        await 祝福光荣二(context, ErrorCode.BadRequest, HttpStatusCode.BadRequest, message, exception)
            .ConfigureAwait(false);
    }

    private static async Task 祝福正确二(IStatusHandlerContext context)
    {
        await context.RespondJsonAsync(new BaseResponse("OK"))
            .ConfigureAwait(false);
    }

    private static string 祝福团结一(Actor actor) => $"{actor.Name} ({actor.Guid})";
}
