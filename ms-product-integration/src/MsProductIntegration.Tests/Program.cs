using Bogus;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Platform.Events.Core.Contracts.Product;
using Platform.Events.Core.Contracts.Enums;

class Program
{
    private const string QueueName = "products";
    private const string ConnectionString = "Endpoint=sb://hml-reconhece.servicebus.windows.net/;SharedAccessKeyName=product-producer;SharedAccessKey=VWbJs/PBl7zlThgSOm5Zq5JertiTxW0JA1RgVhr3bzg=;EntityPath=products";

    static async Task Main(string[] args)
    {

        //Console.WriteLine("Let's queue some products, how many do you want?");

        //var requestedAmount = DetermineAmount();
        //await Queue(requestedAmount);
        //var createProduct = GenerateUpdateSkuEventV1();
        //var str = JsonConvert.SerializeObject(createProduct);

        //var createProduct = JsonConvert.DeserializeObject<CreateProductEventV1>("{\"ApiVersion\":\"CreateProduct/v1\",\"Code\":\"furflesitem\",\"DisplayName\":\"Quia laboriosam non atque et nesciunt.\",\"Description\":\"Velit et ipsum dolor doloremque soluta. Repellat ut totam similique. Consequatur quidem cumque consequatur id accusantium. Blanditiis est vel aspernatur similique veritatis itaque quia. Consequatur vel nihil dolores. Neque vero consequatur et officia quia quia quisquam eius deleniti.\",\"CategoryId\":\"00000000000000004A8EE477\",\"StoreId\":\"000000000000000006E71CB6\",\"Images\":[{\"Size\":2,\"Url\":\"https://orlando.name\"},{\"Size\":1,\"Url\":\"http://yasmin.info\"},{\"Size\":2,\"Url\":\"https://deion.info\"}]}");
        //var createProduct = JsonConvert.DeserializeObject<UpdateProductEventV1>("{\"ApiVersion\":\"UpdateProduct/v1\",\"Id\":\"61f44ae966601a80b18a6c32\",\"Code\":\"1n0xuu1a6a\",\"DisplayName\":\"Mollitia vel non.\",\"Description\":\"Dolores animi quam ea et voluptas. Debitis est nostrum velit et earum accusantium nulla et. Quasi id sit id neque saepe et est dicta saepe. Et placeat aut aut est animi vel. Laudantium magnam provident veritatis voluptas quibusdam impedit. Doloribus facere aut cum autem numquam maxime.\",\"CategoryId\":\"00000000000000007542F604\",\"StoreId\":\"00000000000000007AAE92DE\",\"Active\":true,\"Images\":[{\"Size\":0,\"Url\":\"http://gabrielle.biz\"},{\"Size\":2,\"Url\":\"http://colleen.info\"},{\"Size\":1,\"Url\":\"https://maddison.net\"}]}");
        var createProduct = JsonConvert.DeserializeObject<CreateSkuEventV1>("{\"ApiVersion\":\"CreateSku/v1\",\"Id\":\"0000000000000000C5059496\",\"ProductId\":\"61f54e5aee34b632ddaa87eb\",\"Code\":\"hw31v89xw5\",\"Active\":false,\"ListPrice\":3754.605643685127190,\"SalePrice\":8018.963953663342060,\"Attributes\":{},\"Tags\":null}");
        //var createProduct = JsonConvert.DeserializeObject<UpdateSkuEventV1>("{\"ApiVersion\":\"UpdateSku/v1\",\"Id\":\"0000000000000000C5059496\",\"ProductId\":\"61f54e5aee34b632ddaa87eb\",\"Code\":\"hw31v89xw5\",\"Active\":false,\"ListPrice\":3754.605643685127190,\"SalePrice\":8018.963953663342060,\"Attributes\":{},\"Tags\":null}");
        await Queue(createProduct);
        Console.WriteLine("That's it, see you later!");
    }

    static async Task Queue(object obj)
    {
        var serviceBusClient = new ServiceBusClient(ConnectionString);
        var serviceBusSender = serviceBusClient.CreateSender(QueueName);
        var rawOrder = JsonConvert.SerializeObject(obj);
        var message = new ServiceBusMessage(rawOrder);

        await serviceBusSender.SendMessageAsync(message);

    }
    private static async Task Queue(int requestedAmount, bool a)
    {
        var serviceBusClient = new ServiceBusClient(ConnectionString);
        var serviceBusSender = serviceBusClient.CreateSender(QueueName);

        for (int currentOrderAmount = 0; currentOrderAmount < requestedAmount; currentOrderAmount++)
        {
            var product = GenerateCreateProductEventV1();
            var rawOrder = JsonConvert.SerializeObject(product);
            var message = new ServiceBusMessage(rawOrder);

            await serviceBusSender.SendMessageAsync(message);
        }
    }

    private static CreateProductEventV1 GenerateCreateProductEventV1()
    {
        List<CreateProductEventV1Image> images = new();
        var imageGenerator = new Faker<CreateProductEventV1Image>()
            .RuleFor(u => u.Size, (f, u) => f.PickRandom<ProductImageSize>())
            .RuleFor(u => u.Url, (f, u) => f.Internet.Url());
        for (var i = 0; i < 3; i++)
            images.Add(imageGenerator.Generate());

        var orderGenerator = new Faker<CreateProductEventV1>()
            .RuleFor(u => u.Code, f => f.Random.AlphaNumeric(10))
            .RuleFor(u => u.DisplayName, f => f.Lorem.Sentence())
            .RuleFor(u => u.CategoryId, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.Description, f => f.Lorem.Paragraph())
            .RuleFor(u => u.StoreId, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.Images, images.ToArray());

        return orderGenerator.Generate();
    }
    private static UpdateProductEventV1 GenerateUpdateProductEventV1()
    {
        List<CreateProductEventV1Image> images = new();
        var imageGenerator = new Faker<CreateProductEventV1Image>()
            .RuleFor(u => u.Size, (f, u) => f.PickRandom<ProductImageSize>())
            .RuleFor(u => u.Url, (f, u) => f.Internet.Url());
        for (var i = 0; i < 3; i++)
            images.Add(imageGenerator.Generate());

        var orderGenerator = new Faker<UpdateProductEventV1>()
            .RuleFor(u => u.Id, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.CategoryId, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.Code, f => f.Random.AlphaNumeric(10))
            .RuleFor(u => u.DisplayName, f => f.Lorem.Sentence())
            .RuleFor(u => u.CategoryId, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.Description, f => f.Lorem.Paragraph())
            .RuleFor(u => u.StoreId, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.Active, f => f.Random.Bool())
            .RuleFor(u => u.Images, images.ToArray());

        return orderGenerator.Generate();
    }

    private static UpdateSkuEventV1 GenerateUpdateSkuEventV1()
    {
        var orderGenerator = new Faker<UpdateSkuEventV1>()
            .RuleFor(u => u.Id, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.ProductId, f => f.Random.Int().ToString("X24"))
            .RuleFor(u => u.Code, f => f.Random.AlphaNumeric(10))
            .RuleFor(u => u.Active, f => f.Random.Bool())
            .RuleFor(u => u.ListPrice, f => f.Random.Decimal(10,10000))
            .RuleFor(u => u.SalePrice, f => f.Random.Decimal(10,10000));

        return orderGenerator.Generate();
    }

    private static int DetermineAmount()
    {
        var rawAmount = Console.ReadLine();
        if (int.TryParse(rawAmount, out int amount))
        {
            return amount;
        }

        Console.WriteLine("That's not a valid amount, let's try that again");
        return DetermineAmount();
    }
}
