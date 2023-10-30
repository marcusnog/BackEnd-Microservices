using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth.Api.Contracts.Services;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace Auth.Api.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        readonly IUserAuthorizationService _userAuthorizationService;

        public ResourceOwnerPasswordValidator(IUserAuthorizationService userAuthorizationService)
        {
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = await _userAuthorizationService.GetUser(context.UserName, context.Request.RequestedScopes.First());
                if (user == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "User does not exist.");
                    return;
                }

                if (user.ClientSecrets?.Contains(context.Password.Sha256()) != true)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect credentials");
                    return;
                }

                if(user.Claims == null)
                {
                    user.Claims = new ();
                }
                if(user.ClientId != null) 
                    user.Claims.Add(new Contracts.DTOs.ClientClaim()
                    {
                        Type = "ClientId",
                        Value = user.ClientId
                    });

                //set the result
                context.Result = new GrantValidationResult(
                    subject: user.UserId,
                    authenticationMethod: "custom",
                    claims: user.Claims.Select(x => new Claim(x.Type, x.Value, x.ValueType)));



                return;
            }
            catch (Exception)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "An error has occurred.");
            }

        }
    }
}
