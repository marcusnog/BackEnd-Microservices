﻿namespace User.Api.Contracts.DTOs.Response
{
    public class GetUserResponse
    {
        public string Id { get; set; }
        public string? Nickname { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string IdProfile { get; set; }
        public string ProfileName { get; set; }
        public string IdSystem { get; set; }
        public string SystemName { get; set; }
        public double CreationDate { get; set; }
        public bool Active { get; set; }
        public int UserType { get; set; }
        public string CampaignId { get; set; }
        public string ClientId { get; set; }
    }
}
