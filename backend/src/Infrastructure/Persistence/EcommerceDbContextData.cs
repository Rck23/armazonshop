using Ecommerce.Application.Models.Authorization;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceDbContextData
{
    public static async Task LoadDataAsync(EcommerceDbContext context, 
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> roleManager,
        ILoggerFactory loggerFactory)
    {
		try
		{
			//roles de los usuarios
			if (!roleManager.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));
				await roleManager.CreateAsync(new IdentityRole(Role.USER));
			}

			// Usuarios
			if(!userManager.Users.Any())
			{
				var usuarioAdmin = new Usuario
				{
					Nombre = "Ulises",
					Apellido = "Mtz",
					Email = "admin@gmail.com",
					UserName = "UlisesPro",
					Telefono = "4972134567",
					AvatarUrl = "https://i.pinimg.com/originals/77/25/6b/77256b860c831fa87472e6d391af51db.png"
				}; 
				await userManager.CreateAsync(usuarioAdmin, "PasswordAdmin123*");
				await userManager.AddToRoleAsync(usuarioAdmin, Role.ADMIN);


                var usuario = new Usuario
                {
                    Nombre = "Ana",
                    Apellido = "Perez",
                    Email = "ana@gmail.com",
                    UserName = "Ana1908",
                    Telefono = "4098721398",
                    AvatarUrl = "https://i.pinimg.com/originals/10/93/63/109363a9ae3feac1613a4d04e8af5e8c.png"
                };
                await userManager.CreateAsync(usuario, "PasswordAdmin123$");
                await userManager.AddToRoleAsync(usuario, Role.USER);

            }

			// Categorias
			if (!context.Categories!.Any())
			{
				// llamar al archivo JSON para poner informacion 
				var categoryData = File.ReadAllText("../Infrastructure/Data/category.json");
				
				var categories = JsonConvert.DeserializeObject<List<Category>>(categoryData);
				
				await context.Categories!.AddRangeAsync(categories!);

				await context.SaveChangesAsync();
			}

            // Productos
            if (!context.Products!.Any())
            {
                // llamar al archivo JSON para poner informacion 
                var productData = File.ReadAllText("../Infrastructure/Data/product.json");

                var products = JsonConvert.DeserializeObject<List<Product>>(productData);

                await context.Products!.AddRangeAsync(products!);

                await context.SaveChangesAsync();
            }

            // Imagenes
            if (!context.Images!.Any())
            {
                // llamar al archivo JSON para poner informacion 
                var imageData = File.ReadAllText("../Infrastructure/Data/image.json");

                var images = JsonConvert.DeserializeObject<List<Image>>(imageData);

                await context.Images!.AddRangeAsync(images!);

                await context.SaveChangesAsync();
            }

            // Reviews
            if (!context.Reviews!.Any())
            {
                // llamar al archivo JSON para poner informacion 
                var reviewData = File.ReadAllText("../Infrastructure/Data/review.json");

                var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewData);

                await context.Reviews!.AddRangeAsync(reviews!);

                await context.SaveChangesAsync();
            }

            // Paises
            if (!context.Countries!.Any())
            {
                // llamar al archivo JSON para poner informacion 
                var countryData = File.ReadAllText("../Infrastructure/Data/countries.json");

                var countries = JsonConvert.DeserializeObject<List<Country>>(countryData);

                await context.Countries!.AddRangeAsync(countries!);

                await context.SaveChangesAsync();
            }
        }
		catch (Exception e)
		{
			var logger = loggerFactory.CreateLogger<EcommerceDbContextData>();
			logger.LogError(e.Message);
			
		}
    }
}
