
using DevopsAssignment.Controllers;
using DevopsAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CurdProject.Tests
{
    public class BrandControllerTests
    {
        [Fact]
        public async Task GetBrands_ReturnsAllBrands()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "GetBrands_Database")
                .Options;
            using (var context = new BrandContext(options))
            {
                context.Brands.Add(new Brand { Id = 1, Name = "Brand 1" });
                context.Brands.Add(new Brand { Id = 2, Name = "Brand 2" });
                context.SaveChanges();
            }

            using (var context = new BrandContext(options))
            {
                var controller = new BrandController(context);

                // Act
                var result = await controller.GetBrands();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnedBrands = Assert.IsAssignableFrom<IEnumerable<Brand>>(okResult.Value);
                Assert.Equal(2, returnedBrands.Count());
            }
        }



        [Fact]
        public async Task PostBrand_CreatesNewBrand()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "PostBrand_Database")
                .Options;
            using (var context = new BrandContext(options))
            {
                var controller = new BrandController(context);
                var newBrand = new Brand { Id = 1, Name = "Brand 1" };

                // Act
                var result = await controller.PostBrand(newBrand);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
                var returnedBrand = Assert.IsType<Brand>(createdAtActionResult.Value);
                Assert.Equal(newBrand.Id, returnedBrand.Id);
            }
        }

        [Fact]
        public async Task UpdateBrand_UpdatesExistingBrand()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "UpdateBrand_Database")
                .Options;
            using (var context = new BrandContext(options))
            {
                context.Brands.Add(new Brand { Id = 1, Name = "Brand 1" });
                context.SaveChanges();
            }

            using (var context = new BrandContext(options))
            {
                var controller = new BrandController(context);
                var updatedBrand = new Brand { Id = 1, Name = "Updated Brand 1" };

                // Act
                var result = await controller.UpdateBrand(1, updatedBrand);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Equal(updatedBrand.Name, context.Brands.Find(1).Name);
            }
        }

        [Fact]
        public async Task DeleteBrand_RemovesBrand()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<BrandContext>()
                .UseInMemoryDatabase(databaseName: "DeleteBrand_Database")
                .Options;
            using (var context = new BrandContext(options))
            {
                context.Brands.Add(new Brand { Id = 1, Name = "Brand 1" });
                context.SaveChanges();
            }

            using (var context = new BrandContext(options))
            {
                var controller = new BrandController(context);

                // Act
                var result = await controller.DeleteBrand(1);

                // Assert
                Assert.IsType<NoContentResult>(result);
                Assert.Null(context.Brands.Find(1));
            }
        }
    }
}
