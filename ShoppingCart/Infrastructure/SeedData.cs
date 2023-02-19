using Microsoft.EntityFrameworkCore;
using ShoppingCart.Models;

namespace ShoppingCart.Infrastructure
{
        public class SeedData
        {
                public static void SeedDatabase(DataContext context)
                {
                        context.Database.Migrate();

                        if (!context.Products.Any())
                        {
                Category Cakes = new Category { Name = "Cakes", Slug = "cakes" };
                Category Shirts = new Category { Name = "Shirts", Slug = "shirts" };
                Category Flowers = new Category { Name = "Flowers", Slug = "flowers" };
                               
                                

                context.Products.AddRange(
                                        
                                        new Product
                                        {
                                                Name = "White shirt",
                                                Slug = "white-shirt",
                                                Description = "White shirt",
                                                Price = 5.99M,
                                                Category = Shirts,
                                                Image = "white shirt.jpg"
                                        },
                                        new Product
                                        {
                                                Name = "Black shirt",
                                                Slug = "black-shirt",
                                                Description = "Black shirt",
                                                Price = 7.99M,
                                                Category = Shirts,
                                                Image = "black shirt.jpg"
                                        },
                                        new Product
                                        {
                                                Name = "Yellow shirt",
                                                Slug = "yellow-shirt",
                                                Description = "Yellow shirt",
                                                Price = 11.99M,
                                                Category = Shirts,
                                                Image = "yellow shirt.jpg"
                                        },
                                        new Product
                                        {
                                                Name = "Grey shirt",
                                                Slug = "grey-shirt",
                                                Description = "Grey shirt",
                                                Price = 12.99M,
                                                Category = Shirts,
                                                Image = "grey shirt.jpg"
                                        }
                                );

                                context.SaveChanges();
                        }
                }
        }
}