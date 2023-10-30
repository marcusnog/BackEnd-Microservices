namespace Ms.Campaign.Connector.Contracts.DTO.Response
{
	public class CampaignUserInformation
	{
		public string Name { get; set; }
		public string Document { get; set; }
		public int DocumentType { get; set; }
		public string DateOfBirth { get; set; }
		public string Email { get; set; }
		public CampaignUserContactPhone[] Contact { get; set; }
		public CampaignUserAddress AddressInfo { get; set; }
		public decimal Balance { get; set; }
	}

	public class CampaignUserAddress
	{
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
	}
	public class CampaignUserContactPhone
	{
		public int Type { get; set; }
		public string Number { get; set; }
	}

}
