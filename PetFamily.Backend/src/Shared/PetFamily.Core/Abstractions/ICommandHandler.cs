﻿using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;


namespace PetFamily.Core.Abstractions;

public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    public Task<Result<TResponse, ErrorList>> Handle(TCommand command, CancellationToken cancelationToken = default);
}

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    public Task<UnitResult<ErrorList>> Handle(TCommand command, CancellationToken cancelationToken = default);
}
