using GameVault.API.Controllers;
using GameVault.API.DTO;
using GameVault.API.Models;
using GameVault.API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GameVault.Tests.Controllers
{
    public class GameControllerTests
    {

        /*Test #1*/
        /**/
        [Fact]
        public async Task GetGameById_ReturnsOk_WhenGameExists()
        {
            // Arrange
            var mockService = new Mock<IGameAPIServices>();

            mockService.Setup(s => s.GetGameByIdAsync(1))
                       .ReturnsAsync(new Game { Id = 1, Name = "Halo" });

            var controller = new GameController(mockService.Object);

            // Act
            var result = await controller.GetGameById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var game = Assert.IsType<Game>(okResult.Value);

            Assert.Equal(1, game.Id);
            Assert.Equal("Halo", game.Name);
        }

        /*Tets #2*/
        /*Este tests comprueba que responde correctamente cuando no hay datos, osea un buen manejo de errores*/
        [Fact]
        public async Task GetGameById_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IGameAPIServices>();

            mockService.Setup(s => s.GetGameByIdAsync(1))
                       .ReturnsAsync((Game)null);

            var controller = new GameController(mockService.Object);

            // Act
            var result = await controller.GetGameById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        /*Test #3*/
        /**/
        [Fact]
        public async Task CreateGame_CallsService_AndReturnsOk()
        {
            // Arrange
            var mockService = new Mock<IGameAPIServices>();

            var controller = new GameController(mockService.Object);

            var dto = new CreateGameDto
            {
                Name = "Mario",
                ReleaseDate = DateTime.Now,
                Rating = 9,
                ImageUrl = "url",
                GenreIds = new List<int> { 1 },
                PlatformIds = new List<int> { 1 }
            };

            // Act
            var result = await controller.CreateGame(dto);

            // Assert
            mockService.Verify(s => s.AddGameAsync(dto), Times.Once);
            Assert.IsType<OkResult>(result);
        }


        /*Test #4 */
        /**/
        [Fact]
        public async Task DeleteGame_ReturnsOk_WhenGameExists()
        {
            // Arrange
            var mockService = new Mock<IGameAPIServices>();

            mockService.Setup(s => s.GetGameByIdAsync(1))
                       .ReturnsAsync(new Game());

            var controller = new GameController(mockService.Object);

            // Act
            var result = await controller.DeleteGame(1);

            // Assert
            mockService.Verify(s => s.DeleteGameAsync(1), Times.Once);
            Assert.IsType<OkResult>(result);
        }


        /*Test #5*/
        /**/
        [Fact]
        public async Task DeleteGame_ReturnsNotFound_WhenGameDoesNotExist()
        {
            // Arrange
            var mockService = new Mock<IGameAPIServices>();

            mockService.Setup(s => s.GetGameByIdAsync(1))
                       .ReturnsAsync((Game)null);

            var controller = new GameController(mockService.Object);

            // Act
            var result = await controller.DeleteGame(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
