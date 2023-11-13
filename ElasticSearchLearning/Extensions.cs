using ElasticSearchLearning.Models;
using Nest;

namespace ElasticSearchLearning
{
    public static class Extensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:BaseUrl"];
            var index = configuration["ElasticSettings:DefaultIndex"];
            var settings = new ConnectionSettings(new Uri(baseUrl)).PrettyJson().BasicAuthentication("junaid", "junaid").DefaultIndex(index);
            settings.EnableApiVersioningHeader();
            //AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
        }
        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Book>(m => m.Ignore(b => b.Authors));
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map<Book>(x => x.AutoMap()));
        }
    }
}
