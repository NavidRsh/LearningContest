using System;
using System.Collections.Generic;
using System.Text;

namespace LearningContest.Domain.Common.User
{
    public sealed class AccessTokenDto
    {
        public string Token { get; }
        public int ExpiresIn { get; }

        public AccessTokenDto(string token, int expiresIn)
        {
            Token = token;
            ExpiresIn = expiresIn;
        }
    }
}
