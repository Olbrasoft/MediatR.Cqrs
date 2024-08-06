﻿namespace MediatR.Cqrs.Common;

/// <summary>
/// Defines a Executor to encapsulate command/result
/// </summary>
public interface ICommandExecutor
{
    /// <summary>
    /// Executes the command and returns result.
    /// </summary>
    /// <typeparam name="TResult">The exact type of result to return</typeparam>
    /// <param name="command">Represents a command to be executed</param>
    /// <param name="token">Token to handle any cancellation of the operation.</param>
    /// <returns>The task of result of the executed command</returns>
    Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken token);
}
