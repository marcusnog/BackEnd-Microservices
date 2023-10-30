using Auth.Api.Contracts.UserCases;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Security.Claims;

namespace Auth.Api.Services
{
    public class TokenGrantValidator : IExtensionGrantValidator
    {
        private readonly ICampaignConnectorLoginUseCase _validator;

        public TokenGrantValidator(ICampaignConnectorLoginUseCase validator)
        {
            _validator = validator;
        }

        public string GrantType => "token";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            try
            {
                var userToken = context.Request.Raw.Get("token");
                var environment = context.Request.Raw.Get("environment");
                var campaign = context.Request.Raw.Get("campaign");

                if (new string[] { userToken, environment, campaign }.Any(x => string.IsNullOrWhiteSpace(x)))
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                    return;
                }

                var result = await _validator.Login(environment, campaign, userToken);
                if (result == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                    return;
                }

                context.Result = new GrantValidationResult(subject: result.ClientId,
                    authenticationMethod: "custom",
                    claims: result.Claims.Select(x => new Claim(x.Type, x.Value, x.ValueType)));
                return;
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, ex.Message);
                return;
            }
        }
    }
}