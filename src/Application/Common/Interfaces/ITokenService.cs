﻿namespace CleanApi.Application.Common.Interfaces;

public interface ITokenService
{
    string? CreateJwtSecurityToken(string userId);
}