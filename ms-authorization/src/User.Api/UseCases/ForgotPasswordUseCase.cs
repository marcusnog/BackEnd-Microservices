using System.Text;
using User.Api.Contracts.DTOs;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.Repositories;
using User.Api.Contracts.UseCases;
using User.Api.Extensions;

namespace User.Api.UseCases
{
    public class ForgotPasswordUseCase : IForgotPasswordUseCase
    {
        readonly string _COMMUNICATION_API_URL;
        readonly string _COMMUNICATION_TEMPLATE_ID;
        readonly string _SMTP_ID;
        readonly IConfiguration _configuration;
        readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        readonly IUserAdministratorRepository _userRepository;
        readonly IUserParticipantRepository _userParticipantRepository;
        public ForgotPasswordUseCase(IConfiguration configuration, IPasswordRecoveryRepository passwordRecoveryRepository, IUserAdministratorRepository userRepository,
            IUserParticipantRepository userParticipantRepository)
        {
            _configuration = configuration;
            _passwordRecoveryRepository = passwordRecoveryRepository;
            _userRepository = userRepository;
            _userParticipantRepository = userParticipantRepository;

            _COMMUNICATION_API_URL = _configuration.GetValue<string>("COMMUNICATION_API");
            _COMMUNICATION_TEMPLATE_ID = _configuration.GetValue<string>("COMMUNICATION_TEMPLATE_ID");
            _SMTP_ID = _configuration.GetValue<string>("SMTP_ID");
        }
        static Random random = new();
        static string RandomCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task StartRecoveryPassword(string lang, string userId, string name, string ip, string email, Contracts.DTOs.System system)
        {
            PasswordRecovery passwordRecovery; 
            do
            {
                passwordRecovery = await _passwordRecoveryRepository.GetByUser(userId);
                if (passwordRecovery != null)
                    await _passwordRecoveryRepository.Delete(passwordRecovery.Id);
            }
            while (passwordRecovery != null);

            passwordRecovery = new PasswordRecovery()
            {
                ExpiresIn = DateTime.UtcNow.AddHours(system.RecoveryExpiration).ToUnixTimestamp(),
                UserId = userId,
                Code = RandomCode(),
                Ip = ip,
                Active = true
            };
            await _passwordRecoveryRepository.Create(passwordRecovery);

            using HttpClient client = new();
            var response = await client.PostAsync(String.Format(_COMMUNICATION_API_URL, lang, _COMMUNICATION_TEMPLATE_ID), new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                idSmtp = _SMTP_ID,
                to = email,
                model = new SendPasswordReconveryEmailRequest()
                {
                    imageUrl = string.Format(system.Logo, system.Url),
                    name = name,
                    email = email,
                    code = passwordRecovery.Code,
                    link = string.Format(system.RecoveryUrl, system.Url, passwordRecovery.Id),
                    expiration = system.RecoveryExpiration.ToString(),
                    contactEmail = system.ContactEmail,
                    accountType = system.Name
                }
            }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode) return;

            var content = await response.Content.ReadAsStringAsync();
            throw new Exception(content);
        }
        public async Task<bool> ValidateToken(string tokenId)
        {
            return await _passwordRecoveryRepository.Validate(tokenId);
        }
        public async Task<bool> ValidateToken(string tokenId, string code)
        {
            return await _passwordRecoveryRepository.Validate(tokenId, code);
        }
        public async Task UpdatePassword(string tokenId, string password)
        {
            var token = await _passwordRecoveryRepository.Get(tokenId);
            if (token == null) return;

            await _userRepository.UpdatePassword(token.UserId, password);
            await _passwordRecoveryRepository.Delete(tokenId);
        }

        public async Task UpdatePasswordUserParticipant(string tokenId, string password)
        { 
            var token = await _passwordRecoveryRepository.Get(tokenId);
            if (token == null) return;

            await _userParticipantRepository.UpdatePassword(token.UserId, password);
            await _passwordRecoveryRepository.Delete(tokenId);
        }
    }
}
