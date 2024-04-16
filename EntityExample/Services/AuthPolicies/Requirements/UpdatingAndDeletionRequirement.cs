using Microsoft.AspNetCore.Authorization;

namespace EntityExample.Services.AuthPolicies.Requirements
{
    public class UpdatingAndDeletionRequirement : IAuthorizationRequirement
    {
        public UpdatingAndDeletionRequirement(string actionOne, string actionTwo)
        {
            ActionOne = actionOne;
            ActionTwo = actionTwo;
        }

        public string ActionOne { get; }
        public string ActionTwo { get; }
    }
}
