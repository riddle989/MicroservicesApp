using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Iphone A",
                    Summary = "asd",
                    ImageFile = "ImageFile",
                    Category= "cat1",
                    Price = 950.1M,
                    Description = "asd"
                },
                new Product()
                {
                    Name = "Iphone B",
                    Summary = "asd",
                    ImageFile = "ImageFile",
                    Category= "cat2",
                    Price = 950.1M,
                    Description = "asd"
                },
                new Product()
                {
                    Name = "Iphone C",
                    Summary = "asd",
                    ImageFile = "ImageFile",
                    Category= "cat3",
                    Price = 950.1M,
                    Description = "asd"
                },
                new Product()
                {
                    Name = "Iphone D",
                    Summary = "asd",
                    ImageFile = "ImageFile",
                    Category= "cat4",
                    Price = 950.1M,
                    Description = "asd"
                },
                new Product()
                {
                    Name = "Iphone X",
                    Summary = "asd",
                    Category= "cat5",
                    ImageFile = "ImageFile",
                    Price = 950.1M,
                    Description = "asd"
                }
            };
        }
    }
}
