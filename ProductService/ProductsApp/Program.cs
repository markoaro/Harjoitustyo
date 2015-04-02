using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsApp
{
    class Program
    {
        // Get an entire entity set.
        static void ListAllProducts(Default.Container container)
        {
            foreach (var p in container.Products)
            {
                Console.WriteLine("{0} {1} {2}", p.Name, p.Price, p.Category);
            }
        }

        static void AddProduct(Default.Container container, ProductService.Models.Product product)
        {
            container.AddToProducts(product);
            var serviceResponse = container.SaveChanges();
            foreach (var operationResponse in serviceResponse)
            {
                Console.WriteLine("Response: {0}", operationResponse.StatusCode);
            }
        }
        //lisätty <
        static void DeleteAllProducts(Default.Container container)
        {
            foreach (var p in container.Products)
            {
                container.DeleteObject(p);
                var serviceResponse = container.SaveChanges();
                foreach (var operationResponse in serviceResponse)
                {
                    Console.WriteLine("Response: {0}", operationResponse.StatusCode);
                }
            }

        }


        static void Main(string[] args)
        {
            // TODO: Replace with your local URI.
            string serviceUri = "http://localhost:56577/";
            var container = new Default.Container(new Uri(serviceUri));

            var product1 = new ProductService.Models.Product()
            {
                Name = "Yo-yo",
                Category = "Toys",
                Price = 4.95M
            };

            var product2 = new ProductService.Models.Product()
            {
                Name = "Doll",
                Category = "Toys",
                Price = 8.16M
            };

            var product3 = new ProductService.Models.Product()
            {
                Name = "Teddy",
                Category = "Toys",
                Price = 9.68M
            };

            AddProduct(container, product1);
            AddProduct(container, product2);
            AddProduct(container, product3);
            ListAllProducts(container);
            
            DeleteAllProducts(container);
            
        }
        }
    }
