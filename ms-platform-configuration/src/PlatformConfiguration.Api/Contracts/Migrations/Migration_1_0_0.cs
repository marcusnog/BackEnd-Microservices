using Ms.Api.Utilities.Extensions;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;

namespace PlatformConfiguration.Api.Contracts.Migrations
{
    public static class Migration_1_0_0
    {
        static int _CLIENT = 0;
        static int _CAMPAIGN = 0;
        static int _PARTNER = 0;
        static int _STORE = 0;
        static Dictionary<string, string> _ids = new()
        {
            { "CLIENT_INTERNO", (++_CLIENT).ToHex() },
            { "CAMPAIGN_FATOR1_CARTAO_PONTOS", (++_CAMPAIGN).ToHex() },
            { "PARTNER_VIA", (++_PARTNER).ToHex() },
            { "PARTNER_GIFTTY", (++_PARTNER).ToHex() },
            { "PARTNER_NETSHOES", (++_PARTNER).ToHex() },
            { "PARTNER_MAGAZINELUIZA", (++_PARTNER).ToHex() },
            { "PARTNER_TRAY", (++_PARTNER).ToHex() },
            { "PARTNER_RAM", (++_PARTNER).ToHex() },
            { "PARTNER_TRANSFEERA", (++_PARTNER).ToHex() },
            { "PARTNER_CELCOIN", (++_PARTNER).ToHex() },
            { "PARTNER_BEFLY", (++_PARTNER).ToHex() },
            { "STORE_VIA_EXTRA", (++_STORE).ToHex() },
            { "STORE_VIA_PONTO", (++_STORE).ToHex() },
            { "STORE_VIA_CASASBAHIA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ABBRACCIO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ACESSOCARD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ALÔBEBÊ", (++_STORE).ToHex() },
            { "STORE_GIFTTY_AMÉRICA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_AMERICANAS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_APPLE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_APPLEBEES", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ARAMIS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ARCHIE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_AREZZO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ARTLLURE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ASSAI", (++_STORE).ToHex() },
            { "STORE_GIFTTY_B2W", (++_STORE).ToHex() },
            { "STORE_GIFTTY_BLIZZARD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CeA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CC", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CABIFY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CACAUSHOW", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CARREFOUR", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CARTAOMULTICASHVIRTUAL", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CARTERS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CENTAURO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CESTASMICHELLI", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CHILLIBEANS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CINEMARK", (++_STORE).ToHex() },
            { "STORE_GIFTTY_COCOBAMBURESTAURANTE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_CVC", (++_STORE).ToHex() },
            { "STORE_GIFTTY_DECATHLON", (++_STORE).ToHex() },
            { "STORE_GIFTTY_DEEZER", (++_STORE).ToHex() },
            { "STORE_GIFTTY_DESTINOFÉRIAS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_DUFRY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_EASYLIVE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ELSYS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ENGLISHLIVE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ENGLISHTOWN", (++_STORE).ToHex() },
            { "STORE_GIFTTY_EOTICA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_EVINO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_EVOLUS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FASTSHOP", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FLOT", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FLOTRENTCARD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FLOTTRAVEL", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FNAC", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FOGODECHAO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_FOTOTICA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GIFTTY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GIFTTYPAYCOMBUSTÍVEL", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GIULIANAFLORES", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GLOBO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GOODCARD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GOOGLE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GOOGLEPLAY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_GRUPOPAOACUCAR", (++_STORE).ToHex() },
            { "STORE_GIFTTY_HAVAIANAS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_HAVAN", (++_STORE).ToHex() },
            { "STORE_GIFTTY_HERING", (++_STORE).ToHex() },
            { "STORE_GIFTTY_HOTELURBANO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_IFOOD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_INGRESSOCOM", (++_STORE).ToHex() },
            { "STORE_GIFTTY_IPLACE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_JOHNNYROCKETS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_KALUNGA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LATAMPASS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LEAGUEOFLEGENDS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LEGO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LEROYMERLIN", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LILIWOOD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LIVUP", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LIVRARIACULTURA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LIVRARIADAVILA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LOCCITANE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_LOOKE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_MADERO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_MCDONALDS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_MCAFEE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_MICROSOFT", (++_STORE).ToHex() },
            { "STORE_GIFTTY_MULTICASH", (++_STORE).ToHex() },
            { "STORE_GIFTTY_MULTIPLUS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_NBA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_NETFLIX", (++_STORE).ToHex() },
            { "STORE_GIFTTY_NETMOVIES", (++_STORE).ToHex() },
            { "STORE_GIFTTY_NETSHOESCARTÕES", (++_STORE).ToHex() },
            { "STORE_GIFTTY_NFL", (++_STORE).ToHex() },
            { "STORE_GIFTTY_NIKE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_OBOTICÁRIO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_OMELHORDAVIDA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_OFNER", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ONODERA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_OUTBACK", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PARIS6", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PBKIDS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PETLOVE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PETZ", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PIZZAHUT", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PLAYKIDS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PLAYSTATIONSTORE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_POLISHOP", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PONTOE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PORTOSEGURO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PORTUS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PREMMIA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_PRIMEPASS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_RAPPI", (++_STORE).ToHex() },
            { "STORE_GIFTTY_RAZERGOLD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_RENNER", (++_STORE).ToHex() },
            { "STORE_GIFTTY_RIACHUELO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_RIBASHARE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_RIHAPPY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SANTALUZIA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SARAIVA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SCHUTZ", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SEMPARAR", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SEPHORA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SHELL", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SHOESTOCK", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SHOPTIME", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SHOULDER", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SPICY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SPOLETO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SPOTIFY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_STMARCHE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_STARBUCKS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_SUBMARINO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TELHANORTE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TERRAÇOITÁLIA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TGIFRIDAYS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TIPTOP", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TNG", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TOKSTOK", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TUDOAZUL", (++_STORE).ToHex() },
            { "STORE_GIFTTY_TWININGS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_UBER", (++_STORE).ToHex() },
            { "STORE_GIFTTY_UBEREATS", (++_STORE).ToHex() },
            { "STORE_GIFTTY_UBOOK", (++_STORE).ToHex() },
            { "STORE_GIFTTY_VALECINE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_VIA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_VIBESAUDE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_VIVAFIDELIDADE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_VIVARA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_WALMART", (++_STORE).ToHex() },
            { "STORE_GIFTTY_WEBURN", (++_STORE).ToHex() },
            { "STORE_GIFTTY_WOLI", (++_STORE).ToHex() },
            { "STORE_GIFTTY_WORLDWINE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_XBOX", (++_STORE).ToHex() },
            { "STORE_GIFTTY_XBOXGOLD", (++_STORE).ToHex() },
            { "STORE_GIFTTY_XBOXLIVE", (++_STORE).ToHex() },
            { "STORE_GIFTTY_YALO", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ZARA", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ZARAHOME", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ZATTINI", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ZEDELIVERY", (++_STORE).ToHex() },
            { "STORE_GIFTTY_ZINZANE", (++_STORE).ToHex() },
            { "STORE_NETSHOES_NETSHOES", (++_STORE).ToHex() },
            { "STORE_MAGAZINELUIZA_MAGAZINELUIZA", (++_STORE).ToHex() },
            { "STORE_TRAY_JEEPGEAR", (++_STORE).ToHex() },
            { "STORE_RAM_RAM", (++_STORE).ToHex() },
            { "STORE_TRANSFEERA_PAGAMENTOSBOLETO", (++_STORE).ToHex() },
            { "STORE_CELCOIN_RECARGASCELULAR", (++_STORE).ToHex() },
            { "STORE_BEFLY_VIAGENS", (++_STORE).ToHex() },
        };
        public static async Task Apply(IConfiguration configuration, 
            ICampaignRepository campaignRepository, 
            IClientRepository clientRepository, 
            IPartnerRepository partnerRepository, 
            IStoreRepository storeRepository,  
            IDbVersionRepository dbVersionRepository)
        {
            Client client;
            Partner partner;
            Campaign campaign;
            Store store;
            // client
            client = new Client()
            {
                Id = _ids["CLIENT_INTERNO"],
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Name = "Interno",
                Documents = new Document[]
                {
                    new ()
                    {
                        Type = "CNPJ",
                        Value = "71509837000109",
                    }
                }                
            };
            await clientRepository.Update(client);

            // campaign
            campaign = new Campaign()
            {
                Id = _ids["CAMPAIGN_FATOR1_CARTAO_PONTOS"],
                Name = "Fator 1: Cartão + Pontos Customizado",
                AllowCardPayment = true,
                AllowedCardPaymentPercentage = 1.0m,
                AllowPointAmountSelection = true,
                ClientId = client.Id,
                CoinConversionFactor = 1,
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
            };
            await campaignRepository.Update(campaign);

            // create partner & stores
            #region VIA

            partner = new Partner()
            {
                Id = _ids["PARTNER_VIA"],
                Name = "VIA",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // VIA -> EXTRA
            store = CreateStore(_ids["STORE_VIA_EXTRA"], "EXTRA", partner.Id, true, true, campaign.Id, "5231");
            await storeRepository.Update(store);
            // VIA -> PONTO
            store = CreateStore(_ids["STORE_VIA_PONTO"], "PONTO", partner.Id, true, true, campaign.Id, "5127");
            await storeRepository.Update(store);
            // VIA -> CASAS BAHIA
            store = CreateStore(_ids["STORE_VIA_CASASBAHIA"], "CASAS BAHIA", partner.Id, true, true, campaign.Id, "2412");
            await storeRepository.Update(store);

            #endregion

            #region GIFTTY

            partner = new Partner()
            {
                Id = _ids["PARTNER_GIFTTY"],
                Name = "GIFTTY",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);


            // GIFTTY -> ABBRACCIO
            store = CreateStore(_ids["STORE_GIFTTY_ABBRACCIO"], "Abbraccio", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ACESSOCARD
            store = CreateStore(_ids["STORE_GIFTTY_ACESSOCARD"], "AcessoCard", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ALÔBEBÊ
            store = CreateStore(_ids["STORE_GIFTTY_ALÔBEBÊ"], "Alô Bebê", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> AMÉRICA
            store = CreateStore(_ids["STORE_GIFTTY_AMÉRICA"], "AMÉRICA", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> AMERICANAS
            store = CreateStore(_ids["STORE_GIFTTY_AMERICANAS"], "AMERICANAS", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> APPLE
            store = CreateStore(_ids["STORE_GIFTTY_APPLE"], "Apple", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> APPLEBEES
            store = CreateStore(_ids["STORE_GIFTTY_APPLEBEES"], "APPLEBEE´S", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ARAMIS
            store = CreateStore(_ids["STORE_GIFTTY_ARAMIS"], "Aramis", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ARCHIE
            store = CreateStore(_ids["STORE_GIFTTY_ARCHIE"], "Archie", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> AREZZO
            store = CreateStore(_ids["STORE_GIFTTY_AREZZO"], "Arezzo", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ARTLLURE
            store = CreateStore(_ids["STORE_GIFTTY_ARTLLURE"], "Artllure", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ASSAÍ
            store = CreateStore(_ids["STORE_GIFTTY_ASSAI"], "Assaí", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> B2W
            store = CreateStore(_ids["STORE_GIFTTY_B2W"], "B2W", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> BLIZZARD
            store = CreateStore(_ids["STORE_GIFTTY_BLIZZARD"], "Blizzard", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CeA
            store = CreateStore(_ids["STORE_GIFTTY_CeA"], "C&A", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CeC
            store = CreateStore(_ids["STORE_GIFTTY_CC"], "C&C", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CABIFY
            store = CreateStore(_ids["STORE_GIFTTY_CABIFY"], "CABIFY", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CACAU SHOW
            store = CreateStore(_ids["STORE_GIFTTY_CACAUSHOW"], "Cacau Show", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CARREFOUR
            store = CreateStore(_ids["STORE_GIFTTY_CARREFOUR"], "CARREFOUR", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CARTAOMULTICASHVIRTUAL
            store = CreateStore(_ids["STORE_GIFTTY_CARTAOMULTICASHVIRTUAL"], "Cartão Multicash Virtual", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CARTERS
            store = CreateStore(_ids["STORE_GIFTTY_CARTERS"], "Carters", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CENTAURO
            store = CreateStore(_ids["STORE_GIFTTY_CENTAURO"], "CENTAURO", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CESTASMICHELLI
            store = CreateStore(_ids["STORE_GIFTTY_CESTASMICHELLI"], "CESTAS MICHELLI", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CHILLIBEANS
            store = CreateStore(_ids["STORE_GIFTTY_CHILLIBEANS"], "CHILLI BEANS", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CINEMARK
            store = CreateStore(_ids["STORE_GIFTTY_CINEMARK"], "Cinemark", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> COCOBAMBURESTAURANTE
            store = CreateStore(_ids["STORE_GIFTTY_COCOBAMBURESTAURANTE"], "Coco Bambu Restaurante", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> CVC
            store = CreateStore(_ids["STORE_GIFTTY_CVC"], "CVC", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> DECATHLON
            store = CreateStore(_ids["STORE_GIFTTY_DECATHLON"], "Decathlon", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> DEEZER
            store = CreateStore(_ids["STORE_GIFTTY_DEEZER"], "Deezer", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> DESTINOFÉRIAS
            store = CreateStore(_ids["STORE_GIFTTY_DESTINOFÉRIAS"], "Destino Férias", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> DUFRY
            store = CreateStore(_ids["STORE_GIFTTY_DUFRY"], "Dufry", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> EASYLIVE
            store = CreateStore(_ids["STORE_GIFTTY_EASYLIVE"], "Easy Live", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ELSYS
            store = CreateStore(_ids["STORE_GIFTTY_ELSYS"], "Elsys", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ENGLISHLIVE
            store = CreateStore(_ids["STORE_GIFTTY_ENGLISHLIVE"], "English Live", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ENGLISHTOWN
            store = CreateStore(_ids["STORE_GIFTTY_ENGLISHTOWN"], "ENGLISH TOWN", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> EOTICA
            store = CreateStore(_ids["STORE_GIFTTY_EOTICA"], "eÓtica", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> EVINO
            store = CreateStore(_ids["STORE_GIFTTY_EVINO"], "Evino", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ÉVOLUS
            store = CreateStore(_ids["STORE_GIFTTY_EVOLUS"], "Évolus", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FASTSHOP
            store = CreateStore(_ids["STORE_GIFTTY_FASTSHOP"], "Fast Shop", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FLOT
            store = CreateStore(_ids["STORE_GIFTTY_FLOT"], "FLOT", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FLOTRENTCARD
            store = CreateStore(_ids["STORE_GIFTTY_FLOTRENTCARD"], "Flot Rent Card", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FLOTTRAVEL
            store = CreateStore(_ids["STORE_GIFTTY_FLOTRENTCARD"], "Flot Travel", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FNAC
            store = CreateStore(_ids["STORE_GIFTTY_FNAC"], "FNAC", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FOGODECHAO
            store = CreateStore(_ids["STORE_GIFTTY_FOGODECHAO"], "Fogo de Chão", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> FOTOTICA
            store = CreateStore(_ids["STORE_GIFTTY_FOTOTICA"], "Fototica", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GIFTTY
            store = CreateStore(_ids["STORE_GIFTTY_GIFTTY"], "Giftty", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GIFTTYPAYCOMBUSTÍVEL
            store = CreateStore(_ids["STORE_GIFTTY_GIFTTYPAYCOMBUSTÍVEL"], "GifttyPay Combustível", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GIULIANAFLORES
            store = CreateStore(_ids["STORE_GIFTTY_GIULIANAFLORES"], "GIULIANA FLORES", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GLOBO
            store = CreateStore(_ids["STORE_GIFTTY_GLOBO"], "Globo+", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GOODCARD
            store = CreateStore(_ids["STORE_GIFTTY_GOODCARD"], "GOOD CARD", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GOOGLE
            store = CreateStore(_ids["STORE_GIFTTY_GOOGLE"], "Google", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GOOGLEPLAY
            store = CreateStore(_ids["STORE_GIFTTY_GOOGLEPLAY"], "Google Play", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> GRUPOPAOACUCAR
            store = CreateStore(_ids["STORE_GIFTTY_GRUPOPAOACUCAR"], "GRUPO PÃO DE AÇUCAR", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> HAVAIANAS
            store = CreateStore(_ids["STORE_GIFTTY_HAVAIANAS"], "Havaianas", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> HAVAN
            store = CreateStore(_ids["STORE_GIFTTY_HAVAN"], "Havan", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> HERING
            store = CreateStore(_ids["STORE_GIFTTY_HERING"], "Hering", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> HOTELURBANO
            store = CreateStore(_ids["STORE_GIFTTY_HOTELURBANO"], "HOTEL URBANO", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> IFOOD
            store = CreateStore(_ids["STORE_GIFTTY_IFOOD"], "iFood", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> INGRESSO.COM
            store = CreateStore(_ids["STORE_GIFTTY_INGRESSOCOM"], "Ingresso.com", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> IPLACE
            store = CreateStore(_ids["STORE_GIFTTY_IPLACE"], "iPlace", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> JHONNYROCKETS
            store = CreateStore(_ids["STORE_GIFTTY_JOHNNYROCKETS"], "Jhonny Rockets", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> KALUNGA
            store = CreateStore(_ids["STORE_GIFTTY_KALUNGA"], "Kalunga", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LATAMPASS
            store = CreateStore(_ids["STORE_GIFTTY_LATAMPASS"], "LATAM PASS", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LEAGUEOFLEGENDS
            store = CreateStore(_ids["STORE_GIFTTY_LEAGUEOFLEGENDS"], "League of Legends ", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LEGO
            store = CreateStore(_ids["STORE_GIFTTY_LEGO"], "LEGO ", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LEROYMERLIN
            store = CreateStore(_ids["STORE_GIFTTY_LEROYMERLIN"], "Leroy Merlin ", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LILIWOOD
            store = CreateStore(_ids["STORE_GIFTTY_LILIWOOD"], "Lili Wood", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LIVUP
            store = CreateStore(_ids["STORE_GIFTTY_LIVUP"], "Liv Up", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LIVRARIACULTURA
            store = CreateStore(_ids["STORE_GIFTTY_LIVRARIACULTURA"], "Livraria Cultura", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LIVRARIADAVILA
            store = CreateStore(_ids["STORE_GIFTTY_LIVRARIADAVILA"], "LIVRARIA DA VILA", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LOCCITANE
            store = CreateStore(_ids["STORE_GIFTTY_LOCCITANE"], "L´Occitane", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> LOOKE
            store = CreateStore(_ids["STORE_GIFTTY_LOOKE"], "Looke", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> MADERO
            store = CreateStore(_ids["STORE_GIFTTY_MADERO"], "Madero", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> MCDONALDS
            store = CreateStore(_ids["STORE_GIFTTY_MCDONALDS"], "Mc Donald's", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> MCAFEE
            store = CreateStore(_ids["STORE_GIFTTY_MCAFEE"], "McAfee", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> MICROSOFT
            store = CreateStore(_ids["STORE_GIFTTY_MICROSOFT"], "Microsoft", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> MULTICASH
            store = CreateStore(_ids["STORE_GIFTTY_MULTICASH"], "Multicash", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> MULTIPLUS
            store = CreateStore(_ids["STORE_GIFTTY_MULTIPLUS"], "Multiplus", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> NBA
            store = CreateStore(_ids["STORE_GIFTTY_NBA"], "NBA", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> NETFLIX
            store = CreateStore(_ids["STORE_GIFTTY_NETFLIX"], "Netflix", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> NETMOVIES
            store = CreateStore(_ids["STORE_GIFTTY_NETMOVIES"], "NetMovies", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> NETSHOESCARTÕES
            store = CreateStore(_ids["STORE_GIFTTY_NETSHOESCARTÕES"], "NETSHOES CARTÕES", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> NFL
            store = CreateStore(_ids["STORE_GIFTTY_NFL"], "NFL", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> NIKE
            store = CreateStore(_ids["STORE_GIFTTY_NIKE"], "Nike", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> OBOTICÁRIO
            store = CreateStore(_ids["STORE_GIFTTY_OBOTICÁRIO"], "O BOTICÁRIO", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> OMELHORDAVIDA
            store = CreateStore(_ids["STORE_GIFTTY_OMELHORDAVIDA"], "O Melhor da Vida", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> OFNER
            store = CreateStore(_ids["STORE_GIFTTY_OFNER"], "Ofner", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ONODERA
            store = CreateStore(_ids["STORE_GIFTTY_ONODERA"], "Onodera", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> OUTBACK
            store = CreateStore(_ids["STORE_GIFTTY_OUTBACK"], "OUTBACK", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PARIS6
            store = CreateStore(_ids["STORE_GIFTTY_PARIS6"], "Paris 6", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PBKIDS
            store = CreateStore(_ids["STORE_GIFTTY_PBKIDS"], "PB Kids", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PETLOVE
            store = CreateStore(_ids["STORE_GIFTTY_LOVE"], "Petlove", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PETZ
            store = CreateStore(_ids["STORE_GIFTTY_PETZ"], "PETZ", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PIZZAHUT
            store = CreateStore(_ids["STORE_GIFTTY_PIZZAHUT"], "Pizza Hut", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PLAYKIDS
            store = CreateStore(_ids["STORE_GIFTTY_PLAYKIDS"], "Play Kids", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PLAYSTATIONSTORE
            store = CreateStore(_ids["STORE_GIFTTY_PLAYSTATIONSTORE"], "PlayStation Store", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> POLISHOP
            store = CreateStore(_ids["STORE_GIFTTY_POLISHOP"], "Polishop", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PONTOE
            store = CreateStore(_ids["STORE_GIFTTY_PONTOE"], "PontoE", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PORTOSEGURO
            store = CreateStore(_ids["STORE_GIFTTY_PORTOSEGURO"], "Porto Seguro", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PORTUS
            store = CreateStore(_ids["STORE_GIFTTY_PORTUS"], "Portus", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PREMMIA
            store = CreateStore(_ids["STORE_GIFTTY_PREMMIA"], "Premmia", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> PRIMEPASS
            store = CreateStore(_ids["STORE_GIFTTY_PRIMEPASS"], "Primepass", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> RAPPI
            store = CreateStore(_ids["STORE_GIFTTY_RAPPI"], "Rappi", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> RAZERGOLD
            store = CreateStore(_ids["STORE_GIFTTY_RAZERGOLD"], "Razer Gold", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> RENNER
            store = CreateStore(_ids["STORE_GIFTTY_RENNER"], "Renner", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> RIACHUELO
            store = CreateStore(_ids["STORE_GIFTTY_RIACHUELO"], "Riachuelo", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> RIBASHARE
            store = CreateStore(_ids["STORE_GIFTTY_RIBASHARE"], "Riba Share", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> RIHAPPY
            store = CreateStore(_ids["STORE_GIFTTY_RIHAPPY"], "RI HAPPY", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SANTALUZIA
            store = CreateStore(_ids["STORE_GIFTTY_SANTALUZIA"], "Santa Luzia", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SARAIVA
            store = CreateStore(_ids["STORE_GIFTTY_SARAIVA"], "SARAIVA", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SCHUTZ
            store = CreateStore(_ids["STORE_GIFTTY_SCHUTZ"], "Schutz", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SEMPARAR
            store = CreateStore(_ids["STORE_GIFTTY_SEMPARAR"], "Sem Parar", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SEPHORA
            store = CreateStore(_ids["STORE_GIFTTY_SEPHORA"], "Sephora", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SHELL
            store = CreateStore(_ids["STORE_GIFTTY_SHELL"], "Shell", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SHOESTOCK
            store = CreateStore(_ids["STORE_GIFTTY_SHOESTOCK"], "Shoestock", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SHOPTIME
            store = CreateStore(_ids["STORE_GIFTTY_SHOPTIME"], "SHOP TIME", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SHOULDER
            store = CreateStore(_ids["STORE_GIFTTY_SHOULDER"], "Shoulder", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SPICY
            store = CreateStore(_ids["STORE_GIFTTY_SPICY"], "Spicy", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SPOLETO
            store = CreateStore(_ids["STORE_GIFTTY_SPOLETO"], "Spoleto", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SPOTIFY
            store = CreateStore(_ids["STORE_GIFTTY_SPOTIFY"], "Spotify", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> STMARCHE
            store = CreateStore(_ids["STORE_GIFTTY_STMARCHE"], "St Marche", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> STARBUCKS
            store = CreateStore(_ids["STORE_GIFTTY_STARBUCKS"], "STARBUCKS", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> SUBMARINO
            store = CreateStore(_ids["STORE_GIFTTY_SUBMARINO"], "SUBMARINO", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TELHANORTE
            store = CreateStore(_ids["STORE_GIFTTY_TELHANORTE"], "TELHA NORTE", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TERRAÇOITÁLIA
            store = CreateStore(_ids["STORE_GIFTTY_TERRAÇOITÁLIA"], "Terraço Itália", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TGIFRIDAYS
            store = CreateStore(_ids["STORE_GIFTTY_TGIFRIDAYS"], "TGI Fridays", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TIPTOP
            store = CreateStore(_ids["STORE_GIFTTY_TIPTOP"], "Tip Top", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TNG
            store = CreateStore(_ids["STORE_GIFTTY_TNG"], "TNG", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TOKSTOK
            store = CreateStore(_ids["STORE_GIFTTY_TOKSTOK"], "TOK&STOK", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TUDOAZUL
            store = CreateStore(_ids["STORE_GIFTTY_TUDOAZUL"], "TudoAzul", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> TWININGS
            store = CreateStore(_ids["STORE_GIFTTY_TWININGS"], "Twinings", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> UBER
            store = CreateStore(_ids["STORE_GIFTTY_UBER"], "Uber", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> UBEREATS
            store = CreateStore(_ids["STORE_GIFTTY_UBEREATS"], "Uber Eats", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> UBOOK
            store = CreateStore(_ids["STORE_GIFTTY_UBOOK"], "Ubook", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> VALECINE
            store = CreateStore(_ids["STORE_GIFTTY_VALECINE"], "VALE CINE", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> VIA
            store = CreateStore(_ids["STORE_GIFTTY_VIA"], "Via", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> VIBESAUDE
            store = CreateStore(_ids["STORE_GIFTTY_VIBESAUDE"], "Vibe Saude", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> VIVAFIDELIDADE
            store = CreateStore(_ids["STORE_GIFTTY_VIVAFIDELIDADE"], "Viva Fidelidade", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> VIVARA
            store = CreateStore(_ids["STORE_GIFTTY_VIVARA"], "Vivara", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> WALMART
            store = CreateStore(_ids["STORE_GIFTTY_WALMART"], "Walmart", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> WEBURN
            store = CreateStore(_ids["STORE_GIFTTY_WEBURN"], "Weburn", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> WOLI
            store = CreateStore(_ids["STORE_GIFTTY_WOLI"], "Woli", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> WORLDWINE
            store = CreateStore(_ids["STORE_GIFTTY_WORLDWINE"], "World Wine", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> XBOX
            store = CreateStore(_ids["STORE_GIFTTY_XBOX"], "Xbox", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> XBOXGOLD
            store = CreateStore(_ids["STORE_GIFTTY_XBOXGOLD"], "Xbox Gold", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> XBOXLIVE
            store = CreateStore(_ids["STORE_GIFTTY_XBOXLIVE"], "Xbox Live", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> YALO
            store = CreateStore(_ids["STORE_GIFTTY_YALO"], "YALO", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ZARA
            store = CreateStore(_ids["STORE_GIFTTY_ZARA"], "Zara", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ZARAHOME
            store = CreateStore(_ids["STORE_GIFTTY_ZARAHOME"], "Zara Home", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ZATTINI
            store = CreateStore(_ids["STORE_GIFTTY_ZATTINI"], "Zattini", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ZEDELIVERY
            store = CreateStore(_ids["STORE_GIFTTY_ZEDELIVERY"], "Zé Delivery", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            // GIFTTY -> ZINZANE
            store = CreateStore(_ids["STORE_GIFTTY_ZINZANE"], "Zinzane", partner.Id, true, true, campaign.Id, "L85");
            await storeRepository.Update(store);

            #endregion

            #region NETSHOES

            partner = new Partner()
            {
                Id = _ids["PARTNER_NETSHOES"],
                Name = "NETSHOES",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // NETSHOES -> NETSHOES
            store = CreateStore(_ids["STORE_NETSHOES_NETSHOES"], "NETSHOES", partner.Id, true, true, campaign.Id, "25025066");
            await storeRepository.Update(store);

            #endregion

            #region Magazine Luiza

            partner = new Partner()
            {
                Id = _ids["PARTNER_MAGAZINELUIZA"],
                Name = "Magazine Luiza",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // MAGAZINELUIZA -> MAGAZINELUIZA
            store = CreateStore(_ids["STORE_MAGAZINELUIZA_MAGAZINELUIZA"], "Magazine Luiza", partner.Id, true, true, campaign.Id, "1488"); // code copied from campaign "FORÇA V"
            await storeRepository.Update(store);

            #endregion

            #region Tray

            partner = new Partner()
            {
                Id = _ids["PARTNER_TRAY"],
                Name = "Tray",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // TRAY -> JEEPGEAR
            store = CreateStore(_ids["STORE_TRAY_JEEPGEAR"], "Jeep Gear", partner.Id, true, true, campaign.Id, "912844"); // code copied from campaign "DNA Jeep"
            await storeRepository.Update(store);

            #endregion

            #region Ram Store

            partner = new Partner()
            {
                Id = _ids["PARTNER_RAM"],
                Name = "Ram Store",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // RAM -> RAM
            store = CreateStore(_ids["STORE_RAM_RAM"], "Ram Store", partner.Id, true, true, campaign.Id, "990573"); // code copied from campaign "DNA Jeep"
            await storeRepository.Update(store);

            #endregion

            #region Transfeera

            partner = new Partner()
            {
                Id = _ids["PARTNER_TRANSFEERA"],
                Name = "Transfeera",
                Active = true,
                AcceptCardPayment = false,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // TRANSFEERA -> PAGAMENTOSBOLETO
            store = CreateStore(_ids["STORE_TRANSFEERA_PAGAMENTOSBOLETO"], "Transfeera Pagamento", partner.Id, false, false, campaign.Id, ""); 
            await storeRepository.Update(store);

            #endregion

            #region Celcoin

            partner = new Partner()
            {
                Id = _ids["PARTNER_CELCOIN"],
                Name = "Celcoin",
                Active = true,
                AcceptCardPayment = false,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // CELCOIN -> RECARGASCELULAR
            store = CreateStore(_ids["STORE_CELCOIN_RECARGASCELULAR"], "Celcoin Recargas", partner.Id, false, false, campaign.Id, "");
            await storeRepository.Update(store);

            #endregion

            #region BeFly

            partner = new Partner()
            {
                Id = _ids["PARTNER_BEFLY"],
                Name = "BeFly",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // BEFLY -> VIAGENS
            store = CreateStore(_ids["STORE_BEFLY_VIAGENS"], "BeFly Viagens", partner.Id, true, false, campaign.Id, "");
            await storeRepository.Update(store);

            #endregion

            await dbVersionRepository.SetVersion("1.0.0");
        }
        static Store CreateStore(string id, string name, string partnerId, bool acceptCardPayment, bool haveProducts, string campaignId, string storeCode)
        {
            return new ()
            {
                Id = id,
                Name = name,
                PartnerId = partnerId,
                AcceptCardPayment = acceptCardPayment,
                HaveProducts = haveProducts,
                CampaignConfiguration = new StoreCampaignConfiguration[]
                {
                    new StoreCampaignConfiguration()
                    {
                        CampaignId = campaignId,
                        StoreCode = storeCode
                    }
                },
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),

            };
        }
    }
}
